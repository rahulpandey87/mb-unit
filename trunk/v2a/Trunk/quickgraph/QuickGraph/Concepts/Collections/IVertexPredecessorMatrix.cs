using System;

namespace QuickGraph.Concepts.Collections
{
	/// <summary>
	/// 
	/// </summary>
	public interface IVertexPredecessorMatrix
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		IVertex Predecessor(IVertex source, IVertex target);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="predecessor"></param>
		void SetPredecessor(IVertex source, IVertex target, IVertex predecessor);
	}
}
