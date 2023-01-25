using System;
using System.Windows.Forms;

namespace WinForMono {

    public enum UIResult {
        RETRY,
        CANCEL,
        OK,
        YES,
        NO,
        CONTINUE,
        NONE
    }

    public enum UIButtonGroup {
        OK = 0,
        OK_CANCEL = 1,
        ABORT_RETRY_IGNORE = 2,
        YES_NO_CANCEL = 3,
        YES_NO = 4,
        RETRY_CANCEL = 5,
        CANCEL_TRY_CONTINUE = 6
    }



    public enum UIIcon {
        NONE = 0,
        ERROR = 16,
        QUESTION = 32,
        WARNING = 48,
        INFORMATION = 64
    }

    public interface IMessageBoxHandler {

        UIResult spawn(
            string message="DEFAULT_MESSAGE",
            string title="DEFAULT_TITLE",
            UIButtonGroup buttons=UIButtonGroup.OK,
            UIIcon icon=UIIcon.NONE,
            int default_button=0,
            MessageBoxOptions options=0,
            string help_path=null,
            string help_keyword=null
        );

    }

}