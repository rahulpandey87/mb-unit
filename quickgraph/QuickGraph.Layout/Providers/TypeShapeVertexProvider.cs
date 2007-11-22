using System;
using System.Reflection;
using QuickGraph.Concepts;
using Netron;

namespace QuickGraph.Layout.Providers
{
	/// <summary>
	/// Summary description for DefaultShapeVertexProvider.
	/// </summary>
	public class TypeShapeVertexProvider : IShapeVertexProvider
	{
		private Type shapeType;
		private ConstructorInfo constructor;

		public TypeShapeVertexProvider(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (t!=typeof(Connection) && !t.IsSubclassOf(typeof(Shape)))
				throw new ArgumentException("type not assignalge with Netron.Shape");

			this.shapeType = t;
			this.constructor = t.GetConstructor(Type.EmptyTypes);
			if (this.constructor == null)
				throw new ArgumentException("type does not contain default constructor");
		}

		#region IShapeVertexProvider Members

		public event ShapeVertexEventHandler FormatShape;

		protected void OnFormatShape(Shape shape, IVertex v)
		{
			if (this.FormatShape!=null)
				FormatShape(this,new ShapeVertexEventArgs(shape,v));
		}

		public Netron.Shape ProvideShape(QuickGraph.Concepts.IVertex v)
		{
			Shape shape = (Shape)this.constructor.Invoke(null);
			OnFormatShape(shape,v);

			return shape;
		}

		#endregion
	}
}
