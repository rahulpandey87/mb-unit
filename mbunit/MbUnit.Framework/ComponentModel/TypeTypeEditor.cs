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
    internal class TypeTypeEditor : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context!=null)
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

            TypeListBox list = new TypeListBox(editorService);
            list.Target = value as Type;
            editorService.DropDownControl(list);

            return list.SelectedItem;
        }

        public class TypeListBox : ListBox
        {
            private Type target;
            private IWindowsFormsEditorService editorService;
            private Hashtable assemblies = new Hashtable();

            public TypeListBox(IWindowsFormsEditorService editorService)
            {
                this.editorService = editorService;
                this.Sorted = true;
            }

            public Type Target
            {
                get
                {
                    return this.target;
                }
                set
                {
                    bool changed = this.target != value;
                    this.target =value;
                    if(changed)
                        this.OnTargetChanged(EventArgs.Empty);
                }
            }

            public event EventHandler TargetChanged;
            protected virtual void OnTargetChanged(EventArgs e)
            {
                if (this.TargetChanged!=null)
                    this.TargetChanged(this,EventArgs.Empty);

                this.FillTypes();

                this.SelectedItem = this.Target;
            }

            protected virtual void FillTypes()
            {
                this.Items.Clear();
                if (this.target==null)
                    return;

                this.assemblies.Clear();

                this.BeginUpdate();
                this.FillTypesFromAssembly(typeof(Exception).Assembly);
                this.FillTypesFromAssembly(this.Target.Assembly);
                this.FillTypesFromAssembly(Assembly.GetCallingAssembly());
                this.EndUpdate();
            }

            protected virtual void FillTypesFromAssembly(Assembly assembly)
            {
                if (assembly == null)
                    throw new ArgumentNullException("assembly");
                if (this.assemblies.Contains(assembly))
                    return;
                this.assemblies.Add(assembly, null);
                foreach (Type t in assembly.GetExportedTypes())
                {
                    if (!typeof(Exception).IsAssignableFrom(t))
                        continue;
                    this.Items.Add(t);
                }
            }

            protected override void OnClick(EventArgs e)
            {
                this.Target = this.SelectedItem as Type;
                this.editorService.CloseDropDown();
            }
        }
    }
}
