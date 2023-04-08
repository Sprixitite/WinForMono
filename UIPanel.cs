using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIPanel : UIElement {

        public UIPanel() {

            underlying = new Panel();
            ((Panel)underlying).AutoSize = false;
            ((Panel)underlying).Scroll += (object sender, ScrollEventArgs e) => {}; // Dummy so the event doesn't delete itself lol
            size = new UIPosition(1.0f, 1.0f, 0.0f, 0.0f);
            colour = Color.Transparent;
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

        }

        public Color colour {
            get => underlying.BackColor;
            set => underlying.BackColor = value;
        }

        private void on_underlying_scrolled(object sender, ScrollEventArgs e) { on_transform_invalidated(); ((Panel)underlying).AutoScrollMinSize = new Size(underlying.Width, underlying.Height); }

        public bool scrolling {
            get { ((Panel)underlying).Scroll += on_underlying_scrolled; return ((Panel)underlying).AutoScroll; }
            set { ((Panel)underlying).Scroll -= on_underlying_scrolled; ((Panel)underlying).AutoScroll = value; }
        }

    }

}