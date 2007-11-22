using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing;

using Refly.CodeDom;
using Refly.CodeDom.Expressions;
using Refly.CodeDom.Statements;

namespace TestFu.Gestures
{
    public class GestureRecorder : Refly.Templates.Template, IMessageFilter
    {
        private bool mouseMovement = true;
        private Form targetForm=null;
        private SequenceGesture gestures=null;
        private GestureFactory factory=null;

        private bool leftDown = false;
        private bool rightDown = false;
        private bool isMoving=false;
        private Control sourceControl=null;

        private const int WM_ACTIVATE = 0x6;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;

        public GestureRecorder()
            :base("Gestures","{0}Gesture")
        {
        }

        public Form TargetForm
        {
            get
            {
                return this.targetForm;
            }
            set
            {
                if(value==null)
                    throw new ArgumentNullException("value");
                this.targetForm=value;
                this.factory = new GestureFactory(this.targetForm);
                this.gestures = new SequenceGesture(this.targetForm);
            }
        }

        [Category("Output")]
        public bool MouseMovement
        {
            get
            {
                return this.mouseMovement;
            }
            set
            {
                this.mouseMovement=value;
            }
        }

        public SequenceGesture Gestures
        {
            get
            {
                return this.gestures;
            }
        }

        public void StartRecording()
        {
            this.isMoving=false;
            this.leftDown = false;
            this.rightDown = false;
            this.TargetForm.Show();
            Application.AddMessageFilter(this);
        }

        public void StopRecording()
        {
            Application.RemoveMessageFilter(this);
            this.TargetForm.Close();
        }

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            if(this.TargetForm.IsDisposed)
                return false;
            if (!this.TargetForm.ClientRectangle.Contains(this.TargetForm.PointToClient(Control.MousePosition)))
                return false;

            if (m.Msg==WM_LBUTTONDOWN)
            {
                this.leftDown = true;
                this.isMoving = false;
                return false;
            }
            if (m.Msg==WM_RBUTTONDOWN)
            {
                this.rightDown = true;
                this.isMoving = false;
                return false;
            }
            if (m.Msg==WM_MOUSEMOVE)
            {
                if (!this.isMoving)
                {
                    if (this.leftDown || this.rightDown)
                    {
                        Point p = this.TargetForm.PointToClient(Cursor.Position);
                        this.sourceControl = this.TargetForm.GetChildAtPoint(p);                    
                    }
                }
                this.isMoving = true;
                return false;
            }
            if (m.Msg==WM_LBUTTONUP)
            {
                if (!this.isMoving)
                {
                    this.AddMouseClick(MouseButtons.Left);
                    this.leftDown = false;
                    this.isMoving = false;
                    return false;
                }
                else
                {
                    this.AddMouseDrag(MouseButtons.Left);
                    return false;
                }
            }
            if (m.Msg == WM_RBUTTONUP)
            {
                if (!this.isMoving)
                {
                    this.AddMouseClick(MouseButtons.Right);
                    this.rightDown = false;
                    this.isMoving = false;
                    return false;
                }
                else
                {
                    this.AddMouseDrag(MouseButtons.Right);
                    return false;
                }
            }

            return false;
        }

        
        public override void  Generate()
        {
            this.Prepare();
            // create class
            ClassDeclaration c = this.NamespaceDeclaration.AddClass(PlaybackClassName);

            // add field
            FieldDeclaration f = c.AddField(typeof(SequenceGesture), "gesture");

            // add method
            MethodDeclaration build = c.AddMethod("BuildGesture");
            ParameterDeclaration factory = build.Signature.Parameters.Add(typeof(GestureFactory),"factory");

            // add calls
            build.Body.AddAssign(
                Expr.This.Field(f),
                Expr.New(typeof(SequenceGesture),
                Expr.Arg(factory).Prop("Form"))
                );
            foreach (IGesture gesture in this.Gestures.Gestures)
            {
                Expression expr = gesture.ToCodeDom(Expr.Arg(factory));
                build.Body.Add(
                    Expr.This.Field(f).Method("Add").Invoke(expr)
                    );
            }
            this.Compile();
        }

        protected string PlaybackClassName
        {
            get
            {
                return 
                    String.Format(this.NameFormat, 
                        this.TargetForm.GetType().Name
                        );
            }        
        }

        protected void AddMouseClick(MouseButtons buttons)
        {
            Point p = this.TargetForm.PointToClient(Cursor.Position);
            Control targetControl = this.TargetForm.GetChildAtPoint(p);

            if (targetControl == null)
                return;
            else
               this.gestures.Gestures.Add(
                    this.factory.MouseClick(targetControl,buttons)
                    );
            this.leftDown = false;
            this.rightDown = false;
        }

        protected void AddMouseDrag(MouseButtons buttons)
        {
            Point p = this.TargetForm.PointToClient(Cursor.Position);
            Control targetControl = this.TargetForm.GetChildAtPoint(p);

            this.isMoving = false;
            this.leftDown = false;
            this.rightDown = false;

            if (targetControl == null || this.sourceControl == null)
            {
                return;
            }

            if (targetControl == sourceControl)
                this.AddMouseClick(buttons);
            else
            {
                this.gestures.Gestures.Add(
                    this.factory.MouseDragAndDrop(
                    this.sourceControl,
                        targetControl,
                        buttons
                        )
                    );
            }
        }
    }
}
