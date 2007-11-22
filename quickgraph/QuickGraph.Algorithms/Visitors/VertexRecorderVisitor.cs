using System;

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Collections;

	/// <summary>
	/// A visitor that records vertices.
	/// </summary>
	public class VertexRecorderVisitor
	{
		private VertexCollection vertices;

		/// <summary>
		/// Create an empty vertex visitor
		/// </summary>
		public VertexRecorderVisitor()
		{
			this.vertices = new VertexCollection();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vertices"></param>
		public VertexRecorderVisitor(VertexCollection vertices)
		{
			if (vertices==null)
				throw new ArgumentNullException("vertices");
			this.vertices = vertices;
		}

		/// <summary>
		/// Recorded vertices
		/// </summary>
		public VertexCollection Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		/// <summary>
		/// Record vertex handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void RecordVertex(Object sender, VertexEventArgs args)
		{
			this.vertices.Add(args.Vertex);
		}

		/// <summary>
		/// Record vertex handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void RecordTarget(Object sender, EdgeEventArgs args)
		{
			this.vertices.Add(args.Edge.Target);
		}

		/// <summary>
		/// Record vertex handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void RecordSource(Object sender, EdgeEventArgs args)
		{
			this.vertices.Add(args.Edge.Source);
		}
	}
}
