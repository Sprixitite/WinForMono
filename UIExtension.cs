using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIExtension : UIElement {

        public UIExtension() : base(false) {
            extension_base = new UIPanel();
        }

        public override UIPosition position { get => extension_base.position; set => extension_base.position = value; }
        public override UIPosition size { get => extension_base.size; set => extension_base.size = value; }
        public override UIAnchor anchor { get => extension_base.anchor; set => extension_base.anchor = value; }
        public override bool copy_axis { get => extension_base.copy_axis; set => extension_base.copy_axis = value; }
        public override Control underlying { get => extension_base.underlying; set => extension_base.underlying = value; }
        public override DrawStyle draw_style { get => extension_base.draw_style; set => extension_base.draw_style = value; }
        public override int corner_radius { get => extension_base.corner_radius; set => extension_base.corner_radius = value; }
        // public Color background_colour { get => extension_base.colour; set => extension_base.colour = value; }
        public override WinformWrapper parent { get => extension_base.parent; set => extension_base.parent = value; }

        public UIElement extension_base {
            get;
            protected set;
        }

    }

}