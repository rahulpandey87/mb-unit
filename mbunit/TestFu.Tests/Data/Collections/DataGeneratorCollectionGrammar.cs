using System;
using System.Collections;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Grammars;
using TestFu.Data;
using TestFu.Data.Collections;

namespace TestFu.Tests.Data.Collections
{
	using TestFu.Tests.Data.Generators;

	[GrammarFixture]
	[FixtureCategory("Current")]
	public class DataGeneratorCollectionGrammar : Grammar
	{
		private IDataGeneratorCollection col=new DataGeneratorCollection();
		private AllDataGeneratorFactory factory = new AllDataGeneratorFactory();
		private ArrayList oracle = new ArrayList();

		private IRule add;
		private IRule remove;
		private IRule removeColumn;
		private IRule removeColumnName;
		private IRule clear;

		private IRule guardRemove;
		private IRule guardRemoveColumn;
		private IRule guardRemoveColumnName;

		private IRule nonEmpty;
		private IRule empty;
		
		private IRule modify;

		public DataGeneratorCollectionGrammar()
		{
			// methods
			this.add = Rules.Method(new MethodInvoker(this.Add));
			this.remove = Rules.Method(new MethodInvoker(this.Remove));
			this.removeColumn = Rules.Method(new MethodInvoker(this.RemoveColumn));
			this.removeColumnName = Rules.Method(new MethodInvoker(this.RemoveColumnName));
			this.clear = Rules.Method(new MethodInvoker(this.Clear));

			// guarded methods
			this.guardRemove = Rules.Guard( this.remove, typeof(InvalidOperationException) ); 
			this.guardRemoveColumn = Rules.Guard( this.removeColumn, typeof(InvalidOperationException) ); 
			this.guardRemoveColumnName = Rules.Guard( this.removeColumnName, typeof(InvalidOperationException) ); 

			// high order rules
			this.nonEmpty = Rules.Alt( 
				this.add,
				this.clear, 
				this.remove, 
				this.removeColumn, 
				this.removeColumnName
				);
			this.empty = Rules.Alt( 
				this.add,
				this.clear, 
				this.guardRemove, 
				this.guardRemoveColumn, 
				this.guardRemoveColumnName
				);
 
			this.modify = Rules.If(new ConditionDelegate(this.IsEmpty), empty, nonEmpty);
			this.StartRule = Rules.Kleene(this.modify);
		}

		#region Rules
		public bool IsEmpty(IProductionToken token)
		{
			return this.col.Count==0;
		}

		public void Add()
		{
			IDataGenerator gen = this.factory.GetRandomGenerator();
			if (col.Contains(gen))
			{
				try
				{
					Console.WriteLine("Add({0}) - duplicate",gen);
					col.Add(gen);
				}
				catch(ArgumentException)
				{}
			}
			else
			{
				Console.WriteLine("Add({0})",gen);
				int count = col.Count;
				col.Add(gen);
				oracle.Add(gen);
				Assert.AreEqual(count+1,col.Count);
			}
			this.Check();
		}

		public void Remove()
		{
			IDataGenerator gen = this.factory.GetRandomGenerator();
			Console.WriteLine("Remove({0})",gen);
			int count = col.Count;
			if (col.Contains(gen))
				count--;
			col.Remove(gen);
			oracle.Remove(gen);
			Assert.AreEqual(count,col.Count);
			Assert.IsFalse(col.Contains(gen));
			Assert.IsFalse(col.Contains(gen.Column));
			Assert.IsFalse(col.Contains(gen.Column.ColumnName));

			this.Check();
		}

		public void RemoveColumn()
		{
			IDataGenerator gen = this.factory.GetRandomGenerator();
			Console.WriteLine("RemoveColumn({0})",gen);
			int count = col.Count;
			if (col.Contains(gen))
				count--;
			col.Remove(gen.Column);
			oracle.Remove(gen);
			Assert.AreEqual(count,col.Count);
			Assert.IsFalse(col.Contains(gen));
			Assert.IsFalse(col.Contains(gen.Column));
			Assert.IsFalse(col.Contains(gen.Column.ColumnName));

			this.Check();
		}


		public void RemoveColumnName()
		{
			IDataGenerator gen = this.factory.GetRandomGenerator();
			Console.WriteLine("RemoveColumnName({0})",gen);
			int count = col.Count;
			if (col.Contains(gen))
				count--;
			col.Remove(gen.Column.ColumnName);
			oracle.Remove(gen);
			Assert.AreEqual(count,col.Count);
			Assert.IsFalse(col.Contains(gen));
			Assert.IsFalse(col.Contains(gen.Column));
			Assert.IsFalse(col.Contains(gen.Column.ColumnName));

			this.Check();
		}

		public void Clear()
		{
			Console.WriteLine("Clear()");
			col.Clear();
			oracle.Clear();
			this.Check();
		}
		public void Check()
		{
			// checking count
			Assert.AreEqual(oracle.Count,col.Count);
			CollectionAssert.IsCountCorrect(col);

			// checking elements
			foreach(IDataGenerator gen in this.factory.Generators)
			{
				bool contains = this.oracle.Contains(gen);
				if (contains)
				{
					// check contains
					Assert.IsTrue(col.Contains(gen));
					Assert.IsTrue(col.Contains(gen.Column));
					Assert.IsTrue(col.Contains(gen.Column.ColumnName));

					// check item
					Assert.AreEqual(gen,col[gen.Column]);
					Assert.AreEqual(gen,col[gen.Column.ColumnName]);
				}
				else
				{
					Assert.IsFalse(col.Contains(gen));
					Assert.IsFalse(col.Contains(gen.Column));
					Assert.IsFalse(col.Contains(gen.Column.ColumnName));

					Assert.AreEqual(null,col[gen.Column]);
					Assert.AreEqual(null,col[gen.Column.ColumnName]);
				}
			}
		}
		#endregion

		#region Production execution
		[Grammar]
		public Grammar This()
		{
			return this;
		}

		[Seed]
		public int Seed10()
		{
			return 10;
		}
		[Seed]
		public int Seed50()
		{
			return 50;
		}
		[Seed]
		public int Seed200()
		{
			return 200;
		}
		#endregion
	}
}
