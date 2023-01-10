using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinForMono {

    public class UITabs : UIElement {

        private class UITabPage : UIElement {

            public UITabPage(string name) {
                derived_underlying = new TabPage(name);
            }

            public override Control underlying {
                get => derived_underlying;
                set => derived_underlying = (TabPage)value;
            }
            private TabPage derived_underlying;

        }

        public UITabs() {
            derived_underlying = new TabControl();
            pages = new Dictionary<string, UITabPage>();
        }

        public UITabs(string[] tabs) : this() {
            foreach (string s in tabs) { this.add_tab(s); }
        }

        public void add_tab(string name) {
            UITabPage new_page = new UITabPage(name);
            add_element(new_page);
            pages[name] = new_page;
        }

        public void remove_tab(string name) {
            if (pages.ContainsKey(name)) {
                remove_element(pages[name]);
                pages.Remove(name);
            } else {
                throw new Exception("Attempt to remove tab that didn't exist!");
            }
        }

        public void add_tab_element(string tab_name, UIElement element) {
            pages[tab_name].add_element(element);
        }

        public void remove_tab_element(string tab_name, UIElement element) {
            pages[tab_name].remove_element(element);
        }

        private Dictionary<string, UITabPage> pages;

        public override Control underlying {
            get => derived_underlying;
            set => derived_underlying = (TabControl)value;
        }
        private TabControl derived_underlying;

    }

}