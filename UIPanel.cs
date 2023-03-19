using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIPanel : UIElement {

        public UIPanel() {

            underlying = new Panel();
            size = new UIPosition(1.0f, 1.0f, 0.0f, 0.0f);
            colour = Color.Transparent;
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

        }

        public Color colour {
            get => underlying.BackColor;
            set => underlying.BackColor = value;
        }

    }

}