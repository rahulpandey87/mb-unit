using System;
using System.Collections;
using System.IO;
using System.Collections.Specialized;

namespace FxCop.Graph.Rules
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;
	
	using Microsoft.Cci;

	public class NullStateVisitor : IEdgeColorizerVisitor
	{
		private Hashtable edgeNullStates = new Hashtable();
		private Hashtable vertexNullStates = new Hashtable();
		private StringWriter log =new StringWriter();
		private int parameterIndex=-1;
		private InstructionGraph graph;
		
		private NullState nullState;
		private InstructionVertex source;
		private InstructionVertex target;

		private StringCollection warnings = new StringCollection();
		private StringCollection errors = new StringCollection();
		
		public NullStateVisitor(InstructionGraph graph)
		{
			this.graph=graph;
		}
		
		public InstructionGraph Graph
		{
			get
			{
				return this.graph;
			}
		}
		
		public int ParameterIndex
		{
			get
			{
				return this.parameterIndex;
			}
			set
			{
				this.parameterIndex=value;
			}
		}

		public StringCollection Warnings
		{
			get
			{
				return this.warnings;
			}
		}

		public StringCollection Errors
		{
			get
			{
				return this.errors;
			}
		}
		
		public void Clear()
		{
			this.edgeNullStates.Clear();
			this.vertexNullStates.Clear();
			this.warnings.Clear();
			this.errors.Clear();
		}
		
		public void InitializeEdge(object sender, EdgeEventArgs args)
		{
			// put vertex to uncertain
			if (!this.edgeNullStates.Contains(args.Edge))
			{
				this.edgeNullStates.Add(args.Edge, NullState.Uncertain);	
				this.vertexNullStates[args.Edge.Source]=NullState.Uncertain;	
				this.vertexNullStates[args.Edge.Target]=NullState.Uncertain;	
			}
		}
		
		public void FinishEdge(Object sender, EdgeEventArgs args)
		{}
		
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			// get the state of the previous vertex (IL)
			this.getNullState(args.Edge);
			//	log.WriteLine("TreeEdge: {0}:{1}->{2}, {3}",args.Edge.ID,args.Edge.Source,args.Edge.Target,this.nullState);
			this.source = (InstructionVertex)args.Edge.Source;
			this.target = (InstructionVertex)args.Edge.Target;
			
			// propagate state
			foreach(IEdge e in this.graph.OutEdges((InstructionVertex)args.Edge.Target))
			{
				this.edgeNullStates[e]=this.nullState;
				this.vertexNullStates[e.Target]=this.nullState;
			}
			
			//this.displayInstruction();
			// check state
			if (this.nullState!=NullState.NonNull)
			{
				this.checkMethodInvoke();
			}
			
			// update next state
			this.updateBrTrueFalse();
		}
		
		public StringWriter Log
		{
			get
			{
				return this.log;
			}
		}
		
		private void getNullState(IEdge e)
		{
			NullState edgeNullState = (NullState)this.edgeNullStates[e];	
			NullState vertexNullState = (NullState)this.vertexNullStates[e.Target];	

			if (vertexNullState != NullState.Uncertain)
				this.nullState=vertexNullState;
			else
				this.nullState=edgeNullState;
		}
		
		private string GetParameterIL()
		{
			return string.Format("ldarg.{0}",this.parameterIndex);
		}
		
		private bool isSourceArgument()
		{
			return source.Instruction.OpCode.ToString()== GetParameterIL();
		}
		
		private void displayInstruction()
		{
			if (source.Instruction.Value!=null)
				log.WriteLine("From {0}:{1}",source.Instruction.Value.GetType().Name,
					source.Instruction.Value);
			if (target.Instruction.Value!=null)
				log.WriteLine("Examining {0}:{1}",target.Instruction.Value.GetType().Name,
					target.Instruction.Value);
			else
				log.WriteLine("Examining null");
		}
		
		private void updateBrTrueFalse()
		{
			// check if brtrue
			bool isBrTrue = target.Instruction.OpCode.ToString()== "brtrue.s";
			bool isBrFalse = target.Instruction.OpCode.ToString() == "brfalse.s";
			if (isBrTrue ||isBrFalse)
			{
				//				log.WriteLine("Found {0} {1}",target.Instruction, target.Instruction.Value);
				// check if argument before
				if (isSourceArgument())
				{
					NullState then = NullState.Null;
					NullState else_ = NullState.NonNull;
					if (isBrFalse)
					{
						then = NullState.NonNull;
						else_ = NullState.Null;
					}
					// this edge represents if (arg==null)
					// the "then" edge, will be null,					
					// the "else edge, will be non-null
					foreach(IEdge e in this.graph.OutEdges(target))
					{
						Instruction il = ((InstructionVertex)e.Target).Instruction;
						// then
						sbyte targetOffset = (sbyte)target.Instruction.Value;
						if (il.Offset!=targetOffset)
						{
							this.edgeNullStates[e]=then;
							this.vertexNullStates[target]=then;
						}
						else
						{
							this.edgeNullStates[e]=else_;
							this.vertexNullStates[target]=else_;
						}
					}
				}
			}
		}
		
		private void checkMethodInvoke()
		{
			// check if method call
			if (target.Instruction.NodeType==NodeType.Call)
			{
				if (source.Instruction.OpCode.ToString() == GetParameterIL())
				{
					switch(nullState)
					{
						case NullState.Uncertain:
							this.warnings.Add(String.Format("{0} called on an uncertain object",target.Instruction.Value));
							break;
						case NullState.Null:
							this.errors.Add(String.Format("{0} called on a null object",target.Instruction.Value));
							break;
						default:
							throw new Exception();
					}
				}
			}
		}
	}
}
