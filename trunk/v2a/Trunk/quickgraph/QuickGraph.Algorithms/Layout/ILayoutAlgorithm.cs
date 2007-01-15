/*
 * Created by SharpDevelop.
 * User: dehalleux
 * Date: 10/06/2004
 * Time: 8:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;

	
	public interface ILayoutAlgorithm : IAlgorithm
	{
		new IVertexAndEdgeListGraph VisitedGraph {get;}
		IVertexPointFDictionary Positions{get;}
		
		float EdgeLength {get;set;}
		void UpdateEdgeLength();
		
		Size LayoutSize {get;set;}
		void Compute();
		void RequestComputationAbortion();
		
		event EventHandler PreCompute;
		event EventHandler PostCompute;
	}
}
