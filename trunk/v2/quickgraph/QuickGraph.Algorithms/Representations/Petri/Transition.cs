using System;

namespace QuickGraph.Representations.Petri
{
	using QuickGraph;
	using QuickGraph.Concepts.Petri;

	public class Transition : Vertex, ITransition
	{
		private string name;
		private IConditionExpression condition = new AllwaysTrueConditionExpression();

		public Transition(int id,string name)
			:base(id)
		{this.name=name;}
		#region ITransition Members

		public IConditionExpression Condition
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition=value;
			}
		}

		#endregion

		public string Name
		{get{return name;}}

		public override string ToString()
		{
			return String.Format("T({0})",this.name);
		}

	}
}
