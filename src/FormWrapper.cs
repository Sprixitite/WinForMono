using System;

namespace WinforMono {

    public class FormWrapper {

        public FormWrapper(System.Windows.Forms.Form wrapping) {
            Library.validate_initialised();
            underlying = wrapping;
        }

        public static FormWrapper create() {
            Library.validate_initialised();
            return new FormWrapper(new GenericForm());
        }

        Dictionary<string, System.Windows.Forms.Control> controls = new Dictionary<string, System.Windows.Forms.Control> {};

        public System.Windows.Forms.Control control_of_key(string key) {
            return controls[key];
        }

        public void add_controls(params object[] control_index_pairs) {
            if (!((control_index_pairs.Length % 2) == 0)) throw new Exception("add_controls must recieve an even number of arguments!");
            underlying.SuspendLayout();
            List<System.Windows.Forms.Control> control_list = new List<System.Windows.Forms.Control>();
            for (int i=0; i<control_index_pairs.Length; i++) {
                Type this_type = control_index_pairs[i].GetType();
                Type next_type = control_index_pairs[i+1].GetType();
                if (this_type != typeof(string) || next_type != typeof(System.Windows.Forms.Control)) throw new Exception("add_controls only accepts pairs of control keys and controls!");
                controls.Add(control_index_pairs[i], control_index_pairs[i+1]);
                control_list.Add(control_index_pairs[i+1]);
                i++;
            }
            underlying.controls.AddRange(control_list.ToArray());
            underlying.ResumeLayout();
        }

        public System.Windows.Forms.Form underlying {
            get;
            protected set;
        }

    }

}