using System;
using System.Collections;
using System.Reflection;

namespace FxCop.Graph.Rules
{
	using Microsoft.Tools.FxCop.Sdk;
	using Microsoft.Tools.FxCop.Sdk.Introspection;
	using Microsoft.Cci;
	using QuickGraph.Algorithms.Search;

	public class DanglingArgumentRule : BaseIntrospectionRule
	{
		private InstructionGraphPopulator populator = new InstructionGraphPopulator();

		public DanglingArgumentRule()
			:base(
				typeof(DanglingArgumentRule).Name,
				typeof(DanglingArgumentRule).Namespace+".Resource",
				typeof(DanglingArgumentRule).Assembly
			)
		{}

		#region IReflectionRule Members

		public override ProtectionLevels NestedTypeProtectionLevel
		{
			get
			{
				return ProtectionLevels.Public | 
					ProtectionLevels.Family | 
					ProtectionLevels.NestedAssembly;
			}
		}
		#endregion

		public override Problem[] Check(Method method)
		{
			this.populator.BuildGraphFromMethod(method);

			EdgeDepthFirstSearchAlgorithm edfs = new EdgeDepthFirstSearchAlgorithm(this.populator.Graph);
			NullStateVisitor vis = new NullStateVisitor(this.populator.Graph);
			edfs.RegisterEdgeColorizerHandlers(vis);
			
			vis.Log.WriteLine("-- Fault detection for {0}.{1}",
				method.DeclaringType.ToString(),
				method.Name
				);
			
			int index=0;
			ArrayList problems = new ArrayList();
			for(index=0;index<method.Parameters.Length;++index)
			{
				Parameter param = method.Parameters[index];
				vis.Clear();
				vis.Log.WriteLine("-- {0} parameter",param.Name);
				vis.ParameterIndex=index;
				edfs.Initialize();
				edfs.Compute( this.populator.Graph.Root );

				if (vis.Warnings.Count>0 || vis.Errors.Count > 0)
				{
					this.Problems.Add(
						this.GetResolution(method.Name.Name,param.Name.Name)
						);				
				}
			}

			Problem[] ps=new Problem[problems.Count];
			problems.CopyTo(ps,0);

			return ps;
		}
	}
}
