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
    internal class TypeListTypeEditor : ListTypeEditor
    {
        private Hashtable assemblies = new Hashtable();

        protected override void FillList(
            ListTypeEditor.EditorListBox list,
            ITypeDescriptorContext context,
            IServiceProvider provider,
            object value)
        {
            list.Items.Clear();
            this.assemblies.Clear();

            list.BeginUpdate();
            list.Items.Add(typeof(void));
            this.FillTypesFromAssembly(list, typeof(Exception).Assembly);
            list.EndUpdate();
        }

        protected virtual void FillTypesFromAssembly(ListTypeEditor.EditorListBox list, Assembly assembly)
        {
            if (assembly == null)
                return;
            if (this.assemblies.Contains(assembly))
                return;
            this.assemblies.Add(assembly, null);
            foreach (Type t in assembly.GetExportedTypes())
            {
                if (!typeof(Exception).IsAssignableFrom(t))
                    continue;
                list.Items.Add(t);
            }
        }
    }
}
