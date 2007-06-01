using System;

namespace QuickGraph.Concepts.Visitors
{
	/// <summary>
	/// Summary description for IEdgeColorizerVisitor.
	/// </summary>
	public interface IEdgeColorizerVisitor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void InitializeEdge(Object sender, EdgeEventArgs args);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void TreeEdge(Object sender, EdgeEventArgs args);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void FinishEdge(Object sender, EdgeEventArgs args);
	}
}
