using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using WinForMono.Graphics;

namespace WinForMono {

    public enum RichStyling {
        WAVY,
        BOLD,
        ITALICS,
        UNDERLINE,
    }

    public class UIRichLabel : UIElement {

        private class LabelRecipe {
            public LabelRecipe() {
                style_recipe = new List<object>();
            }

            public void add_styling(object step) {
                style_recipe.Add(step);
            }

            public UILabel[] bake() {

                bool is_bold = false;
                bool is_italics = false;
                bool is_underline = false;
                bool is_wavy = false;

                Color color = Color.White;

                foreach (object step in style_recipe) {
                    if (step is RichStyling) {
                        switch ((RichStyling)step) {
                            case RichStyling.WAVY: is_wavy = true; break;
                            case RichStyling.BOLD: is_bold = true; break;
                            case RichStyling.ITALICS: is_italics = true; break;
                            case RichStyling.UNDERLINE: is_underline = true; break;
                        }
                    } else if (step is ColourHSVA || step is ColourRGBA) {
                        color = ((Color)step);
                    } else {
                        throw new Exception("Unrecognized rich text style!");
                    }
                }

                UILabel[] labels;

                switch (is_wavy) {
                    case true: labels = new UILabel[text.Length];


                    case false: labels = new UILabel[1];
                        labels[0] = new UILabel(text);
                        
                }

                private Font get_font(float size, bool bold, bool italics, bool underline) {

                }

            }

            List<object> style_recipe;
            string text;
        }

        // Text is composed of a list of objects as so
        // "THIS IS A VERY "
        // WAVY_TEXT
        // "FANCY"
        // NORMAL_TEXT
        // " MESSAGE"
        public UIRichLabel(object[] text) {
            labels = new List<UILabel>();
        }

        List<UILabel> labels;

    }

}