using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIImage : UIElement {

        // Needed so UIBuilders don't crash on this thing
        public UIImage() : this(null) {}

        public UIImage(Image i = null) {

            if (i == null) {
                System.IO.Stream image_stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("WinForMono.no_img.png");
                i = Image.FromStream(image_stream);
                image_stream.Close();
            }

            derived_underlying = new PictureBox();

            derived_underlying.Image = i;
            derived_underlying.SizeMode = PictureBoxSizeMode.StretchImage;

            image_clicked += () => {};
            underlying.Click += (object o, EventArgs e) => { image_clicked.Invoke(); };

            CALL_THIS_AFTER_CONSTRUCTION_PLEASE();
        }

        public void clear_click_events() {
            image_clicked = null;
            image_clicked += () => {};
        }

        protected override void on_hover_start(object sender, EventArgs _e) {
            base.on_hover_start(sender, _e);
            derived_underlying.BackColor = hover_background_colour;
        }

        protected override void on_hover_end(object sender, EventArgs _e) {
            base.on_hover_end(sender, _e);
            derived_underlying.BackColor = background_colour;
        }

        public override Control underlying {
            get => (Control)derived_underlying;
            set => derived_underlying = (PictureBox)value;
        }
        private PictureBox derived_underlying;

        public Image image {
            get => derived_underlying.Image;
            set => derived_underlying.Image = value;
        }

        

        public Color background_colour {
            get => _background_colour;
            set {
                _background_colour = value;
                if (_hover_background_colour == null) _hover_background_colour = value;
                if (!hovering) derived_underlying.BackColor = _background_colour;
            }
        }
        private Color _background_colour;

        public Color hover_background_colour {
            get => _hover_background_colour;
            set {
                _hover_background_colour = value;
                if (hovering) derived_underlying.BackColor = _hover_background_colour;
            }
        }
        private Color _hover_background_colour;

        public event Action image_clicked;

    }

}