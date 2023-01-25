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
            if (fields_values.ContainsKey(field_name)) fields_values[field_name] = field_value;
            else fields_values.Add(field_name, field_value);
            return this;
        }

        public T build(params object[] objects) {

            T built = (T)Activator.CreateInstance(typeof(T), objects);

            foreach (string field in fields_values.Keys) {
                built.GetType().GetField(field)?.SetValue(built, fields_values[field]);
                built.GetType().GetProperty(field)?.SetValue(built, fields_values[field]);
            }

            return built;

        }

        private Dictionary<string, object> fields_values;

    }

}