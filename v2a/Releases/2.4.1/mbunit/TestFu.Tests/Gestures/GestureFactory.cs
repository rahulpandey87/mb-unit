using System;
using System.Drawing;
using MbUnit.Framework;
using TestFu.Gestures;
using System.Windows.Forms;

namespace TestFu.Tests.Gestures
{
    public class GestureTestFactory
    {
        private Form form = new Form();
        private Control button;
        private GestureFactory factory;

        public GestureTestFactory()
        {
            this.button=new Button();
            this.form.Controls.Add(this.button);
            this.factory = new GestureFactory(this.form);
        }

        [Factory]
        public ClickMouseGesture Click
        {
            get
            {
                return this.factory.MouseClick();
            }
        }

        [Factory]
        public FixedTargetMoveMouseGesture FixedTargetMove
        {
            get
            {
                return this.factory.MouseMove(new Point(0, 0));
            }
        }

        [Factory]
        public SleepGesture Sleep
        {
            get
            {
                return this.factory.Sleep(100);
            }
        }

        [Factory]
        public ButtonDownMouseGesture MouseDown
        {
            get
            {
                return this.factory.MouseDown();
            }
        }

        [Factory]
        public ButtonUpMouseGesture MouseUp
        {
            get
            {
                return this.factory.MouseUp();
            }
        }

        [Factory]
        public ControlMoveMouseGesture ControlMove
        {
            get
            {
                return this.factory.MouseMove(this.button);
            }
        }

        [Factory]
        public IGesture ControlClick
        {
            get
            {
                return this.factory.MouseClick(this.button);
            }
        }
    }
}
