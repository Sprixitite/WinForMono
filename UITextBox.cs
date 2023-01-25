using System;

using System.Windows.Forms;

namespace WinForMono {

    public class UITextBox : UIElement {

        public UITextBox() : this(false) {}

        public UITextBox(bool _multiline) {

            derived_underlying = new TextBox();

            multiline = _multiline;

            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

        }

        public override Control underlying { 
            get => derived_underlying;
            set => derived_underlying = (TextBox)value;
        }
        private TextBox derived_underlying;

        public string text {
            get => derived_underlying.Text;
            set => derived_underlying.Text = value;
        }

        public bool multiline {
            get => derived_underlying.Multiline;
            set => derived_underlying.Multiline = value;
        }

    }

}