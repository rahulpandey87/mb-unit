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
	public class Part
	{
		private double vendorCost;
		private bool taxable;
		private double internalCost;
		private double markup;
		private string number;

		public double VendorCost
		{
			get {return vendorCost;}
			set {vendorCost=value;}
		}

		public bool Taxable
		{
			get {return taxable;}
			set {taxable=value;}
		}

		public double InternalCost
		{
			get {return internalCost;}
			set {internalCost=value;}
		}

		public double Markup
		{
			get {return markup;}
			set {markup=value;}
		}

		public string Number
		{
			get {return number;}
			set {number=value;}
		}

		public Part()
		{
			vendorCost=0;
			taxable=false;
			internalCost=0;
			markup=0;
			number="";
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
	public class PartsArray
	{
		private ArrayList parts;

		public Part this[int index]
		{
			get {return parts[index] as Part;}
		}

		public int Count
		{
			get {return parts.Count;}
		}

		public PartsArray()
		{
			parts=new ArrayList();
		}

		public void Add(Part p)
		{
			parts.Add(p);
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
	public class PartsHashtable
	{
		private Hashtable parts;

		public Part this[string index]
		{
			get {return parts[index] as Part;}
		}

		public WorkOrder this[Part p]
		{
			get {return parts[p] as WorkOrder;}
		}

		public int Count
		{
			get {return parts.Count;}
		}

		public PartsHashtable()
		{
			parts=new Hashtable();
		}

		public bool Contains(string partNumber)
		{
			return parts.Contains(partNumber);
		}

		public void Add(string partNumber, Part p)
		{
			parts.Add(partNumber, p);
		}

		public void Add(Part p, WorkOrder wo)
		{
			parts.Add(p, wo);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return parts.GetEnumerator();
		}
	}
}