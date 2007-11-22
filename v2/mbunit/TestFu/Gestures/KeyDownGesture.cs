using System;
using System.Windows.Forms;

namespace TestFu.Gestures
{
    public class KeyDownGesture : KeyGestureBase
    {
        public KeyDownGesture()
        { }

        public KeyDownGesture(Form form, char key)
            : base(form,key)
        { }

        public override void Start()
        {
            VirtualInput.KeyDown(this.Form.Handle, Key);
        }

        public override Refly.CodeDom.Expressions.Expression ToCodeDom(Refly.CodeDom.Expressions.Expression factory)
        {
            throw new NotImplementedException();
        }
    }
}
