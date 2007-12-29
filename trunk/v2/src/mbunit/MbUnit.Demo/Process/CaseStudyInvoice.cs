using System;

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
	public class Invoice
	{
		private string number;
		private Vendor vendor;
		private ChargesArray charges;

		public string Number
		{
			get {return number;}
			set {number=value;}
		}

		public Vendor Vendor
		{
			get {return vendor;}
			set
			{
				if (value==null)
				{
					throw(new InvalidVendorException());
				}
				vendor=value;
			}
		}

		public int ChargeCount
		{
			get {return charges.Count;}
		}

		public ChargesArray Charges
		{
			get {return charges;}
		}

		public Invoice()
		{
			charges=new ChargesArray();
			number="";
			vendor=null;
		}

		public void Add(Charge c)
		{
			if (c==null)
			{
				throw(new InvalidChargeException());
			}
			if (c.Description=="")
			{
				throw(new UnassignedChargeException());
			}
			charges.Add(c);
		}
	}
}
