using System;
using System.Reflection;
using System.Collections.Generic;

namespace WinForMono {

    public class UIBuilder<T>
        where T : UIElement
    {

        public UIBuilder() {
            fields_values = new Dictionary<string, object>();
        }

        public UIBuilder<T> set_field(string field_name, object field_value) {
            if (is_immutable) throw new Exception("Attempt to modify immutable UIBuilder!");
            fields_values[field_name] = field_value;
            return this;
        }

        public UIBuilder<T> set_field(string field_name, Func<int, object> field_value) {
            if (is_immutable) throw new Exception("Attempt to modify immutable UIBuilder!");
            fields_values[field_name] = field_value;
            return this;
        }

        private object get_val(string k) {
            if (fields_values[k].GetType() == typeof(Func<int, object>)) {
                return ((Func<int, object>)fields_values[k]).Invoke(times_built++);
            } else {
                return fields_values[k];
            }
        }

        public T build(params object[] objects) {

            T built = (T)Activator.CreateInstance(typeof(T), objects);

            foreach (string field in fields_values.Keys) {
                built.GetType().GetField(field)?.SetValue(built, get_val(field));
                built.GetType().GetProperty(field)?.SetValue(built, get_val(field));
            }

            return built;

        }

        public void make_immutable() { is_immutable = true; }

        private bool is_immutable = false;
        private int times_built = 0;

        private Dictionary<string, object> fields_values;

        public UIBuilder<T> clone() {
            UIBuilder<T> cloned = new UIBuilder<T>();
            foreach (string k in fields_values.Keys) { cloned.fields_values[k] = this.fields_values[k]; }
            return cloned;
        }

    }

}