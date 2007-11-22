using System;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.ConnectorChoosers
{
	/// <summary>
	/// Defines a connector chooser class
	/// </summary>
	/// <remarks>
	/// The connector chooser is, given a <see cref="Connection"/> and
	/// two <see cref="Shape"/> instances, find the best connectors.
	/// </remarks>
	public interface IConnectorChooser
	{
		/// <summary>
		/// Connects <paramref name="conn"/>
		/// </summary>
		/// <param name="e">edge reprensted by conn</param>
		/// <param name="conn">the <see cref="Connection"/> instance</param>
		/// <param name="source">the <see cref="Shape"/> source</param>
		/// <param name="target">the <see cref="Shape"/> target</param>
		void Connect(
			IEdge e,
			Connection conn, 
			Shape source, 
			Shape target
			);
	}
}
