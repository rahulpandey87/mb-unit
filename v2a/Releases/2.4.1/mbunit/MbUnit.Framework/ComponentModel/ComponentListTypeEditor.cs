using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace MbUnit.Framework.ComponentModel
{
    internal class ComponentListTypeEditor : ListTypeEditor
    {
        public Type targetType;

        public ComponentListTypeEditor()
        :this(typeof(IComponent))
        {}

        public ComponentListTypeEditor(Type targetType)
        {
            this.targetType = targetType;
        }

        protected override void FillList(
            EditorListBox list, 
            ITypeDescriptorContext context, 
            IServiceProvider provider, 
            object value)
        {
            list.Items.Clear();
            IDesignerHost designer = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designer == null || designer.Container==null)
                return;

            foreach (IComponent component in designer.Container.Components)
            {
                if (this.targetType.IsAssignableFrom(component.GetType()))
                    list.Items.Add(component);
            }
        }
    }
}
