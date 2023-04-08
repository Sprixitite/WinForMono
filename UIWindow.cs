using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinForMono {

    public class UIWindow : WinformWrapper {

        public delegate void OnFormElementClicked(object control);
        public event OnFormElementClicked form_element_clicked;

        public delegate void OnKeyDown(KeyEventArgs key);
        public event OnKeyDown form_key_down;

        private class GenericForm : Form {}

        public UIWindow() {
            derived_underlying = new GenericForm();
            derived_underlying.Resize += invalidate_size;
            _performant_resizing = false;
            form_element_clicked += (object o) => { }; // Fake user so the event never gets deleted
            form_key_down += (KeyEventArgs e) => {  };
            derived_underlying.KeyDown += (object o, KeyEventArgs e) => { e.SuppressKeyPress = true; form_key_down.Invoke(e); e.Handled = true; };
            derived_underlying.KeyPreview = true;
        }

        public bool performant_resizing {
            get => _performant_resizing;
            set {
                _performant_resizing = value;
                switch (_performant_resizing) {
                    case true:
                        derived_underlying.Resize -= invalidate_size;
                        derived_underlying.ResizeEnd += invalidate_size;
                        break;
                    case false:
                        derived_underlying.ResizeEnd -= invalidate_size;
                        derived_underlying.Resize += invalidate_size;
                        break;
                }
            }
        }
        private bool _performant_resizing;

        public bool fullscreen {
            get => _fullscreen;
            set {
                if (value == true) {
                    derived_underlying.WindowState = FormWindowState.Normal;
                    derived_underlying.FormBorderStyle = FormBorderStyle.None;
                    derived_underlying.WindowState = FormWindowState.Maximized;
                    //derived_underlying.mainPanel.Dock = DockStyle.Fill;
                    //derived_underlying.BringToFront();
                } else {
                    derived_underlying.WindowState = FormWindowState.Maximized;
                    derived_underlying.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                }
                _fullscreen = value;
            }
        }
        private bool _fullscreen;

        public Icon icon {
            get => derived_underlying.Icon;
            set => derived_underlying.Icon = value;
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

        public void element_clicked(object o, EventArgs e) {
            form_element_clicked.Invoke(o);
        }

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