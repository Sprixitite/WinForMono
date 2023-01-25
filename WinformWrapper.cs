using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForMono {

    public abstract class WinformWrapper {

        public delegate void OnTransformInvalidated();
        public event OnTransformInvalidated transform_invalidated;
        protected void invalidate_size() => transform_invalidated.Invoke();
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

        public void add_element(UIElement element) {

            if (underlying.Controls.Contains(element.underlying)) throw new Exception("Attempted to add control to a " + this.GetType().Name + " more than once!");

            WinformWrapper window = get_window();

            window.underlying.SuspendLayout();

            // If the parent isn't null, remove the element from it's old parent
            if (element.parent != WinformWrapper.NULL) { element.parent.remove_element(element); }

            elements.Add(element);
            underlying.Controls.Add(element.underlying);

            element.parent = this;

            window.underlying.ResumeLayout(true);

        }

        public void remove_element(UIElement element) {

            if (!underlying.Controls.Contains(element.underlying)) throw new Exception("Attempted to remove control from a " + this.GetType().Name + " that did not exist!");

            WinformWrapper window = get_window();

            window.underlying.SuspendLayout();

            elements.Remove(element);
            underlying.Controls.Remove(element.underlying);

            element.parent = WinformWrapper.NULL;

            window.underlying.ResumeLayout(true);

        }

        public bool contains_element(UIElement element) => underlying.Controls.Contains(element.underlying);

        public virtual Control underlying {
            get;
            set;
        }

        // Recursion is OP
        public WinformWrapper get_window() {

            if (this.GetType() == typeof(UIWindow) || this.GetType() == typeof(WrapperNull)) return this;
            else return parent.get_window();

        }

        public virtual WinformWrapper parent {
            get => _base_parent;
            protected set {
                if (_base_parent != WinformWrapper.NULL) _base_parent.transform_invalidated -= _base_on_transform_invalidated;
                if (value != WinformWrapper.NULL) value.transform_invalidated += _base_on_transform_invalidated;
                _base_parent = value;
                if (underlying != null) _base_on_transform_invalidated();
            }
        }

        private WinformWrapper _base_parent;

        private void _base_on_transform_invalidated() {

            //Console.WriteLine("Transform Invalidated!");

            WinformWrapper window = get_window();

            window.underlying.SuspendLayout();

            on_transform_invalidated();

            invalidate_size();

            window.underlying.ResumeLayout(true);

        }

        protected virtual void on_transform_invalidated() {}

        protected List<UIElement> elements;

    }

}