using System;
using System.Collections;
using System.Collections.Specialized;

namespace TestFu.Data.Collections
{
	/// <summary>
	/// Summary description for DataGeneratorCollection.
	/// </summary>
	public class DataGeneratorCollection : IDataGeneratorCollection
	{
		private IDictionary generators=new HybridDictionary();

		public DataGeneratorCollection()
		{}
		#region IDataGeneratorCollection Members

		public IDataGenerator this[System.Data.DataColumn column]
		{
			get
			{
				if (column==null)
					throw new ArgumentNullException("column");
				return (IDataGenerator)this.generators[column.ColumnName];
			}
		}

		public IDataGenerator this[String columnName]
		{
			get
			{
				if (columnName==null)
					throw new ArgumentNullException("columnName");
				return (IDataGenerator)this.generators[columnName];
			}
		}

		public void Add(IDataGenerator dataGenerator)
		{
			if (dataGenerator==null)
				throw new ArgumentNullException("dataGenerator");
			this.generators.Add(dataGenerator.Column.ColumnName,dataGenerator);
		}

		public void Remove(IDataGenerator dataGenerator)
		{
			if (dataGenerator==null)
				throw new ArgumentNullException("dataGenerator");
			this.Remove(dataGenerator.Column);
		}

		public void Remove(System.Data.DataColumn column)
		{
			if (column==null)
				throw new ArgumentNullException("column");
			this.Remove(column.ColumnName);
		}

		public void Remove(String columnName)
		{
			if (columnName==null)
				throw new ArgumentNullException("columnName");
			this.generators.Remove(columnName);
		}

		public bool Contains(IDataGenerator dataGenerator)
		{
			if (dataGenerator==null)
				throw new ArgumentNullException("dataGenerator");
			return this.Contains(dataGenerator.Column);
		}

		public bool Contains(System.Data.DataColumn column)
		{
			if (column==null)
				throw new ArgumentNullException("column");
			return this.Contains(column.ColumnName);
		}

		public bool Contains(String columnName)
		{
			if (columnName==null)
				throw new ArgumentNullException("columnName");
			return this.generators.Contains(columnName);
		}

		public void Clear()
		{
			this.generators.Clear();
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public int Count
		{
			get
			{
				return this.generators.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.generators.Values.CopyTo(array,index);
		}

		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return this.generators.Values.GetEnumerator();
		}

		#endregion
	}
}
