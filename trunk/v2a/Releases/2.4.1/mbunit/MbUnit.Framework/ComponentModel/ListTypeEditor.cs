using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;

namespace MbUnit.Framework.ComponentModel
{
    internal abstract class ListTypeEditor : UITypeEditor
    {
        public ListTypeEditor()
        { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.DropDown;
            return base.GetEditStyle(context);
        }

        public override object EditValue(
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            if (context == null || provider == null)
                return base.EditValue(context, provider, value);

            IWindowsFormsEditorService editorService =
                provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService == null)
                return base.EditValue(context, provider, value);

            EditorListBox list = new EditorListBox(editorService);
            this.FillList(list,context, provider, value);
            editorService.DropDownControl(list);

            return list.SelectedItem;
        }

        protected abstract void FillList(EditorListBox list,
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value);

        public class EditorListBox : ListBox
        {
            private IWindowsFormsEditorService editorService;

            public EditorListBox(IWindowsFormsEditorService editorService)
            {
                this.editorService = editorService;
                this.Sorted = true;
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                this.editorService.CloseDropDown();
            }
        }
    }
}
