using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForMono {

    public class UIComboBox<T> : UIElement {

        public UIComboBox() {
            derived_underlying = new ComboBox();
            derived_underlying.SelectedIndexChanged += _underlying_selection_handler;
            selection_changed += fake_event_user;
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();
        }

        private void _underlying_selection_handler(object _s, EventArgs _e) {
            selection_changed.Invoke();
        }

        public override Control underlying {
            get => (Control)derived_underlying;
            set => derived_underlying = (ComboBox)value;
        }
        private ComboBox derived_underlying;

        private class UIComboBoxItem<T> {
            private string user_name;
            private T value;

            public UIComboBoxItem(string name, T _value) { user_name = name; value = _value; }

            public static implicit operator T(UIComboBoxItem<T> self) => self.value;

            public override string ToString() {
                return user_name;
            }
        }

        public ComboBox.ObjectCollection underlying_items {
            get => derived_underlying.Items;
            //private set => derived_underlying.Items = value;
        }

        public T selected_val {
            get { 
                if (derived_underlying.SelectedItem == null) return (dynamic)null;
                else return (UIComboBoxItem<T>)derived_underlying.SelectedItem;
            }
        }

        public int selected_index {
            get => derived_underlying.SelectedIndex;
            set => derived_underlying.SelectedIndex = value;
        }

        public void add_item(T item, int? pos=null) => underlying_items.Insert(pos ?? underlying_items.Count, item);
        public void add_item(string name, T item, int? pos=null) => underlying_items.Insert(pos ?? underlying_items.Count, new UIComboBoxItem<T>(name, item));
        public void add_items(T[] items, int? pos=null) {
            pos = pos ?? underlying_items.Count;
            for (int i=0; i<items.Length; i++) {
                add_item(items[i], pos+i);
            }
        }
        public void add_items(string[] names, T[] items, int? pos=null) {
            if (names.Length != items.Length) throw new Exception("Cannot have a non-equal amount of items and names!");
            pos = pos ?? underlying_items.Count;
            for (int i=0; i<names.Length; i++) {
                add_item(names[i], items[i], pos+i);
            }
        }
        public void add_items(Dictionary<string, T> items, int? pos=null) {
            pos = pos ?? underlying_items.Count;
            int i=0;
            foreach (string index in items.Keys) {
                add_item(index, items[index], pos+i);
                i++;
            }
        }

        public void set_items(T[] items) {
            underlying_items.Clear();
            add_items(items);
        }
        public void set_items(string[] names, T[] items) {
            if (names.Length != items.Length) throw new Exception("Cannot have a non-equal amount of items and names!");
            underlying_items.Clear();
            add_items(names, items);
        }
        public void set_items(Dictionary<string, T> items) {
            underlying_items.Clear();
            add_items(items);
        }

        public void remove_item(int index) => underlying_items.RemoveAt(index);
        public void remove_item(T item, bool search_named=false) {
            switch (search_named) {
                case true:
                    foreach (object o in underlying_items) {
                        if (!(o is T || o is UIComboBoxItem<T>)) continue; // I have no idea when this could occur
                        if (((T)o).Equals(item)) { underlying_items.Remove(o); break; }
                    }
                    break;
                case false:
                    underlying_items.Remove(item);
                    break;
            }
        }

        public void bind_source(object source) {
            derived_underlying.DataSource = source;
        }

        public void refresh_contents() {
            object _placeholder = derived_underlying.DataSource;
            bind_source(null);
            bind_source(_placeholder);
        }

        protected override void on_transform_invalidated() {
            base.on_transform_invalidated();
            refresh_contents();
        }

        public delegate void OnUIComboBoxSelectionChanged();
        public event OnUIComboBoxSelectionChanged selection_changed;

    }

}