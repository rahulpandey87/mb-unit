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
	public class Vendor
	{
		private string name;
		private PartsHashtable parts;
		private PartsArray partsArray;

		public string Name
		{
			get {return name;}
			set {name=value;}
		}

		public int PartCount
		{
			get {return parts.Count;}
		}

		public PartsArray Parts
		{
			get {return partsArray;}
		}

		public void Add(Part p)
		{
			if (p==null)
			{
				throw(new InvalidPartException());
			}

			if (p.Number=="")
			{
				throw(new UnassignedPartException());
			}

			if (parts.Contains(p.Number))
			{
				throw(new DuplicatePartException());
			}

			parts.Add(p.Number, p);
			partsArray.Add(p);
		}

		public Vendor()
		{
			parts=new PartsHashtable();
			partsArray=new PartsArray();
			name="";
		}

		public bool Find(Part p)
		{
			return parts.Contains(p.Number);
		}
	}
}
