using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinForMono {

    public class UITabs : UIElement {

        public class UITabPage : UIElement {

            public UITabPage(string name) {
                derived_underlying = new TabPage(name);
                CALL_THIS_AFTER_CONSTRUCTION_PLEASE();
            }

            public override Control underlying {
                get => (Control)derived_underlying;
                set => derived_underlying = (TabPage)value;
            }
            private TabPage derived_underlying;

        }

        public UITabs() {
            derived_underlying = new TabControl();
            pages = new Dictionary<string, UITabPage>();
        }

        public UITabs(params string[] tabs) {
            derived_underlying = new TabControl();
            pages = new Dictionary<string, UITabPage>();
            foreach (string s in tabs) { this.add_tab(s); }
        }

        public void add_tab(string name) {
            UITabPage new_page = new UITabPage(name);
            add_element(new_page);
            pages[name] = new_page;
        }

        public void bind_tab(string name) {
            UIElement.bind_parent(pages[name]);
        }

        public UITabPage get_tab(string name) => pages[name];

        public void remove_tab(string name, bool error=true) {
            if (pages.ContainsKey(name)) {
                remove_element(pages[name]);
                pages.Remove(name);
            } else {
                if (error) throw new Exception("Attempt to remove tab that didn't exist!");
            }
        }

        public void add_tab_element(string tab_name, UIElement element) {
            element.parent = pages[tab_name];
        }

        public void remove_tab_element(string tab_name, UIElement element) {
            element.parent = WinformWrapper.NULL;
        }

        private Dictionary<string, UITabPage> pages;

        public override Control underlying {
            get => (Control)derived_underlying;
            set => derived_underlying = (TabControl)value;
        }
        private TabControl derived_underlying;

    }

}