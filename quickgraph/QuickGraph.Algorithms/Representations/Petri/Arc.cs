using System;
using QuickGraph.Concepts.Petri;

namespace QuickGraph.Representations.Petri
{
	public class Arc : Edge,IArc
	{
		private bool isInputArc;
		private IPlace place;
		private ITransition transition;
		private IExpression annotation = new IdentityExpression();

		public Arc(int id,IPlace place, ITransition transition)
			:base(id,place,transition)
		{
			this.place=place;
			this.transition=transition;
			this.isInputArc=true;
		}
		public Arc(int id,ITransition transition,IPlace place)
			:base(id,transition,place)
		{
			this.place=place;
			this.transition=transition;
			this.isInputArc=false;
		}

		#region IArc Members

		public bool IsInputArc
		{
			get
			{
				return this.isInputArc;
			}
		}

		public IPlace Place
		{
			get
			{
				return this.place;
			}
		}

		public ITransition Transition
		{
			get
			{
				return this.transition;
			}
		}

		public IExpression Annotation
		{
			get
			{
				return this.annotation;
			}
			set
			{
				this.annotation=value;
			}
		}

		#endregion

		public override string ToString()
		{
			if (this.IsInputArc)
				return String.Format("{0} -> {1}",this.place,this.transition);
			else
				return String.Format("{0} -> {1}",this.transition,this.place);
		}
	}
}
