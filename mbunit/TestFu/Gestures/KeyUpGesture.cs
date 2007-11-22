using System;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    public class KeyUpGesture : KeyGestureBase
    {
        public KeyUpGesture()
        { }

        public KeyUpGesture(Form form, char key)
            : base(form,key)
        { }

        public override void Start()
        {
            VirtualInput.KeyUp(this.Form.Handle, Key);
        }

        public override Refly.CodeDom.Expressions.Expression ToCodeDom(Refly.CodeDom.Expressions.Expression factory)
        {
            throw new NotImplementedException();
        }
    }
}
