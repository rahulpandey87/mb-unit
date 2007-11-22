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
	[ProcessTestFixture]
	public class POSequenceTest
	{
		private PurchaseOrder po;
		private Vendor vendor;
		private Part part1;
		private Part part2;
		private Part part3;
		private WorkOrder wo1;
		private WorkOrder wo2;
		private Invoice invoice;
		private Charge charge;

		[TestSequence(1)]
		public void POConstructor()
		{
			po=new PurchaseOrder();
			Assert.AreEqual(po.Number,"", "Number not initialized.");
			Assert.AreEqual(po.PartCount,0, "PartCount not initialized.");
			Assert.AreEqual(po.ChargeCount,0, "ChargeCount not initialized.");
			Assert.AreEqual(po.Invoice,null, "Invoice not initialized.");
			Assert.AreEqual(po.Vendor,null, "Vendor not initialized.");
		}

		[TestSequence(2)]
		public void VendorConstructor()
		{
			vendor=new Vendor();
			Assert.AreEqual(vendor.Name,"", "Name is not an empty string.");
			Assert.AreEqual(vendor.PartCount,0, "PartCount is not zero.");
		}

		[TestSequence(3)]
		public void PartConstructor()
		{
			part1=new Part();
			Assert.AreEqual(part1.VendorCost,0, "VendorCost is not zero.");
			Assert.AreEqual(part1.Taxable,false, "Taxable is not false.");
			Assert.AreEqual(part1.InternalCost,0, "InternalCost is not zero.");
			Assert.AreEqual(part1.Markup,0, "Markup is not zero.");
			Assert.AreEqual(part1.Number,"", "Number is not an empty string.");

			part2=new Part();
			part3=new Part();
		}

		[TestSequence(4)]
		public void PartInitialization()
		{
			part1.Number="A";
			part1.VendorCost=15;
			Assert.AreEqual(part1.Number,"A", "Number did not get set.");
			Assert.AreEqual(part1.VendorCost,15, "VendorCost did not get set.");

			part2.Number="B";
			part2.VendorCost=20;

			part3.Number="C";
			part3.VendorCost=25;
		}

		[TestSequence(5)]
		public void AddVendorParts()
		{
			vendor.Add(part1);
			vendor.Add(part2);
			vendor.Add(part3);

			Assert.AreEqual(vendor.Parts[0].Number,"A", "PartNumber is wrong.");
			Assert.AreEqual(vendor.Parts[1].Number,"B", "PartNumber is wrong.");
			Assert.AreEqual(vendor.Parts[2].Number,"C", "PartNumber is wrong.");
		}

		[TestSequence(6)]
		public void WorkOrderConstructor()
		{
			wo1=new WorkOrder();
			Assert.AreEqual(wo1.Number,"", "Number not initialized.");
			Assert.AreEqual(wo1.ChargeSlipCount,0, "ChargeSlipCount not initialized.");

			wo2=new WorkOrder();
		}

		[TestSequence(7)]
		public void WorkOrderInitialization()
		{
			wo1.Number="000001";
			wo2.Number="000002";

			Assert.AreEqual(wo1.Number,"000001", "Number not set.");
			Assert.AreEqual(wo2.Number,"000002", "Number not set.");
		}

		[TestSequence(8)]
		public void AssignVendorToPO()
		{
			po.Vendor=vendor;
			Assert.AreEqual(po.Vendor,vendor, "Vendor not set.");
		}

		[TestSequence(9)]
		public void AddPartsToPO()
		{
			po.Add(part1, wo1);
			po.Add(part2, wo1);
			po.Add(part3, wo2);

			WorkOrder _wo2;
			Part p2;
			po.GetPart(0, out p2, out _wo2);
			Assert.AreEqual(p2.Number,part1.Number, "Part number does not match.");
			Assert.AreEqual(_wo2.Number,wo1.Number, "Work order number does not match.");
		}

		[TestSequence(10)]
		public void InvoiceConstructor()
		{
			invoice=new Invoice();
			Assert.AreEqual(invoice.Number,"", "Number not initialized.");
			Assert.AreEqual(invoice.ChargeCount,0, "ChargeCount not initialized.");
			Assert.AreEqual(invoice.Vendor,null, "Vendor not initialized.");
		}

		[TestSequence(11)]
		public void InvoiceInitialization()
		{
			invoice.Number="112233";
			Assert.AreEqual(invoice.Number,"112233", "Number not set.");

			invoice.Vendor=vendor;
			Assert.AreEqual(invoice.Vendor.Name,vendor.Name, "Vendor name not set.");
		}

		[TestSequence(12)]
		public void ChargeConstructor()
		{
			charge=new Charge();
			Assert.AreEqual(charge.Description,"", "Description is not an empty string.");
			Assert.AreEqual(charge.Amount,0, "Amount is not zero.");
		}

		[TestSequence(13)]
		public void ChargeInitialization()
		{
			charge.Description="Freight";
			charge.Amount=10.50;

			Assert.AreEqual(charge.Description,"Freight", "Description is not set.");
			Assert.AreEqual(charge.Amount,10.50, "Amount is not set correctly.");
		}

		[TestSequence(14)]
		public void AddChargeToInvoice()
		{
			invoice.Add(charge);
			Assert.AreEqual(invoice.ChargeCount,1, "Charge count wrong.");
			Assert.AreEqual(charge.Description,invoice.Charges[0].Description, "Charge description does not match.");
		}

		[TestSequence(15)]
		public void AddInvoiceToPO()
		{
			po.Invoice=invoice;
			Assert.AreEqual(invoice.Number,po.Invoice.Number, "Invoice not set correctly.");
		}

		[TestSequence(16)]
		public void ClosePO()
		{
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
