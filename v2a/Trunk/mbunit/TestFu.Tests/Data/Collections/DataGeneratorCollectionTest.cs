using System;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Data;
using TestFu.Data.Generators;

namespace TestFu.Tests.Data.Collections
{
	using TestFu.Tests.Data.Generators;

	[TypeFixture(typeof(IDataGeneratorCollection))]
	[ProviderFactory(typeof(DataGeneratorCollectionFactory),typeof(IDataGeneratorCollection))]
	public class DataGeneratorCollectionTest
	{
		private AllDataGeneratorFactory factory=new AllDataGeneratorFactory();

		#region Argument Checking
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddNull(IDataGeneratorCollection col)
		{
			col.Add(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveDataGeneratorNull(IDataGeneratorCollection col)
		{
			IDataGenerator dg = null;
			col.Remove(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveColumnNull(IDataGeneratorCollection col)
		{
			DataColumn dg = null;
			col.Remove(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveColumnNameNull(IDataGeneratorCollection col)
		{
			String dg = null;
			col.Remove(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsDataGeneratorNull(IDataGeneratorCollection col)
		{
			IDataGenerator dg = null;
			col.Contains(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsColumnNull(IDataGeneratorCollection col)
		{
			DataColumn dg = null;
			col.Contains(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ContainsColumnNameNull(IDataGeneratorCollection col)
		{
			string dg = null;
			col.Contains(dg);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetItemColumnNull(IDataGeneratorCollection col)
		{
			DataColumn column = null;
			IDataGenerator dg = col[column];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetItemStringNull(IDataGeneratorCollection col)
		{
			string n=null;
			IDataGenerator dg = col[n];
		}
		#endregion

		#region Add and check is in collection
		[Test]
		public void AddAndCheckByDataGenerator(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			Assert.IsTrue(col.Contains(factory.Binary));
			Assert.AreEqual(count+1,col.Count);
		}

		[Test]
		public void AddAndCheckByColumn(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			Assert.IsTrue(col.Contains(factory.Binary.Column));
			Assert.AreEqual(count+1,col.Count);
		}

		[Test]
		public void AddAndCheckByColumnName(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			Assert.IsTrue(col.Contains(factory.Binary.Column.ColumnName));
			Assert.AreEqual(count+1,col.Count);
		}
		#endregion

		#region Add and remove
		[Test]
		public void AddAndRemoveByDataGenerator(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			col.Remove(factory.Binary);
			Assert.IsTrue(!col.Contains(factory.Binary));
			Assert.AreEqual(count,col.Count);
		}

		[Test]
		public void AddAndRemoveByColumn(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			col.Remove(factory.Binary.Column);
			Assert.IsTrue(!col.Contains(factory.Binary));
			Assert.AreEqual(count,col.Count);
		}

		[Test]
		public void AddAndRemoveByColumnName(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			col.Remove(factory.Binary.Column.ColumnName);
			Assert.IsTrue(!col.Contains(factory.Binary));
			Assert.AreEqual(count,col.Count);
		}
		#endregion

		#region Clear
		[Test]
		public void Clear(IDataGeneratorCollection col)
		{
			col.Clear();
			Assert.AreEqual(0,col.Count);
		}
		#endregion

		#region Item
		[Test]
		public void AddAndGetItemColumn(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			Assert.AreEqual(factory.Binary,col[factory.Binary.Column]);
		}
		[Test]
		public void AddAndGetItemColumnName(IDataGeneratorCollection col)
		{
			int count=col.Count;
			col.Add(factory.Binary);
			Assert.AreEqual(factory.Binary,col[factory.Binary.Column.ColumnName]);
		}
		#endregion

		#region Duplicates
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void AddDuplicate(IDataGeneratorCollection col)
		{
			col.Add(factory.Binary);
			col.Add(factory.Binary);
		}
		#endregion
	}
}
