using System;
using System.Collections;
using System.Globalization;

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
	public class PurchaseOrder
	{
		private string number;
		private Vendor vendor;
		private Invoice invoice;
		private PartsHashtable parts;
		private PartsArray partsArray;
		private ChargesArray charges;

		public string Number
		{
			get {return number;}
			set {number=value;}
		}

		public Invoice Invoice
		{
			get {return invoice;}
			set
			{
				if (value==null)
				{
					throw(new InvalidInvoiceException());
				}
				else
				if (value.Number=="")
				{
					throw(new UnassignedInvoiceException());
				}
				// *** NO VENDOR TEST !!! ***
				if (value.Vendor.Name != vendor.Name)
				{
					throw(new DifferentVendorException());
				}
				invoice=value;
			}
		}

		public Vendor Vendor
		{
			get {return vendor;}
			set {vendor=value;}
		}

		public int PartCount
		{
			get {return parts.Count;}
		}

		public int ChargeCount
		{
			get {return charges.Count;}
		}

		public PurchaseOrder()
		{
			parts=new PartsHashtable();
			partsArray=new PartsArray();
			charges=new ChargesArray();
			number="";
			vendor=null;
			invoice=null;
		}

		public void Add(Part p, WorkOrder wo)
		{
			if (p==null)
			{
				throw(new InvalidPartException());
			}

			if (wo==null)
			{
				throw(new InvalidWorkOrderException());
			}

			if (p.Number=="")
			{
				throw(new UnassignedPartException());
			}

			if (wo.Number=="")
			{
				throw(new UnassignedWorkOrderException());
			}

			if (!vendor.Find(p))
			{
				throw(new PartNotFromVendorException());
			}

			parts.Add(p, wo);
			partsArray.Add(p);
		}

		private void Add(Charge c)
		{
			charges.Add(c);
		}

		public void GetPart(int index, out Part p, out WorkOrder wo)
		{
			// *** NO BOUNDARY TESTING !!! ***
			p=partsArray[index];
			wo=parts[p] as WorkOrder;
		}

		public void Close()
		{
			if (invoice==null)
			{
				throw(new InvalidInvoiceException());
			}
			// add invoice charges to po charges
			foreach(Charge c in invoice.Charges)
			{
				Add(c);
			}

			// Collect all the different work orders the parts go to.
			// For each work order, create a charge slip
			Hashtable woList=new Hashtable();
			int n=1;		// we always start with charge slip #000001
			string nStr="000000";
			double totalPartCost=0;
			foreach (DictionaryEntry item in parts)
			{
				if (!woList.Contains(item.Value))
				{
					ChargeSlip cs=new ChargeSlip();
					string s=n.ToString();
					cs.Number=nStr.Substring(0, 6-s.Length)+s; 
					woList[item.Value]=cs;

					// add the new charge slip to the work order
					(item.Value as WorkOrder).Add(cs);
				}
				
				// For each charge slip, add the part to
				// the charge slip.
				ChargeSlip cs2=woList[item.Value] as ChargeSlip;
				cs2.Add(item.Key as Part);
				totalPartCost+=(item.Key as Part).VendorCost;
			}

			// For each work order, get the total parts amount on
			// its corresponding charge slip.
			foreach (DictionaryEntry item in woList)
			{
				ChargeSlip cs=item.Value as ChargeSlip;
				double csPartCost=0;
				for (int i=0; i<cs.PartCount; i++)
				{
					csPartCost+=cs.Parts[i].VendorCost;
				}

				// The charge amount added to the charge slip =
				// csPartCost * chargeAmt / totalPartCost
				for (int i=0; i<charges.Count; i++)
				{
					Charge charge=new Charge();
					charge.Amount=csPartCost * charges[i].Amount / totalPartCost;
					charge.Description=charges[i].Description;
					cs.Add(charge);
				}
			}
		}
	}
}
