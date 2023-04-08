using System;
using System.Drawing;

namespace WinForMono {

    public abstract partial class UIElement : WinformWrapper {

        // Internal anchor point within the respective UIElement
        public virtual UIAnchor anchor {
            get => _anchor;
            set { _anchor = value; invalidate_size(); }
        }
        private UIAnchor _anchor;

        //-------------------------------------------------------------------------------------------

        public Color border_colour {
            get => _border_colour;
            set {
                _border_colour = value;
                if (!hovering) underlying.Refresh();
            }
        }
        private Color _border_colour;

        //-------------------------------------------------------------------------------------------

        public int border_size {
            get;
            set;
        }

        //-------------------------------------------------------------------------------------------

        // Ensures the respective UIElement's width always == height
        // Done by using the smaller of the width/height for *both* to ensure scaling works
        public virtual bool copy_axis {
            get => _copy_axis;
            set { _copy_axis = value; invalidate_size(); }
        }
        private bool _copy_axis;

        //-------------------------------------------------------------------------------------------

        public virtual int corner_radius {
            get;
            set;
        }

        //-------------------------------------------------------------------------------------------

        public virtual DrawStyle draw_style {
            get => _draw_style;
            set {
                _draw_style = value;
                refresh_underlying_region();
                underlying.Refresh();
            }
        }
        private DrawStyle _draw_style;

        //-------------------------------------------------------------------------------------------

        public Color hover_border_colour {
            get => _hover_border_colour;
            set {
                _hover_border_colour = value;
                if (hovering) underlying.Refresh();
            }
        }
        private Color _hover_border_colour;

        //-------------------------------------------------------------------------------------------

        public virtual UIPosition position {
            get => _position;
            set { _position = value; invalidate_size(); }
        }
        private UIPosition _position;

        //-------------------------------------------------------------------------------------------

        public virtual UIPosition size {
            get => _size;
            set { _size = value; invalidate_size(); }
        }
        private UIPosition _size;

        //-------------------------------------------------------------------------------------------

        // Z order of controls, lower is further towards the front of the form
        public virtual int z_index {
            get => parent.underlying.Controls.GetChildIndex(this.underlying, true);
            set => parent.underlying.Controls.SetChildIndex(this.underlying, value);
        }

    }

}