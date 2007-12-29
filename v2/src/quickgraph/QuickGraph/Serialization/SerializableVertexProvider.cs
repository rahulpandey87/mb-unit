using System;

namespace QuickGraph.Serialization
{
	using QuickGraph.Providers;

	public sealed class SerializableVertexProvider : TypedVertexProvider
	{
		public SerializableVertexProvider()
			:base(typeof(SerializableVertex))
		{}
	}
}
