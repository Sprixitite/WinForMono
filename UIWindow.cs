using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForMono {

    public class UIWindow : WinformWrapper {

        private class GenericForm : Form {}

        public UIWindow() {
            derived_underlying = new GenericForm();
            derived_underlying.Resize += invalidate_size;
        }

        public void run() {
            // Do this to get the first frame ready
            derived_underlying.Show();
            invalidate_size();
            derived_underlying.Hide();

            // Actually run the thing lol
            Application.Run(derived_underlying);
        }

        public string title {
            get => underlying.Text;
            set => underlying.Text = value;
        }

        public override Control underlying {
            get => derived_underlying;
            set => derived_underlying = (Form)value;
        }
        private Form derived_underlying;

        public UIResult spawn(
            string message="DEFAULT_MESSAGE",
            string title="DEFAULT_TITLE",
            UIButtonGroup buttons=UIButtonGroup.OK,
            UIIcon icon=UIIcon.NONE,
            int default_button=0,
            MessageBoxOptions options=0,
            string help_path=null,
            string help_keyword=null
        ) {

            if (help_path == null) {

                return (UIResult)MessageBox.Show(
                    derived_underlying,
                    message,
                    title,
                    (MessageBoxButtons)buttons,
                    (MessageBoxIcon)icon,
                    (MessageBoxDefaultButton)default_button,
                    options
                );

            } else {

                return (UIResult)MessageBox.Show(
                    derived_underlying,
                    message,
                    title,
                    (MessageBoxButtons)buttons,
                    (MessageBoxIcon)icon,
                    (MessageBoxDefaultButton)default_button,
                    options,
                    help_path,
                    help_keyword
                );

            }

        }

    }

}