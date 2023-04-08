using System;
using System.Timers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;

using WinForMono.Graphics;

namespace WinForMono {

    public abstract partial class UIElement : WinformWrapper {

        public UIElement(bool wants_parent_ctor = true) {

            elements = new List<UIElement>();

            if (!wants_parent_ctor) return;

            // Default to top left
            anchor = UIAnchor.TOP_LEFT;
            position = new UIPosition(0, 0, 0, 0);

            // Default to x/2 and y/2
            _size = new UIPosition(0.5f, 0.5f, -4, -4);

            copy_axis = false;
            corner_radius = 16;
            border_size = 0;
            
            //underlying.GetType().GetField("")

        }

        //-------------------------------------------------------------------------------------------

        // Needed because underlying is always null in the constructor
        protected void CALL_THIS_AFTER_CONSTRUCTION_PLEASE() {
            if (!default_parent.Equals(WinformWrapper.NULL)) {
                this.parent = default_parent;
            }
            underlying.Paint += paint_border;
            underlying.MouseEnter += on_hover_start;
            underlying.MouseLeave += on_hover_end;
            typeof(Control).GetProperty("ResizeRedraw", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default).SetValue(underlying, false);
        }

        //-------------------------------------------------------------------------------------------

        // Returns a square region with rounded corners
        // Used for "DrawStyle.ROUNDED_SQUARE"'s implementation
        private GraphicsPath get_rounded_region(RectangleF Rect, int radius) {
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();
            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, 
                            Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);
            GraphPath.CloseFigure();
            return GraphPath;
        }

        //-------------------------------------------------------------------------------------------

        // Implements "DrawStyle"s
        private void refresh_underlying_region() {
            switch (_draw_style) {
                case DrawStyle.SQUARE:
                    underlying.Region = new Region(new Rectangle(0, 0, underlying.ClientSize.Width, underlying.ClientSize.Height));
                    break;
                case DrawStyle.ELIPSE:
                    Rectangle rc = underlying.ClientRectangle;
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(rc);
                    underlying.Region = new Region(gp);
                    break;
                case DrawStyle.ROUNDED_SQUARE:
                    underlying.Region = new Region(get_rounded_region(underlying.ClientRectangle, corner_radius));
                    break;
            }
            
        }

        //-------------------------------------------------------------------------------------------

        private void paint_border(object sender, PaintEventArgs pea) {
            Pen border_pen = new Pen( hovering ? hover_border_colour : border_colour );
            border_pen.Width = border_size;
            border_pen.Alignment = PenAlignment.Outset;
            switch (_draw_style) {
                case DrawStyle.SQUARE:
                    pea.Graphics.DrawRectangle(border_pen, underlying.ClientRectangle);
                    break;
                case  DrawStyle.ELIPSE:
                    pea.Graphics.DrawEllipse(border_pen, underlying.ClientRectangle);
                    break;
                case DrawStyle.ROUNDED_SQUARE:
                    pea.Graphics.DrawPath(border_pen, get_rounded_region(underlying.ClientRectangle, corner_radius));
                    break;
            }
            border_pen.Dispose();
        }

        //-------------------------------------------------------------------------------------------

        protected bool hovering = false;
        protected virtual void on_hover_start(object o, EventArgs e) {
            hovering = true;
        }

        protected virtual void on_hover_end(object o, EventArgs e) {
            hovering = false;
        }

        //-------------------------------------------------------------------------------------------

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

            switch (_copy_axis) {
                case false:
                    break;
                case true:
                    width = Math.Min(width, height);
                    height = Math.Min(width, height);
                    break;
            }

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

            // Correct for vertical anchoring
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

            refresh_underlying_region();
            
        }

        //-------------------------------------------------------------------------------------------

        // Implemented on UIElements which make use of autosizing text
        protected virtual TextFormatFlags get_text_format_flags() { throw new NotImplementedException(); }

        // The size by which we increase/decrease the font size when brute-forcing the ideal size
        // Lower values provide more accurate results while higher values are faster
        const float text_size_increment = 1f;

        // Doesn't so much "fix" the text size as it finds the ideal one for the control's size
        // Done by brute forcing the new size via trial and error
        protected void fix_text_size(int upper_limit) {

            //System.Console.WriteLine("S"); 

            // If underlying.Text.Trim() contains nothing, add a single character to avoid crashing
            // If underlying.Text.Trim() has text, replace all spaces in underlying.Text with an underscore for correct sizing
            // Without replacing the spaces, trailing/leading spaces won't affect the sizing
            string text = underlying.Text.Trim() == "" ? "A" : underlying.Text.Replace(" ", "_");

            float size = underlying.Font.Size;
            Font last_font = new Font(underlying.Font.FontFamily, size);

            bool done = false;

            bool incrementing = true;

            Size underlying_size = new Size(underlying.Width, underlying.Height);

            Size current = TextRenderer.MeasureText(text, last_font, underlying.Size, get_text_format_flags());
            if (current.Width > underlying.Width || current.Height > underlying.Height) incrementing = false;

            if (size < 0) size = 1;

            switch (incrementing) {

                case true:
                    while (!done) {
                        if ((size + text_size_increment) > upper_limit) { size = upper_limit; done = true; }
                        size += text_size_increment;
                        Font bigger_font = new Font(last_font.FontFamily, size);
                        Size bigger_size = TextRenderer.MeasureText(text, bigger_font, underlying.Size, get_text_format_flags());
                        if (bigger_size.Width > underlying.ClientSize.Width || bigger_size.Height > underlying.ClientSize.Height) done = true;
                        else last_font = bigger_font;
                    } break;

                case false:
                    while (!done) {
                        if ((size - text_size_increment) < 1) { size = 1+text_size_increment; done = true; }
                        size -= text_size_increment;
                        Font smaller_font = new Font(last_font.FontFamily, size);
                        Size smaller_size = TextRenderer.MeasureText(text, smaller_font, underlying_size, get_text_format_flags());
                        if (smaller_size.Width < underlying.ClientSize.Width && smaller_size.Height < underlying.ClientSize.Height) done = true;
                        last_font = smaller_font;
                    } break;
                
            }

            last_font = new Font(last_font.FontFamily, Math.Max(1, size-text_size_increment));

            underlying.Font = last_font;

        }

    }

}