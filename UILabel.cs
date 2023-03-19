using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace WinForMono {

    public class UILabel : UIElement {

        public UILabel(string text = "") {
            derived_underlying = new Label();
            derived_underlying.Text = text;
            derived_underlying.AutoSize = false;
            derived_underlying.BackColor = Color.Transparent;
            autosize_text = false;
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();
        }

        public override Control underlying {
            get => derived_underlying;
            set => derived_underlying = (Label)value;
        }

        public string text {
            get => underlying.Text;
            set => underlying.Text = value;
        }

        public bool single_line = true;

        private Label derived_underlying;

        public UIAnchor text_anchor {
            get => UIAnchor.from_content_alignment(derived_underlying.TextAlign);
            set => derived_underlying.TextAlign = value.to_content_alignment();
        }

        public float font_size {
            get => derived_underlying.Font.Size;
            set => derived_underlying.Font = new Font(derived_underlying.Font.FontFamily, value);
        }

        public bool autosize_text;
        public int font_size_upper_limit = int.MaxValue;

        protected override TextFormatFlags get_text_format_flags() {

            TextFormatFlags returning = TextFormatFlags.Default;

            switch (text_anchor.x) {
                case AnchorX.LEFT: returning = returning | TextFormatFlags.Left; break;
                case AnchorX.CENTER: returning = returning | TextFormatFlags.HorizontalCenter; break;
                case AnchorX.RIGHT: returning = returning | TextFormatFlags.Right; break;
            }

            switch (text_anchor.y) {
                case AnchorY.TOP: returning = returning | TextFormatFlags.Top; break;
                case AnchorY.CENTER: returning = returning | TextFormatFlags.VerticalCenter; break;
                case AnchorY.BOTTOM: returning = returning | TextFormatFlags.Bottom; break;
            }

            //if (single_line) returning = returning | TextFormatFlags.SingleLine;
            returning = returning | TextFormatFlags.NoClipping;

            return returning;

        }

        protected override void on_transform_invalidated() {
            base.on_transform_invalidated();
            if (autosize_text) fix_text_size(font_size_upper_limit);
        }

    }

}