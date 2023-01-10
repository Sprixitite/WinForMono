using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UILabel : UIElement {

        public UILabel(string text = "") {
            derived_underlying = new Label();
            derived_underlying.Text = text;
            derived_underlying.AutoSize = true; // For whatever reason this fixes scaling issues, so we force this on
        }

        public override Control underlying {
            get => derived_underlying;
            set => derived_underlying = (Label)value;
        }

        public string text {
            get => underlying.Text;
            set => underlying.Text = value;
        }

        private Label derived_underlying;

        public UIAnchor text_anchor {
            get => UIAnchor.from_content_alignment(derived_underlying.TextAlign);
            set => derived_underlying.TextAlign = value.to_content_alignment();
        }

        public float font_size {
            get => derived_underlying.Font.Size;
            set => derived_underlying.Font = new Font(derived_underlying.Font.FontFamily, value);
        }

    }

}