using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts.Collections;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts;

    /// <summary>
	/// <para>
	/// A <em>TestGraph</em> as defined in the section 2 of the article.
	/// </para>
	/// <para>
	/// </para>
	/// </summary>
	public interface ITestGraph
	{
		/// <summary>
		/// Gets the underlying <see cref="IBidirectionalVertexAndEdgeListGraph"/>
		/// graph representing the Finite State Machine.
		/// </summary>
		/// <value>
		/// <see cref="IBidirectionalVertexAndEdgeListGraph"/> instance representing
		/// the fsm.
		/// </value>
		IBidirectionalVertexAndEdgeListGraph Graph {get;}

		/// <summary>
		/// Get the state enumerable collection (V-CP).
		/// </summary>
		/// <value>
		/// State vertices enumerable collection.
		/// </value>
		IVertexEnumerable States {get;}

		/// <summary>
		/// Gets a value indicating if <paramref name="v"/> is in the state set.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>true if <paramref name="v"/> is in the state set</returns>
		bool ContainsState(IVertex v);

		/// <summary>
		/// Get the choice point enumerable collection (CP).
		/// </summary>
		/// <value>
		/// Choice point vertices enumerable collection.
		/// </value>
		IVertexEnumerable ChoicePoints {get;}

		/// <summary>
		/// Gets a value indicating if <paramref name="v"/> is in CP.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>true if <paramref name="v"/> is in CP</returns>
		bool ContainsChoicePoint(IVertex v);

		/// <summary>
		/// Gets a probability associated to the <see cref="IEdge"/>
		/// <paramref name="e"/>.
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>Probability associated to <paramref name="e"/></returns>
		double Prob(IEdge e);

		/// <summary>
		/// Gets a cost associated to the <see cref="IEdge"/>
		/// <paramref name="e"/>.
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>Cost associated to <paramref name="e"/></returns>
		double Cost(IEdge e);
	}
}
