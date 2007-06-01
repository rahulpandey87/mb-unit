using System;

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// An predicate that checks that the edge is not in both circuit
	/// and temporary circuit.
	/// </summary>
	public class NotInCircuitEdgePredicate : IEdgePredicate
	{
		private IEdgeCollection circuit;
		private IEdgeCollection temporaryCircuit;

		/// <summary>
		/// Construct an edge that filters out edge in circuit 
		/// and temporary circuit
		/// </summary>
		/// <param name="circuit"></param>
		/// <param name="temporaryCircuit"></param>
		public NotInCircuitEdgePredicate(
			IEdgeCollection circuit,
			IEdgeCollection temporaryCircuit
			)
		{
			if (circuit == null)
				throw new ArgumentNullException("circuit");
			if (temporaryCircuit==null)
				throw new ArgumentNullException("temporaryCircuit");
			this.circuit = circuit;
			this.temporaryCircuit = temporaryCircuit;
		}

		/// <summary>
		/// Edge circuit
		/// </summary>
		public IEdgeCollection Circuit
		{
			get
			{
				return circuit;
			}
		}

		/// <summary>
		/// Temporary circuit
		/// </summary>
		public IEdgeCollection TemporaryCircuit
		{
			get
			{
				return temporaryCircuit;
			}
		}

		/// <summary>
		/// Test method
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool Test(IEdge e)
		{
			return !Circuit.Contains(e)&&!TemporaryCircuit.Contains(e);
		}
	}
}
