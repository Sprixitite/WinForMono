using System;
using WinforMono;

namespace WinforMono {

    public static class Library {

        static WinforMono() {
            Inited = false;
        }

        public static FormWrapper Init() {
            if (inited) throw new Exception("Cannot init twice!");
            Inited = true;
            FormWrapper master_form = FormWrapper.create();
            System.Windows.Forms.Application.Run(master_form.underlying);
            return master_form;
        }

        public static FormWrapper Init(System.Windows.Forms.Form master_to_wrap) {
            if (inited) throw new Exception("Cannot init twice!");
            inited = true;
            FormWrapper master_form = new FormWrapper(master_to_wrap);
            System.Windows.Forms.Application.Run(master_form.underlying);
            return master_form;
        }

        internal static void validate_initialised() {
            switch (inited) {
                case true:
                    return;
                case false:
                    throw new TypeInitializationException("Call WinforMono.Init before using any of the other types.");
            }
        }

        private static bool inited {
            get;
            set;
        }

    }

}