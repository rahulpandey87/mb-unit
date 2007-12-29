using System;

namespace QuickGraph.Concepts.Serialization
{
	/// <summary>
	/// Defines an instance that can be deserialized from a
	/// <see cref="IGraphSerializationInfo"/> instance.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public interface IGraphDeSerializable
	{

		/// <summary>
		/// Reads data from serialization info
		/// </summary>
		/// <param name="info">data holder</param>
		/// <exception cref="ArgumentNullException">info is a null reference</exception>
		/// <exception cref="ArgumentException">info is serializing</exception>
		void ReadGraphData(IGraphSerializationInfo info);
	}
}
