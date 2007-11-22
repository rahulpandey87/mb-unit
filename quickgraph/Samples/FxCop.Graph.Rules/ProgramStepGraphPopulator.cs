
namespace FxCop.Graph.Rules
{
	using System;
	using System.Collections;
	using QuickGraph.Exceptions;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Concepts.Traversals;
	using Microsoft.Tools.FxCop.Sdk.Reflection;
	using Microsoft.Tools.FxCop.Sdk.Reflection.IL;

	public class ProgramStepGraphPopulator
	{
		private Hashtable instructionVertices = null;
		private ProgramStepGraph graph=null;

		public ProgramStepGraphPopulator()
		{}
		public ProgramStepGraph BuildGraphFromMethod(Method method)
		{			
			if (method==null)
				throw new ArgumentNullException("method");
			// create graph			
			this.graph = new ProgramStepGraph(false);
			this.instructionVertices = new Hashtable();
			
			// first add all instructions			
			foreach(ProgramStep i in method.DataFlow)
			{
				// avoid certain instructions
				if (i.Operation.Flow == ControlFlow.Meta
					|| i.Operation.Flow == ControlFlow.Meta)
					continue;
				
				// add vertex
				ProgramStepVertex iv = graph.AddVertex();
				iv.Instruction = i;
				
				this.instructionVertices.Add((uint)i.Offset,iv);
			}
				
			// iterating over the instructions
			search(null, method.DataFlow.GetEnumerator());
			
			// iterating of the the try/catch handler
			searchExceptions(method.RetrieveEHClauses());
			
			return this.graph;
		}

		public ProgramStepGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		public EdgeCollectionCollection GetAllEdgePaths()
		{
			if (this.graph.VerticesCount==0)
				return new EdgeCollectionCollection();

			DepthFirstSearchAlgorithm efs = new DepthFirstSearchAlgorithm(this.graph);
			PredecessorRecorderVisitor vis =new PredecessorRecorderVisitor();

			efs.RegisterPredecessorRecorderHandlers(vis);

			// get root vertex
			efs.Compute(this.Graph.Root);

			return vis.AllPaths();
		}
		
		private void searchExceptions(ICollection exceptions)
		{
			if (exceptions==null)
				return;
			
			// handle all catch			
			foreach(EHClause handler in exceptions)
			{
				if (handler.TryOffset == handler.HandlerOffset)
					continue;
				ProgramStepVertex tv = vertexFromOffset(handler.TryOffset);
				
				if (handler.Type == EHClauseTypes.EHNone)
				{
					ProgramStepVertex cv = vertexFromOffset(handler.HandlerOffset);
					graph.AddEdge(tv,cv);
				}
				if (handler.Type == EHClauseTypes.EHFilter)
				{
					ProgramStepVertex cv = vertexFromOffset(handler.ClassTokenOrFilterOffset);
					graph.AddEdge(tv,cv);
				}				
				if (handler.Type == EHClauseTypes.EHFinally)
				{
					ProgramStepVertex fv = vertexFromOffset(handler.HandlerOffset);
					graph.AddEdge(tv,fv);
					foreach(EHClause catchHandler in exceptions)
					{
						if (catchHandler.TryOffset == catchHandler.HandlerOffset)
							continue;
						if (handler.TryOffset != catchHandler.TryOffset)
							continue;
						if (handler.HandlerOffset == catchHandler.HandlerOffset)
							continue;
						ProgramStepVertex cv = vertexFromOffset(catchHandler.HandlerOffset);
						graph.AddEdge(cv,fv);
					}
				}
			}							
		}
		
		[Obsolete("Tweaked for fxcop bug+3")]
		private void search(
			ProgramStepVertex parentVertex, 
			IEnumerator instructions)
		{
			ProgramStepVertex jv = null;
			ProgramStepVertex cv = parentVertex;
			while(instructions.MoveNext())
			{
				// add vertex to graph
				ProgramStep i = (ProgramStep)instructions.Current;
				ControlFlow f = i.Operation.Flow;
				// avoid certain instructions
				if (f == ControlFlow.Phi 
					|| f == ControlFlow.Meta)
					continue;

				ProgramStepVertex iv = vertexFromOffset((uint)i.Offset);
				
				if (cv!=null)
				{
					graph.AddEdge(cv, iv);
				}				
				// find how to handle the rest
				switch(f)
				{
					case ControlFlow.Next:
						cv = iv;
						break;
					case ControlFlow.Call:
						cv=iv;
						break;
					case ControlFlow.Return:
						cv = null;
						break;
					case ControlFlow.Throw:
						cv = null;
						break;
					case ControlFlow.ConditionalBranch:
						if (i.Operation.StringName == "switch")
						{
							foreach(int target in (int[])i.Operand)
							{
								jv = vertexFromOffset((uint)target);
								graph.AddEdge(iv,jv);
								search(iv,instructions);
							}
							cv=iv;						
						}
						else
						{
							sbyte targetOffset = (System.SByte)iv.Instruction.Operand;							
							jv = vertexFromOffset((uint)targetOffset+3);
							graph.AddEdge(iv,jv);
							cv = iv;
						}
						break;
					case ControlFlow.Branch:
						if (i.Operation.StringName.StartsWith("brtrue") 
							|| i.Operation.StringName.StartsWith("brfalse")
							)
						{	
							// add jump to offset
							sbyte targetOffset = (System.SByte)iv.Instruction.Operand;							
							jv = vertexFromOffset((uint)targetOffset+3);
							if (jv==null)
								throw new VertexNotFoundException("Could not find vertex");
							graph.AddEdge(iv,jv);
							cv=null;
							break;
						}						
						break;
					case ControlFlow.Break:
						// add jump to offset
						jv = vertexFromOffset((uint)iv.Instruction.Offset);
						graph.AddEdge(iv,jv);
						cv = null;
						break;
				}
			}
		}
		
		private ProgramStepVertex vertexFromOffset(uint offset)
		{	
			ProgramStepVertex iv =  (ProgramStepVertex)this.instructionVertices[offset];
			if (iv==null)
				throw new InvalidOperationException("Could not find vertex at offset " + offset.ToString());
			return iv;
		}
	}
}
