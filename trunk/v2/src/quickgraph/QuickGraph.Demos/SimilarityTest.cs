using System;

namespace QuickGraphTest
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Representations;
	using QuickGraph.Algorithms.MatrixAlgebra;

	/// <summary>
	/// Summary description for SimilarityTest.
	/// </summary>
	public sealed class SimilarityTest
	{
		private SimilarityTest()
		{}

		public static void WriteMatrix(GeneralMatrix m)
		{
			if (m==null)
				return;
			for(int i = 0;i<m.RowDimension;++i)
			{
				for(int j = 0;j<m.ColumnDimension;++j)
				{
					Console.Write("{0} ",m.GetElement(i,j));
				}
				Console.WriteLine();
			}
		}

		public static void BasicTest()
		{
			BidirectionalGraph gA = new BidirectionalGraph(false);          
			BidirectionalGraph gB = new BidirectionalGraph(false);          
                        
			IVertex a1 = gA.AddVertex();
			IVertex a2 = gA.AddVertex();
			IVertex a3 = gA.AddVertex();
			IVertex a4 = gA.AddVertex();
                        
			gA.AddEdge(a1,a2);
			gA.AddEdge(a2,a3);
			gA.AddEdge(a3,a1);
			gA.AddEdge(a3,a4);
                        
			SimilarityMatrix similarity = new SimilarityMatrix(gA);
			WriteMatrix(similarity.Matrix);
		}
	}
}
