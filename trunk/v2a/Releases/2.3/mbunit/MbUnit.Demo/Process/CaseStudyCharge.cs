// The source from this example was totally cut and pasted from
// Marc Clifton articles on CodeProject:
// Original source:
// http://www.codeproject.com/csharp/autp3.asp
// 

using System;
using System.Collections;

namespace MbUnit.Demo.Process
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>
	/// The source of this class was copied from Marc Clifton articles on
	/// <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a>.
	/// </para>
	/// </remarks>
	public class Charge
	{
		private string description;
		private double amount;

		public string Description
		{
			get {return description;}
			set {description=value;}
		}

		public double Amount
		{
			get {return amount;}
			set {amount=value;}
		}

		public Charge()
		{
			description="";
			amount=0;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>
	/// The source of this class was copied from Marc Clifton articles on
	/// <a href="http://www.codeproject.com/csharp/autp3.asp">CodeProject</a>.
	/// </para>
	/// </remarks>
	public class ChargesArray
	{
		private ArrayList charges;

		public Charge this[int index]
		{
			get {return charges[index] as Charge;}
		}

		public int Count
		{
			get {return charges.Count;}
		}

		public ChargesArray()
		{
			charges=new ArrayList();
		}

		public void Add(Charge c)
		{
			charges.Add(c);
		}

		public IEnumerator GetEnumerator()
		{
			return charges.GetEnumerator();
		}
	}
/*
	public class ChargesHashtable
	{
		private Hashtable charges;

		public Charge this[int index]
		{
			get {return charges[index] as Charge;}
		}

		public int Count
		{
			get {return charges.Count;}
		}

		public ChargesHashtable()
		{
			charges=new Hashtable();
		}
	}
*/	
}
