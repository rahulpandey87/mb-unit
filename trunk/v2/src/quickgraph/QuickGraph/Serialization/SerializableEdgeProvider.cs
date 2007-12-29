using System;

namespace QuickGraph.Serialization
{
	using QuickGraph.Providers;
	public sealed class SerializableEdgeProvider : TypedEdgeProvider
	{
		public SerializableEdgeProvider()
			:base(typeof(SerializableEdge))
		{}
	}
}
