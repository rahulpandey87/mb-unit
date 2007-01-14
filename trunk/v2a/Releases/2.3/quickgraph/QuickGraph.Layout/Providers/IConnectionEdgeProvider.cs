using System;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.Providers
{
	public interface IConnectionEdgeProvider
	{
		Connection ProvideConnection(IEdge e);
	}
}
