using System;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    public abstract class KeyGestureBase : GestureBase
    {
        private char key;

        public KeyGestureBase()
        { }

        public KeyGestureBase(Form form, char key)
            : base(form)
        {
            this.key = key;
        }

        public char Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }
    }
}
