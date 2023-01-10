using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public enum AnchorX {
        LEFT = 0,
        RIGHT = 1,
        CENTER = 2
    }

    public enum AnchorY {
        TOP = 0,
        BOTTOM = 1,
        CENTER = 2
    }

    public struct UIAnchor {

        public static readonly UIAnchor TOP_LEFT = new UIAnchor(AnchorX.LEFT, AnchorY.TOP);
        public static readonly UIAnchor TOP_CENTER = new UIAnchor(AnchorX.CENTER, AnchorY.TOP);
        public static readonly UIAnchor TOP_RIGHT = new UIAnchor(AnchorX.RIGHT, AnchorY.TOP);
        public static readonly UIAnchor MIDDLE_LEFT = new UIAnchor(AnchorX.LEFT, AnchorY.CENTER);
        public static readonly UIAnchor MIDDLE_CENTER = new UIAnchor(AnchorX.CENTER, AnchorY.CENTER);
        public static readonly UIAnchor MIDDLE_RIGHT = new UIAnchor(AnchorX.RIGHT, AnchorY.CENTER);
        public static readonly UIAnchor BOTTOM_LEFT = new UIAnchor(AnchorX.LEFT, AnchorY.BOTTOM);
        public static readonly UIAnchor BOTTOM_CENTER = new UIAnchor(AnchorX.CENTER, AnchorY.BOTTOM);
        public static readonly UIAnchor BOTTOM_RIGHT = new UIAnchor(AnchorX.RIGHT, AnchorY.BOTTOM);

        private UIAnchor(AnchorX _x, AnchorY _y) {
            x = _x;
            y = _y;
        }

        public AnchorX x {
            get;
            private set;
        }

        public AnchorY y {
            get;
            private set;
        }

        public ContentAlignment to_content_alignment() {
            switch (y) {
                case AnchorY.TOP: switch (x) {
                    case AnchorX.LEFT: return ContentAlignment.TopLeft;
                    case AnchorX.CENTER: return ContentAlignment.TopCenter;
                    case AnchorX.RIGHT: return ContentAlignment.TopRight;
                    default: throw new Exception("New AnchorX!!!");
                }

                case AnchorY.CENTER: switch (x) {
                    case AnchorX.LEFT: return ContentAlignment.MiddleLeft;
                    case AnchorX.CENTER: return ContentAlignment.MiddleCenter;
                    case AnchorX.RIGHT: return ContentAlignment.MiddleRight;
                    default: throw new Exception("New AnchorX!!!");
                }

                case AnchorY.BOTTOM: switch (x) {
                    case AnchorX.LEFT: return ContentAlignment.BottomLeft;
                    case AnchorX.CENTER: return ContentAlignment.BottomCenter;
                    case AnchorX.RIGHT: return ContentAlignment.BottomRight;
                    default: throw new Exception("New AnchorX!!!");
                }

                default: throw new Exception("New AnchorY!!!");
            }
        }

        public static UIAnchor from_content_alignment(ContentAlignment alignment) {
            switch (alignment) {
                case ContentAlignment.TopLeft: return TOP_LEFT;
                case ContentAlignment.TopCenter: return TOP_CENTER;
                case ContentAlignment.TopRight: return TOP_RIGHT;
                case ContentAlignment.MiddleLeft: return MIDDLE_LEFT;
                case ContentAlignment.MiddleCenter: return MIDDLE_CENTER;
                case ContentAlignment.MiddleRight: return MIDDLE_RIGHT;
                case ContentAlignment.BottomLeft: return BOTTOM_LEFT;
                case ContentAlignment.BottomCenter: return BOTTOM_CENTER;
                case ContentAlignment.BottomRight: return BOTTOM_RIGHT;
                default: throw new Exception("New ContentAlignment!!!");
            }
        }

    }

}