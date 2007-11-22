using System;

namespace QuickGraph.Concepts.Serialization
{
	public interface IGraphBiSerializable :
		IGraphSerializable,
		IGraphDeSerializable
	{
	}
}
