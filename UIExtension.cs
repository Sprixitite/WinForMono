using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIExtension : UIElement {

        public UIExtension() : base(false) {
            panel = new UIPanel();
        }

        public override UIPosition position { get => panel.position; set => panel.position = value; }
        public override UIPosition size { get => panel.size; set => panel.size = value; }
        public override UIAnchor anchor { get => panel.anchor; set => panel.anchor = value; }
        public override bool copy_axis { get => panel.copy_axis; set => panel.copy_axis = value; }
        public override Control underlying { get => panel.underlying; set => panel.underlying = value; }
        public override DrawStyle draw_style { get => panel.draw_style; set => panel.draw_style = value; }
        public override int corner_radius { get => panel.corner_radius; set => panel.corner_radius = value; }
        public Color background_colour { get => panel.colour; set => panel.colour = value; }
        public override WinformWrapper parent { get => panel.parent; set => panel.parent = value; }

        protected UIPanel panel;

    }

}