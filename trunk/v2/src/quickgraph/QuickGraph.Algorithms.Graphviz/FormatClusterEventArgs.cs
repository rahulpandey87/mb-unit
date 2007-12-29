using System;

namespace QuickGraph.Algorithms.Graphviz
{
	using NGraphviz.Helpers;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// A clustered graph event argument.
	/// </summary>
	public class FormatClusterEventArgs : EventArgs
	{
		private IVertexAndEdgeListGraph cluster;
		private GraphvizGraph graphFormat;

		/// <summary>
		/// Construct a clustered graph event argument
		/// </summary>
		/// <param name="cluster">cluster to send to event handlers</param>
		/// <param name="graphFormat">cluster formatter</param>
		/// <exception cref="ArgumentNullException">cluster is a null reference.
		/// </exception>
		public FormatClusterEventArgs(IVertexAndEdgeListGraph cluster, GraphvizGraph graphFormat)
		{
			if (cluster==null)
				throw new ArgumentNullException("cluster");
			this.cluster = cluster;
			this.graphFormat = graphFormat;
		}

		/// <summary>
		/// Cluster
		/// </summary>
		public IVertexAndEdgeListGraph Cluster
		{
			get
			{
				return cluster;
			}
		}

		/// <summary>
		/// Cluster format
		/// </summary>
		public GraphvizGraph GraphFormat
		{
			get
			{
				return graphFormat;
			}
		}
	}

	/// <summary>
	/// Clustered graph event
	/// </summary>
	public delegate void FormatClusterEventHandler(
		Object sender,
		FormatClusterEventArgs e
	);
}
