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
	public class ChargeSlip
	{
		private string number;
		private PartsArray parts;
		private ChargesArray charges;

		public string Number
		{
			get {return number;}
			set
			{
				if (value.Length != 6)
				{
					throw(new BadChargeSlipNumberException());
				}
				number=value;
			}
		}

		public int PartCount
		{
			get {return parts.Count;}
		}

		public int ChargeCount
		{
			get {return charges.Count;}
		}

		public void Add(Part p)
		{
			if (p.Number=="")
			{
				throw(new UnassignedPartException());
			}
			parts.Add(p);
		}

		public void Add(Charge c)
		{
			if (c.Description=="")
			{
				throw(new UnassignedChargeException());
			}
			charges.Add(c);
		}

		public PartsArray Parts
		{
			get {return parts;}
		}

		public ChargesArray Charges
		{
			get {return charges;}
		}

		public ChargeSlip()
		{
			parts=new PartsArray();
			charges=new ChargesArray();
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
	public class ChargeSlipArray
	{
		private ArrayList chargeSlips;

		public ChargeSlip this[int index]
		{
			get {return chargeSlips[index] as ChargeSlip;}
		}

		public int Count
		{
			get {return chargeSlips.Count;}
		}

		public ChargeSlipArray()
		{
			chargeSlips=new ArrayList();
		}

		public void Add(ChargeSlip cs)
		{
			chargeSlips.Add(cs);
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
	public class ChargeSlipHashtable
	{
		private Hashtable chargeSlips;

		public ChargeSlip this[string index]
		{
			get {return chargeSlips[index] as ChargeSlip;}
		}

		public int Count
		{
			get {return chargeSlips.Count;}
		}

		public ChargeSlipHashtable()
		{
			chargeSlips=new Hashtable();
		}

		public void Add(string csNumber, ChargeSlip cs)
		{
			chargeSlips.Add(csNumber, cs);
		}

		public bool Contains(string csNumber)
		{
			return chargeSlips.Contains(csNumber);
		}
	}
}