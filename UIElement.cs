using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WinForMono.Graphics;

namespace WinForMono {

    public abstract class UIElement : WinformWrapper {

        public UIElement() {

            elements = new List<UIElement>();

            // Default to top left
            anchor = UIAnchor.TOP_LEFT;
            position = new UIPosition(0, 0, 0, 0);

            // Default to x/2 and y/2
            _size = new UIPosition(0.5f, 0.5f, -4, -4);

        }

        protected void CALL_THIS_AFTER_CONSTRUCTION_PLEASE() {
            if (!default_parent.Equals(WinformWrapper.NULL)) {
                default_parent.add_element(this);
            }
        }

        static UIElement() {
            default_parent = WinformWrapper.NULL;
        }

        public static void bind_parent(WinformWrapper binding) {
            switch (binding == null) {
                case true:
                    default_parent = WinformWrapper.NULL;
                    break;
                case false:
                    default_parent = binding;
                    break;
            }
        }

        private static WinformWrapper default_parent;

        public UIAnchor anchor {
            get => _anchor;
            set { _anchor = value; invalidate_size(); }
        }
        private UIAnchor _anchor;

        public UIPosition position {
            get => _position;
            set { _position = value; invalidate_size(); }
        }
        private UIPosition _position;

        public UIPosition size {
            get => _size;
            set { _size = value; invalidate_size(); }
        }
        private UIPosition _size;

        protected override void on_transform_invalidated() {

            // Default this to (0, 0) because we figure this out in an "if" branch
            Vector2 parent_dimensions = Vector2.ZERO;

            // We need to make a special-case for windows because the borders are included in their regular Width/Height
            // Instead use ClientSize.Width/Height to get borderless size
            if (parent.GetType() == typeof(UIWindow)) { parent_dimensions = new Vector2(parent.underlying.ClientSize.Width, parent.underlying.ClientSize.Height); }
            else { parent_dimensions = new Vector2(parent.underlying.Width, parent.underlying.Height); }

            // Position along axis = ( parent size on axis * this.scale along this axis ) + this.offset along this axis
            int left = (int)Math.Floor(parent_dimensions.x * position.scale.x) + (int)position.offset.x;
            int top = (int)Math.Floor(parent_dimensions.y * position.scale.y) + (int)position.offset.y;

            // Same as above just using width
            int width = (int)Math.Floor(parent_dimensions.x * size.scale.x) + (int)size.offset.x;
            int height = (int)Math.Floor(parent_dimensions.y * size.scale.y) +  (int)size.offset.y;

            // TODO: eventually implement anchoring with Vector2s
            // ! Should be something to do with subtracting the size * the anchor position along the relevant axes
            switch (anchor.x) {
                case AnchorX.LEFT:
                    break;
                case AnchorX.CENTER:
                    left -= (int)Math.Floor(width / 2.0f);
                    break;
                case AnchorX.RIGHT:
                    left -= width;
                    break;
            }

            switch (anchor.y) {
                case AnchorY.TOP:
                    break;
                case AnchorY.CENTER:
                    top -= (int)Math.Floor(height / 2.0f);
                    break;
                case AnchorY.BOTTOM:
                    top -= height;
                    break;
            }

            // Only set these once because otherwise we get flickering even though the layout is suspended
            // Above occurring on my Pop!_os 22.04 using xwayland
            // Also occurred on my fedora 36 install using xwayland
            underlying.Left = left;
            underlying.Top = top;
            underlying.Width = width;
            underlying.Height = height;

        }

    }

}