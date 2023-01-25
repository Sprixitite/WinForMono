using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIPanel : UIElement {

        public UIPanel() {

            underlying = new Panel();
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

        }

        public Color colour {
            get => underlying.BackColor;
            set => underlying.BackColor = value;
        }

    }

}