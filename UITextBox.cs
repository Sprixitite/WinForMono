using System;

using System.Windows.Forms;

namespace WinForMono {

    public class UITextBox : UIElement {

        public UITextBox() : this(false) {}

        public UITextBox(bool _multiline) {

            multiline = _multiline;

            derived_underlying = new TextBox();
            derived_underlying.AcceptsReturn = multiline;
            derived_underlying.AutoSize = false;

            multiline = true;

            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

            autosize_text = false;

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

        public bool multiline;

        public bool autosize_text;

        protected override TextFormatFlags get_text_format_flags() {

            TextFormatFlags returning = TextFormatFlags.Default | TextFormatFlags.TextBoxControl;

            returning = returning | TextFormatFlags.Left;
            returning = returning | TextFormatFlags.Top;

            return returning;

        }

        protected override void on_transform_invalidated() {
            base.on_transform_invalidated();
            if (autosize_text) fix_text_size(int.MaxValue);
        }

    }

}