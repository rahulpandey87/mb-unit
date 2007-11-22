using System;

namespace QuickGraphTest
{
	using QuickGraph;
	using QuickGraph.Concepts;
	using QuickGraph.Representations;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// Provides canal examples
	/// </summary>
	public sealed class CanalProvider
	{
		private CanalProvider()
		{}

		/// <summary>
		/// Cascade graph
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static AdjacencyGraph CreateCascade(int n)
		{
			IVertex previous, next;

			AdjacencyGraph g = new AdjacencyGraph();
			next = g.AddVertex();
			for(int i = 0;i<n;++i)
			{
				previous = next;
				next = g.AddVertex();
				g.AddEdge(previous,next);
			}
			return g;
		}

		/// <summary>
		/// Single canal
		/// </summary>
		/// <returns></returns>
		public static AdjacencyGraph CreateSingle()
		{
			return CreateCascade(1);        
		}

		/// <summary>
		/// Star canal
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static AdjacencyGraph CreateStar(int n)
		{
			AdjacencyGraph g = new AdjacencyGraph();
			IVertex root = g.AddVertex();
			for(int i = 0;i<n;++i)
			{
				IVertex v = g.AddVertex();
				g.AddEdge(root, v);
			}
			return g;
		}

		/// <summary>
		/// Reversed star canal
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static AdjacencyGraph CreateReversedStart(int n)
		{  
			AdjacencyGraph g = new AdjacencyGraph();
			IVertex root = g.AddVertex();
			for(int i = 0;i<n;++i)
			{
				IVertex v = g.AddVertex();
				g.AddEdge(v,root);
			}
			return g;
		}
	}

	/// <summary>
	/// Characteristic graph
	/// </summary>
	public class CharacteristicGraph
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="avs"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		protected CharacteristicVertex FindTargetVertex(VertexCollection avs,IEdge e)
		{
			foreach(CharacteristicVertex avtarget in avs)
			{
				if (avtarget.IncomingEdge==e)
					return avtarget;
			}
			throw new Exception("could not find target vertex");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="assg"></param>
		public void Transform(IBidirectionalGraph g, IMutableVertexAndEdgeListGraph assg)
		{
			VertexCollection avs = new VertexCollection();
			// adding vertices
			foreach(IEdge e in g.Edges)
			{
				// xi_-(L) = g(xi_-(0), xi_+(L))
				CharacteristicVertex avm = (CharacteristicVertex)assg.AddVertex();
				avm.IncomingEdge = e;
				avm.Vertex = e.Target;
				avs.Add(avm);

				// xi_+(0) = g(xi_-(0), xi_+(L))
				CharacteristicVertex avp = (CharacteristicVertex)assg.AddVertex();
				avp.IncomingEdge = e;
				avp.Vertex = e.Source;
				avs.Add(avp);
			}

			// adding out edges
			foreach(CharacteristicVertex av in avs)
			{
				foreach(IEdge e in g.OutEdges(av.Vertex))
				{
					// find target vertex:
					CharacteristicVertex avtarget = FindTargetVertex(e);
					// add xi_-
					CharacteristicEdge aem = (CharacteristicEdge)assg.AddEdge(av,avtarget);
					aem.Positive = false;
					aem.Edge = e;
				}
				foreach(IEdge e in g.InEdges(av.Vertex))
				{
					// find target vertex:
					CharacteristicVertex avtarget = FindTargetVertex(e);
					// add xi_-
					CharacteristicEdge aem = (CharacteristicEdge)assg.AddEdge(av,avtarget);
					aem.Positive = true;
					aem.Edge = e;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		public void DisplayAssGraph(IVertexAndEdgeListGraph g)
		{
			foreach(IVertex v in g.Vertices)
			{
				Console.Write("{0}{1}: ",
					v.IncomingEdge.ID,v.Vertex.ID
					);
				if (v.IncomingEdge.Target == v.Target ) // at L
					Console.Write("xi_-^{0}(L) -> ", v.IncomingEdge.ID);
				else 
					Console.Write("xi_+^{0}(L) -> ", v.IncomingEdge.ID);

				foreach(CharacteristicEdge e in g.SelectOutEdges(v, new NegativeCharacteristicEdgePredicate()) )
					Console.Write("xi_-^{0}(0) ",e.Edge.ID);
				foreach(CharacteristicEdge e in g.SelectOutEdges(v, new PositiveCharacteristicEdgePredicate()) )
					Console.Write("xi_+^{0}(L) ",e.Edge.ID);
				Console.WriteLine(")");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		public void DisplayGraph(IVertexAndEdgeListGraph g)
		{
			foreach(IVertex v in g.Vertices)
			{
				Console.Write("{0}: ",v.ID);
				foreach(IEdge e in g.OutEdges(v))
					Console.Write("{0} ",e.ID);
				Console.WriteLine();
			}
		}

	}
}
