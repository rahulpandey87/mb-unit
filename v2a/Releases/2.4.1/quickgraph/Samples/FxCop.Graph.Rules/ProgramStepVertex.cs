using System;
using QuickGraph;
using System.Reflection;
using System.IO;

namespace FxCop.Graph.Rules
{
	using Microsoft.Tools.FxCop.Sdk.Reflection.IL;

	public class ProgramStepVertex : QuickGraph.Vertex
	{
		private ProgramStep instruction=null;
		
		public ProgramStepVertex(int id)
			:base(id)
		{}
		
		public ProgramStep Instruction
		{
			get
			{
				if (this.instruction==null)
					throw new InvalidOperationException();
				return this.instruction;
			}
			set
			{
				this.instruction = value;
			}
		}
		
		public override string ToString()
		{
			return this.Instruction.ToString();
		}
	}
	
	public class ProgramStepVertexProvider : QuickGraph.Providers.TypedVertexProvider
	{
		public ProgramStepVertexProvider()
			:base(typeof(ProgramStepVertex))
		{}
	}
}
