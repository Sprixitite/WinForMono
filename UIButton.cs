using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WinForMono {

    public class UIButton : UIElement {

        public UIButton() : this("Default Text") { }

        public UIButton(string _text) {
            derived_underlying = new Button();
            text = _text;
            
            button_clicked += fake_event_user;
            text_anchor = UIAnchor.MIDDLE_CENTER;
            autosize_text = false;
            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();

            // Set to sane defaults
            colour = derived_underlying.BackColor;
            text_colour = derived_underlying.ForeColor;
            hover_colour = colour;
            text_hover_colour = text_colour;

            typeof(Button)
                .GetMethod("SetStyle",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(derived_underlying, new object[] { ControlStyles.Selectable, false });

            // Bind events
            derived_underlying.Click += _underlying_click_handler;
        }

        protected override void on_hover_start(object sender, EventArgs _e) {
            base.on_hover_start(sender, _e);
            derived_underlying.BackColor = hover_colour;
            derived_underlying.ForeColor = text_hover_colour;
        }

        protected override void on_hover_end(object sender, EventArgs _e) {
            base.on_hover_end(sender, _e);
            derived_underlying.BackColor = colour;
            derived_underlying.ForeColor = text_colour;
        }

        ~UIButton() {
            derived_underlying.Click -= _underlying_click_handler;
            derived_underlying.MouseEnter -= on_hover_start;
            derived_underlying.MouseLeave -= on_hover_end;
            button_clicked -= fake_event_user;
        }

        private void _underlying_click_handler(object _s, EventArgs _e) {
            button_clicked.Invoke();
        }

        public override Control underlying {
            get => (Control)derived_underlying;
            set => derived_underlying = (Button)value;
        }
        private Button derived_underlying;

        public string text {
            get => derived_underlying.Text;
            set => derived_underlying.Text = value;
        }

        public delegate void OnUIButtonClicked();
        public event OnUIButtonClicked button_clicked;

        public UIAnchor text_anchor {
            get => UIAnchor.from_content_alignment(derived_underlying.TextAlign);
            set => derived_underlying.TextAlign = value.to_content_alignment();
        }

        public bool autosize_text;

        protected override TextFormatFlags get_text_format_flags() {

            TextFormatFlags returning = TextFormatFlags.Default;

            switch (text_anchor.x) {
                case AnchorX.LEFT: returning = returning | TextFormatFlags.Left; break;
                case AnchorX.CENTER: returning = returning | TextFormatFlags.HorizontalCenter; break;
                case AnchorX.RIGHT: returning = returning | TextFormatFlags.Right; break;
            }

            switch (text_anchor.y) {
                case AnchorY.TOP: returning = returning | TextFormatFlags.Top; break;
                case AnchorY.CENTER: returning = returning | TextFormatFlags.VerticalCenter; break;
                case AnchorY.BOTTOM: returning = returning | TextFormatFlags.Bottom; break;
            }

            return returning;

        }

        protected override void on_transform_invalidated() {
            base.on_transform_invalidated();
            if (autosize_text) fix_text_size(int.MaxValue);
        }

        public Color colour {
            get => _colour;
            set {
                _colour = value;
                if (!hovering) derived_underlying.BackColor = _colour;
            }
        }
        private Color _colour;

        public Color hover_colour {
            get => _hover_colour;
            set {
                _hover_colour = value;
                if (hovering) derived_underlying.BackColor = _hover_colour;
            }
        }
        private Color _hover_colour;

        public Color text_colour {
            get => _text_colour;
            set {
                _text_colour = value;
                if (!hovering) derived_underlying.ForeColor = _text_colour;
            }
        }
        private Color _text_colour;

        public Color text_hover_colour {
            get => _text_hover_colour;
            set {
                _text_hover_colour = value;
                if (hovering) derived_underlying.ForeColor = _text_hover_colour;
            }
        }
        private Color _text_hover_colour;

        public bool flat {
            get => _flat;
            set {
                switch (value) {
                    case true:
                        derived_underlying.FlatStyle = FlatStyle.Flat;
                        derived_underlying.FlatAppearance.BorderSize = 0;
                        break;
                    case false:
                        derived_underlying.FlatStyle = FlatStyle.Standard;
                        derived_underlying.FlatAppearance.BorderSize = 1;
                        break;
                }
                _flat = value;
            }
        }
        private bool _flat;

    }

}