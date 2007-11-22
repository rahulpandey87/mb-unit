using System;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.Providers
{
	public interface IShapeVertexProvider
	{
		Shape ProvideShape(IVertex v);
	}	
}
