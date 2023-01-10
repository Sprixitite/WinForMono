using System;

using WinForMono.Graphics;

namespace WinForMono {

    public sealed class UIPosition {

        public UIPosition() {
            scale = Vector2.ZERO;
            offset = Vector2.ZERO;
        }

        public UIPosition(Vector2 _scale, Vector2 _offset) {
            scale = _scale;
            offset = _offset;
        }

        public UIPosition(float scale_x, float scale_y, float offset_x, float offset_y) {
            scale = new Vector2(scale_x, scale_y);
            offset = new Vector2(offset_x, offset_y);
        }

        // Position on the screen in screen space
        // E.g: (0.5f, 0.5f) with anchor (0.5f, 0.5f) is the middle of the screen
        public Vector2 scale;

        // Offset from the scale in pixels
        public Vector2 offset;

    }

}