using System;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    public class PressKeyGesture : KeyGestureBase
    {
        public PressKeyGesture()
        { }

        public PressKeyGesture(Form form, char key)
            : base(form,key)
        { }

        public override void Start()
        {
            VirtualInput.PressKey(this.Form.Handle, Key);
        }

        public override Refly.CodeDom.Expressions.Expression ToCodeDom(Refly.CodeDom.Expressions.Expression factory)
        {
            throw new NotImplementedException();
        }
    }
}
