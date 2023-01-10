using System;
using System.Windows;
using System.Windows.Forms;

namespace WinForMono {

    public class UIButton : UIElement {

        public UIButton() {
            derived_underlying = new Button();
            derived_underlying.Click += _underlying_click_handler;
            button_clicked += fake_event_user;
        }

        ~UIButton() {
            derived_underlying.Click -= _underlying_click_handler;
            button_clicked -= fake_event_user;
        }

        private void _underlying_click_handler(object _s, EventArgs _e) {
            button_clicked.Invoke();
        }

        public override Control underlying {
            get => (Control)derived_underlying;
            set => derived_underlying = (Button)value;
        }
        private Button derived_underlying;

        public string text {
            get => derived_underlying.Text;
            set => derived_underlying.Text = value;
        }

        public delegate void OnUIButtonClicked();
        public event OnUIButtonClicked button_clicked;

    }

}