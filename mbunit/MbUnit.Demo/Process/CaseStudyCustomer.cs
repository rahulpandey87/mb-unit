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
	public class Customer
	{
		private string name;
		private WorkOrderArray workOrders;

		public string Name
		{
			get {return name;}
			set {name=value;}
		}

		public int WorkOrderCount
		{
			get {return workOrders.Count;}
		}

		public WorkOrderArray WorkOrders
		{
			get {return workOrders;}
		}

		public Customer()
		{
			workOrders=new WorkOrderArray();
			name="";
		}

		public void Add(WorkOrder wo)
		{
			if (wo.Number=="")
			{
				throw(new UnassignedWorkOrderException());
			}
			workOrders.Add(wo);
		}
	}
}
