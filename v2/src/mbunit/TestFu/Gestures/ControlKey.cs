using System;
using System.IO;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    public class ControlKey
    {
        private string key =null;
        private char pathSeparator = '.';


        public ControlKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            this.key =key;
        }

        public ControlKey(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            this.key = this.BuildKey(control);
        }

        private string BuildKey(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            if (control.Parent != null)
            {
                string upperKey = BuildKey(control.Parent);
                return string.Format("{0}{1}{2}",
                    upperKey,
                    this.PathSeparator,
                    control.Name);
            }
            else
            {
                return control.Name;
            }
        }

        public Control FindControl(Form form)
        {
            string[] keys = this.Key.Split(this.PathSeparator);
            return findControl(form, keys, 0);
        }

        private Control findControl(Control control, string[] keys, int index)
        {
            if (control.Name != keys[index])
                throw new InvalidOperationException();
            int i = ++index;
            if (i == keys.Length)
                return control;

            string name = keys[i];
            foreach (Control child in control.Controls)
            {
                if (child.Name == name)
                    return findControl(child, keys, i);
            }
            return null;
        }

        public char PathSeparator
        {
            get
            {
                return this.pathSeparator;
            }
            set
            {
                this.pathSeparator = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("key");
                this.key = value;
            }
        }

        public override string ToString()
        {
            return this.Key;
        }
    }
}
