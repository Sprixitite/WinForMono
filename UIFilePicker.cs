using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinForMono {

    public class UIFilePicker<T> {

        private const string ALL_FILETYPES = "All Files (*.*)|*.*";
        public const string IMAGE_FILTER = ALL_FILETYPES + "|GDI+ Supported Images (*.bmp;*.gif;*.jpeg;*.png;*.tiff)|*.bmp;*.gif;*.jpeg;*.png;*.tiff";

        public UIFilePicker() : this("Pick a file") {}

        public UIFilePicker(
            string title="Pick a file",
            string default_dir=null,
            string filter=ALL_FILETYPES,
            bool show_readonly=false
        ) {
            underlying = new OpenFileDialog();
            underlying.Title = title;
            underlying.InitialDirectory = default_dir;
            underlying.ShowReadOnly = show_readonly;
            underlying.Filter = filter;
        }

        // Will run until a valid input is given
        public string run(bool existing_file) {
            while (true) {
                if (try_run(existing_file, out string path)) return path;
            }
        }

        // Will run once, if nothing is selected returns false and path is null
        public bool try_run(bool existing_file, out string path) {
            path = null;
            underlying.CheckFileExists = existing_file;
            underlying.CheckPathExists = existing_file;
            underlying.Multiselect = false;
            switch (underlying.ShowDialog() == DialogResult.OK) {
                case true:
                    path = underlying.FileName;
                    return true;
                default:
                    return false;
            }
        }

        public string[] run_mult(bool existing_file) {
            while (true) {
                if (try_run_mult(existing_file, out string[] paths)) return paths;
            }
        }

        public bool try_run_mult(bool existing_file, out string[] paths) {
            // Default to null to make compiler happy
            paths = null;

            underlying.CheckFileExists = existing_file;
            underlying.CheckPathExists = existing_file;
            underlying.Multiselect = true;
            bool success = underlying.ShowDialog() == DialogResult.OK;
            if (success) paths = underlying.FileNames;

            return success;
        }

        private OpenFileDialog underlying;

    }

}