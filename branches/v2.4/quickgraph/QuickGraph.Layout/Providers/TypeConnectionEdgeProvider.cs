using System;
using System.Reflection;
using QuickGraph.Concepts;
using Netron;

namespace QuickGraph.Layout.Providers
{
	/// <summary>
	/// Summary description for DefaultShapeVertexProvider.
	/// </summary>
	public class TypeConnectionEdgeProvider : IConnectionEdgeProvider
	{
		private Type shapeType;
		private ConstructorInfo constructor;

		public TypeConnectionEdgeProvider(Type t)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (t!=typeof(Connection) && !t.IsSubclassOf(typeof(Connection)))
				throw new ArgumentException("type not assignalge with Netron.Connection");

			this.shapeType = t;
			this.constructor = t.GetConstructor(Type.EmptyTypes);
			if (this.constructor == null)
				throw new ArgumentException("type does not contain default constructor");
		}


		public event ConnectionEdgeEventHandler FormatConnection;

		protected void OnFormatConnection(Connection conn, IEdge e)
		{
			if (this.FormatConnection!=null)
				FormatConnection(this,new ConnectionEdgeEventArgs(conn,e));
		}

		public Netron.Connection ProvideConnection(QuickGraph.Concepts.IEdge e)
		{
			Connection conn=(Connection)this.constructor.Invoke(null);
			OnFormatConnection(conn,e);
			return conn;
		}
	}
}
