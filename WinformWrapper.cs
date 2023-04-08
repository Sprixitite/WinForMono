using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForMono {

    public abstract class WinformWrapper {

        public delegate void OnTransformInvalidated();
        public event OnTransformInvalidated transform_invalidated;
        protected void invalidate_size() => transform_invalidated.Invoke();
        protected void invalidate_size(WinformWrapper o) => o.transform_invalidated.Invoke();
        protected void invalidate_size(object _s, EventArgs _e) => transform_invalidated.Invoke();

        public static readonly WinformWrapper NULL = new WrapperNull();
        private sealed class WrapperNull : UIWindow { public WrapperNull() { } protected override void on_transform_invalidated() {} }

        public void fake_event_user() {}

        public WinformWrapper() {
            elements = new List<UIElement>();
            _base_parent = WinformWrapper.NULL;
            transform_invalidated += fake_event_user;
        }

        ~WinformWrapper() {
            transform_invalidated -= fake_event_user;
        }

        protected virtual void add_element(UIElement element) {

            if (underlying.Controls.Contains(element.underlying)) throw new Exception("Attempted to add control to a " + this.GetType().Name + " more than once!");

            WinformWrapper window = get_window();
            
            if (!window.Equals(WinformWrapper.NULL)) element.underlying.Click += ((UIWindow)window).element_clicked;

            // window.underlying.SuspendLayout();
            Control ex_parent = element.parent.underlying;
            ex_parent.SuspendLayout();

            // If the parent isn't null, remove the element from it's old parent
            if (element.parent != WinformWrapper.NULL) { element.parent.remove_element(element); }

            // Parent the element to ourselves
            elements.Add(element);
            underlying.Controls.Add(element.underlying);

            this.transform_invalidated += element._base_on_transform_invalidated;
            element._base_parent = this;
            if (element.underlying != null) invalidate_size();

            ex_parent.ResumeLayout(true);

        }

        protected virtual void remove_element(UIElement element) {

            if (!underlying.Controls.Contains(element.underlying)) throw new Exception("Attempted to remove control from a " + this.GetType().Name + " that did not exist!");

            WinformWrapper window = get_window();
            Control ex_parent = element.parent.underlying;

            if (!window.Equals(WinformWrapper.NULL)) element.underlying.Click -= ((UIWindow)window).element_clicked;

            ex_parent.SuspendLayout();

            elements.Remove(element);
            underlying.Controls.Remove(element.underlying);

            element.parent.transform_invalidated -= element._base_on_transform_invalidated;
            element._base_parent = WinformWrapper.NULL;

            ex_parent.ResumeLayout(true);

        }

        public bool contains_element(UIElement element) => underlying.Controls.Contains(element.underlying);

        public void refresh() {
            bool vis_tmp = visible;
            visible = true;
            _base_on_transform_invalidated();
            visible = vis_tmp;
        }

        public virtual Control underlying {
            get;
            set;
        }

        public bool visible {
            get => underlying.Visible;
            set => underlying.Visible = value;
        }

        // Recursion is OP
        public WinformWrapper get_window() {

            if (this.GetType() == typeof(UIWindow) || this.GetType() == typeof(WrapperNull)) return this;
            else return parent.get_window();

        }

        public virtual WinformWrapper parent {
            get => _base_parent;
            set => value.add_element((UIElement)this);
        }

        private WinformWrapper _base_parent;

        protected void _base_on_transform_invalidated() {

            if (!visible) return;

            parent.underlying.SuspendLayout();

            on_transform_invalidated();

            invalidate_size();

            underlying.Refresh();

            parent.underlying.ResumeLayout();

        }

        protected virtual void on_transform_invalidated() {}

        protected List<UIElement> elements;

    }

}