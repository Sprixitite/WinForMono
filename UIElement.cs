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

            // Default to four pixels from the bottom left on both axes
            position = new UIPosition(0, 0, 4, 4);

            // Default to (x-4)/2 and (y-4)/2
            // We subtract 4 to make up for the previous offset
            _size = new UIPosition(0.5f, 0.5f, -4, -4);

        }

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
            
            Vector2 parent_dimensions = Vector2.ZERO;

            if (parent.GetType() == typeof(UIWindow)) { parent_dimensions = new Vector2(parent.underlying.ClientSize.Width, parent.underlying.ClientSize.Height); }
            else { parent_dimensions = new Vector2(parent.underlying.Width, parent.underlying.Height); }

            underlying.Left = (int)Math.Floor(parent_dimensions.x * position.scale.x) + (int)position.offset.x;
            underlying.Top = (int)Math.Floor(parent_dimensions.y * position.scale.y) + (int)position.offset.y;

            underlying.Width = (int)Math.Floor(parent_dimensions.x * size.scale.x) + (int)size.offset.x;
            underlying.Height = (int)Math.Floor(parent_dimensions.y * size.scale.y) + (int)size.offset.y;

            switch (anchor.x) {
                case AnchorX.LEFT:
                    break;
                case AnchorX.CENTER:
                    underlying.Left -= (int)Math.Floor(underlying.Width / 2.0f);
                    break;
                case AnchorX.RIGHT:
                    underlying.Left -= underlying.Width;
                    break;
            }

            switch (anchor.y) {
                case AnchorY.TOP:
                    break;
                case AnchorY.CENTER:
                    underlying.Top -= (int)Math.Floor(underlying.Height / 2.0f);
                    break;
                case AnchorY.BOTTOM:
                    underlying.Top -= underlying.Height;
                    break;
            }

            invalidate_size();

        }

    }

}