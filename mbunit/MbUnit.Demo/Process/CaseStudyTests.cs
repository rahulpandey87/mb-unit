using System;

using MbUnit.Core.Framework;
using MbUnit.Framework;

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
	[TestFixture]
	public class PartTest
	{
		[Test]
		public void ConstructorInitialization()
		{
			Part part=new Part();
			Assert.AreEqual(part.VendorCost,0, "VendorCost is not zero.");
			Assert.AreEqual(part.Taxable,false, "Taxable is not false.");
			Assert.AreEqual(part.InternalCost,0, "InternalCost is not zero.");
			Assert.AreEqual(part.Markup,0, "Markup is not zero.");
			Assert.AreEqual(part.Number,"", "Number is not an empty string.");
		}

		[Test]
		public void SetVendorInfo()
		{
			Part part=new Part();
			part.Number="FIG 4RAC #R11T";
			part.VendorCost=12.50;
			part.Taxable=true;
			part.InternalCost=13.00;
			part.Markup=2.0;
		
			Assert.AreEqual(part.Number,"FIG 4RAC #R11T", "Number did not get set.");
			Assert.AreEqual(part.VendorCost,12.50, "VendorCost did not get set.");
			Assert.AreEqual(part.Taxable,true, "Taxable did not get set.");
			Assert.AreEqual(part.InternalCost,13.00, "InternalCost did not get set.");
			Assert.AreEqual(part.Markup,2.0, "Markup did not get set.");
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
	[TestFixture]
	public class VendorTest
	{
		private Vendor vendor;

		[SetUp]
		public void VendorSetUp()
		{
			vendor=new Vendor();
		}

		[Test]
		public void ConstructorInitialization()
		{
			Assert.AreEqual(vendor.Name,"", "Name is not an empty string.");
			Assert.AreEqual(vendor.PartCount,0, "PartCount is not zero.");
		}

		[Test]
		public void VendorName()
		{
			vendor.Name="Jamestown Distributors";
			Assert.AreEqual(vendor.Name,"Jamestown Distributors", "Name did not get set.");
			}

		[Test]
		public void AddUniqueParts()
		{
			CreateTestParts();
			Assert.AreEqual(vendor.PartCount,2, "PartCount is not 2.");
		}

		[Test]
		public void RetrieveParts()
		{
			CreateTestParts();
			Part part;
			part=vendor.Parts[0];
			Assert.AreEqual(part.Number,"BOD-13-25P", "PartNumber is wrong.");
			part=vendor.Parts[1];
			Assert.AreEqual(part.Number,"BOD-13-33P", "PartNumber is wrong.");
		}

		[Test, ExpectedException(typeof(DuplicatePartException))]
		public void DuplicateParts()
		{
			Part part=new Part();
			part.Number="Same Part Number";
			vendor.Add(part);
			vendor.Add(part);
		}

		[Test, ExpectedException(typeof(UnassignedPartException))]
		public void UnassignedPartNumber()
		{
			Part part=new Part();
			vendor.Add(part);
		}

		void CreateTestParts()
		{
			Part part1=new Part();
			part1.Number="BOD-13-25P";
			vendor.Add(part1);
		
			Part part2=new Part();
			part2.Number="BOD-13-33P";
			vendor.Add(part2);
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
	[TestFixture]
	public class ChargeTest
	{
		[Test]
		public void ConstructorInitialization()
		{
			Charge charge=new Charge();
			Assert.AreEqual(charge.Description,"", "Description is not an empty string.");
			Assert.AreEqual(charge.Amount,0, "Amount is not zero.");
		}
	
		[Test]
		public void SetChargeInfo()
		{
			Charge charge=new Charge();
			charge.Description="Freight";
			charge.Amount=8.50;
		
			Assert.AreEqual(charge.Description,"Freight", "Description is not set.");
			Assert.AreEqual(charge.Amount,8.50, "Amount is not set correctly.");
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
	[TestFixture]
	public class ChargeSlipTest
	{
		private ChargeSlip chargeSlip;

		[SetUp]
		public void SetUp()
		{
			chargeSlip=new ChargeSlip();
		}

		[Test]
		public void ConstructorInitialization()
		{
			Assert.AreEqual(chargeSlip.Number,"", "Number is not initialized correctly.");
			Assert.AreEqual(chargeSlip.PartCount,0, "PartCount is not zero.");
			Assert.AreEqual(chargeSlip.ChargeCount,0, "ChargeCount is not zero.");
		}

		[Test]
		public void ChargeSlipNumberAssignment()
		{
			chargeSlip.Number="123456";
			Assert.AreEqual(chargeSlip.Number,"123456", "Number is not set correctly.");
			}

		[Test, ExpectedException(typeof(BadChargeSlipNumberException))]
		public void BadChargeSlipNumber()
		{
			chargeSlip.Number="12345";			// must be six digits or letters
		}

		[Test]
		public void AddPart()
		{
			Part part=new Part();
			part.Number="VOD-13-33P";
			chargeSlip.Add(part);
			Assert.AreEqual(chargeSlip.PartCount,1, "PartCount is wrong.");
		}

		[Test]
		public void AddCharge()
		{
			Charge charge=new Charge();
			charge.Description="Freight";
			charge.Amount=10.50;		
			chargeSlip.Add(charge);
			Assert.AreEqual(chargeSlip.ChargeCount,1, "ChargeCount is wrong.");
		}

		[Test]
		public void RetrievePart()
		{
			Part part=new Part();
			part.Number="VOD-13-33P";
			chargeSlip.Add(part);
			Part p2=chargeSlip.Parts[0];
			Assert.AreEqual(p2.Number,part.Number, "Part numbers do not match.");
		}

		[Test]
		public void RetrieveCharge()
		{
			Charge charge=new Charge();
			charge.Description="Freight";
			charge.Amount=10.50;		
			chargeSlip.Add(charge);
			Charge c2=chargeSlip.Charges[0];
			Assert.AreEqual(c2.Description,charge.Description, "Descriptions do not match.");
		}

		[Test, ExpectedException(typeof(UnassignedPartException))]
		public void AddUnassignedPart()
		{
			Part part=new Part();
			chargeSlip.Add(part);
		}

		[Test, ExpectedException(typeof(UnassignedChargeException))]
		public void UnassignedCharge()
		{
			Charge charge=new Charge();
			chargeSlip.Add(charge);
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
	[TestFixture]
	public class WorkOrderTest
	{
		private WorkOrder workOrder;

		[SetUp]
		public void WorkOrderSetUp()
		{
			workOrder=new WorkOrder();
		}

		[Test]
		public void ConstructorInitialization()
		{
			Assert.AreEqual(workOrder.Number,"", "Number not initialized.");
			Assert.AreEqual(workOrder.ChargeSlipCount,0, "ChargeSlipCount not initialized.");
		}

		[Test]
		public void WorkOrderNumber()
		{
			workOrder.Number="112233";
			Assert.AreEqual(workOrder.Number,"112233", "Number not set.");
			}

		[Test, ExpectedException(typeof(BadWorkOrderNumberException))]
		public void BadWorkOrderNumber()
		{
			workOrder.Number="12345";
		}

		[Test]
		public void AddChargeSlip()
		{
			ChargeSlip chargeSlip=new ChargeSlip();
			chargeSlip.Number="123456";
			workOrder.Add(chargeSlip);
			Assert.AreEqual(workOrder.ChargeSlipCount,1, "ChargeSlip not added.");
		}

		[Test]
		public void RetrieveChargeSlip()
		{
			ChargeSlip chargeSlip=new ChargeSlip();
			chargeSlip.Number="123456";
			workOrder.Add(chargeSlip);
			ChargeSlip cs2=workOrder.ChargeSlips[0];
			Assert.AreEqual(chargeSlip.Number,cs2.Number, "ChargeSlip numbers do not match.");
		}

		[Test, ExpectedException(typeof(DuplicateChargeSlipException))]
		public void DuplicateChargeSlip()
		{
			ChargeSlip chargeSlip=new ChargeSlip();
			chargeSlip.Number="123456";
			workOrder.Add(chargeSlip);
			workOrder.Add(chargeSlip);
		}

		[Test, ExpectedException(typeof(UnassignedChargeSlipException))]
		public void UnassignedChargeSlipNumber()
		{
			ChargeSlip chargeSlip=new ChargeSlip();
			workOrder.Add(chargeSlip);
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
	[TestFixture]
	public class InvoiceTest
	{
		private Invoice invoice;

		[SetUp]
		public void InvoiceSetUp()
		{
			invoice=new Invoice();
		}

		[Test]
		public void ConstructorInitialization()
		{
			Assert.AreEqual(invoice.Number,"", "Number not initialized.");
			Assert.AreEqual(invoice.ChargeCount,0, "ChargeCount not initialized.");
			Assert.AreEqual(invoice.Vendor,null, "Vendor not initialized.");
		}

		[Test]
		public void InvoiceNumber()
		{
			invoice.Number="112233";
			Assert.AreEqual(invoice.Number,"112233", "Number not set.");
		}

		[Test]
		public void InvoiceVendor()
		{
			Vendor vendor=new Vendor();
			vendor.Name="Nantucket Parts";
			invoice.Vendor=vendor;
			Assert.AreEqual(invoice.Vendor.Name,vendor.Name, "Vendor name not set.");
		}

		[Test]
		public void AddCharge()
		{
			Charge charge=new Charge();
			charge.Description="Freight";
			invoice.Add(charge);
			Assert.AreEqual(invoice.ChargeCount,1, "Charge count wrong.");
		}

		[Test]
		public void RetrieveCharge()
		{
			Charge charge=new Charge();
			charge.Description="123456";
			invoice.Add(charge);
			Charge c2=invoice.Charges[0];
			Assert.AreEqual(charge.Description,c2.Description, "Charge description does not match.");
		}

		[Test, ExpectedException(typeof(UnassignedChargeException))]
		public void UnassignedChargeNumber()
		{
			Charge charge=new Charge();
			invoice.Add(charge);
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
	[TestFixture]
	public class CustomerTest
	{
		private Customer customer;

		[SetUp]
		public void CustomerSetUp()
		{
			customer=new Customer();
		}

		[Test]
		public void ConstructorInitialization()
		{
			Assert.AreEqual(customer.Name,"", "Name not initialized.");
			Assert.AreEqual(customer.WorkOrderCount,0, "WorkOrderCount not initialized.");
		}

		[Test]
		public void CustomerName()
		{
			customer.Name="Marc Clifton";
			Assert.AreEqual(customer.Name,"Marc Clifton", "Name not set.");
		}

		[Test]
		public void AddWorkOrder()
		{
			WorkOrder workOrder=new WorkOrder();
			workOrder.Number="123456";
			customer.Add(workOrder);
			Assert.AreEqual(customer.WorkOrderCount,1, "Work order not added.");
		}

		[Test]
		public void RetrieveWorkOrder()
		{
			WorkOrder workOrder=new WorkOrder();
			workOrder.Number="123456";
			customer.Add(workOrder);
			WorkOrder wo2=customer.WorkOrders[0];
			Assert.AreEqual(workOrder.Number,wo2.Number, "WorkOrder numbers do not match.");
		}

		[Test, ExpectedException(typeof(UnassignedWorkOrderException))]
		public void UnassignedWorkOrderNumber()
		{
			WorkOrder workOrder=new WorkOrder();
			customer.Add(workOrder);
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
	[TestFixture]
	public class PurchaseOrderTest
	{
		private PurchaseOrder po;
		private Vendor vendor;

		[SetUp]
		public void PurchaseOrderSetUp()
		{
			po=new PurchaseOrder();
			vendor=new Vendor();
			vendor.Name="West Marine";
			po.Vendor=vendor;
		}

		[Test]
		public void ConstructorInitialization()
		{
			PurchaseOrder po=new PurchaseOrder();
			Assert.AreEqual(po.Number,"", "Number not initialized.");
			Assert.AreEqual(po.PartCount,0, "PartCount not initialized.");
			Assert.AreEqual(po.ChargeCount,0, "ChargeCount not initialized.");
			Assert.AreEqual(po.Invoice,null, "Invoice not initialized.");
			Assert.AreEqual(po.Vendor,null, "Vendor not initialized.");
		}

		[Test]
		public void PONumber()
		{
			po.Number="123456";
			Assert.AreEqual(po.Number,"123456", "Number not set.");
		}

		[Test]
		public void AddPart()
		{
			WorkOrder workOrder=new WorkOrder();
			workOrder.Number="123456";
			Part part=new Part();
			part.Number="112233";
			vendor.Add(part);
			po.Add(part, workOrder);
			WorkOrder wo2;
			Part p2;
			po.GetPart(0, out p2, out wo2);
			Assert.AreEqual(p2.Number,part.Number, "Part number does not match.");
			Assert.AreEqual(wo2.Number,workOrder.Number, "Work order number does not match.");
		}

		[Test, ExpectedException(typeof(PartNotFromVendorException))]
		public void AddPartNotFromVendor()
		{
			WorkOrder workOrder=new WorkOrder();
			workOrder.Number="123456";
			Part part=new Part();
			part.Number="131133";
			po.Add(part, workOrder);
		}

		[Test, ExpectedException(typeof(DifferentVendorException))]
		public void AddInvoiceFromDifferentVendor()
		{
			Vendor vendor1=new Vendor();
			vendor1.Name="ABC Co.";
			po.Vendor=vendor1;
			Invoice invoice=new Invoice();
			invoice.Number="123456";
			Vendor vendor2=new Vendor();
			vendor2.Name="XYZ Inc.";
			invoice.Vendor=vendor2;
			po.Invoice=invoice;
		}

		[Test, ExpectedException(typeof(UnassignedWorkOrderException))]
		public void UnassignedWorkOrderNumber()
		{
			WorkOrder workOrder=new WorkOrder();
			Part part=new Part();
			part.Number="112233";
			po.Add(part, workOrder);
		}

		[Test, ExpectedException(typeof(UnassignedPartException))]
		public void UnassignedPartNumber()
		{
			WorkOrder workOrder=new WorkOrder();
			workOrder.Number="123456";
			Part part=new Part();
			po.Add(part, workOrder);
		}

		[Test, ExpectedException(typeof(UnassignedInvoiceException))]
		public void UnassignedInvoiceNumber()
		{
			Invoice invoice=new Invoice();
			po.Invoice=invoice;
		}

		[Test]
		public void ClosePO()
		{
			WorkOrder wo1=new WorkOrder();
			WorkOrder wo2=new WorkOrder();

			wo1.Number="000001";
			wo2.Number="000002";

			Part p1=new Part();
			Part p2=new Part();
			Part p3=new Part();

			p1.Number="A";
			p1.VendorCost=15;

			p2.Number="B";
			p2.VendorCost=20;

			p3.Number="C";
			p3.VendorCost=25;

			vendor.Add(p1);
			vendor.Add(p2);
			vendor.Add(p3);

			po.Add(p1, wo1);
			po.Add(p2, wo1);
			po.Add(p3, wo2);

			Charge charge=new Charge();
			charge.Description="Freight";
			charge.Amount=10.50;

			Invoice invoice=new Invoice();
			invoice.Number="1234";
			invoice.Vendor=vendor;
			invoice.Add(charge);

			po.Invoice=invoice;

			po.Close();

			// one charge slip should be added to both work orders
			Assert.AreEqual(wo1.ChargeSlipCount,1, "First work order: ChargeSlipCount not 1.");
			Assert.AreEqual(wo2.ChargeSlipCount,1, "Second work order: ChargeSlipCount not 1.");

			ChargeSlip cs1=wo1.ChargeSlips[0];
			ChargeSlip cs2=wo2.ChargeSlips[0];

			// three charges should exist for charge slip #1: two parts and one freight charge
			Assert.AreEqual(cs1.PartCount + cs1.ChargeCount,3, "Charge slip 1: doesn't have three charges.");

			// the freight for CS1 should be 10.50 * (15+20)/(15+20+25) = 6.125
			Assert.AreEqual(cs1.Charges[0].Amount,6.125, "Charge slip 1: charge not the correct amount.");

			// two charges should exist for charge slip #2: one part and one freight charge
			Assert.AreEqual(cs2.PartCount + cs2.ChargeCount,2, "Charge slip 2: doesn't have two charges.");

			// the freight for CS2 should be 10.50 * 25/(15+20+25) = 4.375  (also = 10.50-6.125)
			Assert.AreEqual(cs2.Charges[0].Amount,4.375, "Charge slip 2: charge not the correct amount.");

			// while we have a unit test that verifies that parts are added to charge slips correctly,
			// we don't have a unit test to verify that the purchase order Close process does this
			// correctly.

			Part cs1p1=cs1.Parts[0];
			Part cs1p2=cs1.Parts[1];
			if (cs1p1.Number=="A")
			{
				Assert.AreEqual(cs1p1.VendorCost,15, "Charge slip 1, vendor cost not correct for part A.");
			}
			else if (cs1p1.Number=="B")
			{
				Assert.AreEqual(cs1p1.VendorCost,20, "Charge slip 1, vendor cost not correct for part B.");
			}
			else
			{
				throw(new IncorrectChargeSlipException());
			}

			Assert.IsTrue(cs1p1.Number != cs1p2.Number, "Charge slip part numbers are not unique.");

			if (cs1p2.Number=="A")
			{
				Assert.AreEqual(cs1p2.VendorCost,15, "Charge slip 1, vendor cost is not correct for part A.");
			}
			else if (cs1p2.Number=="B")
			{
				Assert.AreEqual(cs1p2.VendorCost,20, "Charge slip 1, vendor cost is not correct for part B.");
			}
			else
			{
				throw(new IncorrectChargeSlipException());
			}

			Assert.AreEqual(cs2.Parts[0].Number,"C", "Charge slip 2, part number is not correct.");
			Assert.AreEqual(cs2.Parts[0].VendorCost,25, "Charge slip 2, vendor cost is not correct for part C.");		
		}
	}
}

