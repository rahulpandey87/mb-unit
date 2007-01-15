using System;
using System.IO;

namespace QuickGraph.Representations.Petri
{
	using QuickGraph.Concepts.Petri;
	using QuickGraph.Concepts.Petri.Collections;
	using QuickGraph.Representations.Petri.Collections;

	public class Place : Vertex, IPlace
	{
		private string name;
		private TokenCollection marking = new TokenCollection();

		public Place(int id,string name)
			:base(id)
		{
			this.name=name;
		}

		#region IPlace Members

		public QuickGraph.Concepts.Petri.Collections.ITokenCollection Marking
		{
			get
			{
				return this.marking;
			}
		}

		#endregion

		#region IPetriVertex Members

		public String Name
		{
			get
			{
				return this.name;
			}
		}

		#endregion

		public string ToStringWithMarking()
		{
			StringWriter sw = new StringWriter();
			sw.WriteLine(this.ToString());
			foreach(object token in this.marking)
				sw.WriteLine("\t{0}",token.GetType().Name);

			return sw.ToString();

		}
		public override string ToString()
		{
			return String.Format("P({0}|{1})",this.name,this.marking.Count);
		}
	}
}
