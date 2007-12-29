/*
 * Created by SharpDevelop.
 * User: dehalleux
 * Date: 10/06/2004
 * Time: 9:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	
	public interface IPotential
	{	
		IIteratedLayoutAlgorithm Algorithm {get;set;}
		void Compute(IVertexPointFDictionary potentials);
	}	
}
