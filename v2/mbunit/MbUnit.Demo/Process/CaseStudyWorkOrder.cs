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
	public class WorkOrder
	{
		private string number;
		private ChargeSlipHashtable chargeSlips;
		private ChargeSlipArray chargeSlipsArray;

		public string Number
		{
			get {return number;}
			set
			{
				if (value.Length != 6)
				{
					throw(new BadWorkOrderNumberException());
				}
				number=value;
			}
		}

		public int ChargeSlipCount
		{
			get {return chargeSlips.Count;}
		}

		public ChargeSlipArray ChargeSlips
		{
			get {return chargeSlipsArray;}
		}

		public WorkOrder()
		{
			chargeSlips=new ChargeSlipHashtable();
			chargeSlipsArray=new ChargeSlipArray();
			number="";
		}

		public void Add(ChargeSlip cs)
		{
			if (cs.Number=="")
			{
				throw(new UnassignedChargeSlipException());
			}

			if (chargeSlips.Contains(cs.Number))
			{
				throw(new DuplicateChargeSlipException());
			}
			chargeSlips.Add(cs.Number, cs);
			chargeSlipsArray.Add(cs);
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
	public class WorkOrderArray
	{
		private ArrayList workOrders;

		public WorkOrder this[int index]
		{
			get {return workOrders[index] as WorkOrder;}
		}

		public int Count
		{
			get {return workOrders.Count;}
		}

		public WorkOrderArray()
		{
			workOrders=new ArrayList();
		}

		public void Add(WorkOrder wo)
		{
			workOrders.Add(wo);
		}
	}
}
