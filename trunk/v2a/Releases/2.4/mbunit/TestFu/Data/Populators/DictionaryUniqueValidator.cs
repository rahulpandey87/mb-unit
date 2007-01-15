using System;
using System.Data;
using System.Collections;
using System.IO;

namespace TestFu.Data.Populators
{
	/// <summary>
	/// A <see cref="IUniqueValidator"/> based on a <see cref="IDictionary"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DictionaryUniqueValidator")]'
	///		/>
	public class DictionaryUniqueValidator : UniqueValidatorBase
	{
		private Random rnd = new Random();
		private Hashtable keys= new Hashtable();

		public DictionaryUniqueValidator(ITablePopulator table, UniqueConstraint unique)
		:base(table,unique)
		{}
		public ICollection Keys
		{
			get
			{
				return this.keys.Values;
			}
		}

		public override bool IsEmpty
		{
			get
			{
				return this.keys.Count==0;
			}
		}

		public override DataRow GetKey()
		{
			int index = this.rnd.Next(this.keys.Count);
			int i=0;
			foreach(DataRow key in this.keys.Values)
			{
				if (i==index)
					return key;
				++i;
			}
			throw new Exception();
		}

		public override void AddKey(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");

			String key = GetKey(row);
			this.keys.Add(key,row);
		}

		public override void RemoveKey(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");

			String key = GetKey(row);
			this.keys.Remove(key);
		}

		public override bool Contains(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");

			String key = GetKey(row);
			return this.keys.Contains(key);
		}

		protected virtual string GetKey(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");
			StringWriter sw =new StringWriter();
			foreach(DataColumn column in this.Unique.Columns)
			{
				object value= row[column.Ordinal];
				sw.Write(",{0}",value);
			}		
			return sw.ToString();
		}
	}
}
