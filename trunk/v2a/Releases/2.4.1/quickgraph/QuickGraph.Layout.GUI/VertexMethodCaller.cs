using System;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.GUI
{
	public delegate void ComputeVertexDelegate(IVertex v);
	/// <summary>
	/// Summary description for VertexMethodCaller.
	/// </summary>
	public class VertexMethodCaller
	{
		private ComputeVertexDelegate del;
		private IVertex v;
		public VertexMethodCaller(ComputeVertexDelegate del, IVertex v)
		{
			this.del  = del;
			this.v = v;
		}

		public void Run()
		{
			this.del(v);
		}
	}

	public delegate void ComputeEdgeDelegate(IEdge v);
	/// <summary>
	/// Summary description for VertexMethodCaller.
	/// </summary>
	public class EdgeMethodCaller
	{
		private ComputeEdgeDelegate del;
		private IEdge e;
		public EdgeMethodCaller(ComputeEdgeDelegate del, IEdge e)
		{
			this.del  = del;
			this.e = e;
		}

		public void Run()
		{
			this.del(e);
		}
	}
}
