using System;

namespace WinForMono {

    public abstract partial class UIElement : WinformWrapper {

        static UIElement() {
            default_parent = WinformWrapper.NULL;
        }

        //-------------------------------------------------------------------------------------------

        public static void bind_parent(WinformWrapper binding) {
            switch (default_parent.Equals(WinformWrapper.NULL) || binding == null) {
                case true:
                    switch (binding == null) {
                        case true:
                            default_parent = WinformWrapper.NULL;
                            break;
                        case false:
                            default_parent = binding;
                            break;
                    }
                    break;
                default:
                    throw new Exception("Attempt to overwrite non-null default parent.");
            }
        }

        private static WinformWrapper default_parent;

    }

}