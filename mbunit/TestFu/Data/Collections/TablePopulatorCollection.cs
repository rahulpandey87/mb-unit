using System;
using System.Collections;
using System.Data;

namespace TestFu.Data.Collections
{
	/// <summary>
	/// Summary description for TablePopulatorCollection.
	/// </summary>
	public class TablePopulatorCollection : ITablePopulatorCollection
	{
		private Hashtable tablePopulators = new Hashtable();

		public TablePopulatorCollection()
		{}

		#region ITablePopulatorCollection Members

		public ITablePopulator this[System.Data.DataTable table]
		{
			get
			{
				if (table==null)
					throw new ArgumentNullException("table");
				return this[table.TableName];
			}
            set
            {
                if (table == null)
                    throw new ArgumentNullException("table");
                if (value == null)
                    throw new ArgumentNullException("value");
                this[table.TableName] = value;
            }
        }

		public ITablePopulator this[String tableName]
		{
			get
			{
				if (tableName==null)
					throw new ArgumentNullException("tableName");
				return (ITablePopulator)this.tablePopulators[tableName];
			}
            set
            {
                if (tableName == null)
                    throw new ArgumentNullException("tableName");
                if (tableName.Length==0)
                    throw new ArgumentException("Length is zero","tableName");
                if (value == null)
                    throw new ArgumentNullException("value");
                this.tablePopulators[tableName] = value;
            }
        }

		public void Add(ITablePopulator tablePopulator)
		{
			if (tablePopulator==null)
				throw new ArgumentNullException("tablePopulator");
			this.tablePopulators.Add(tablePopulator.Table.TableName, tablePopulator);
		}

		public void Remove(ITablePopulator tablePopulator)
		{
			if (tablePopulator==null)
				throw new ArgumentNullException("tablePopulator");
			this.Remove(tablePopulator.Table);
		}

		public void Remove(DataTable table)
		{
			if (table==null)
				throw new ArgumentNullException("table");
			this.Remove(table.TableName);
		}

		public void Remove(String tableName)
		{
			if (tableName==null)
				throw new ArgumentNullException("tableName");
			this.tablePopulators.Remove(tableName);
		}

		public bool Contains(ITablePopulator tablePopulator)
		{
			if (tablePopulator==null)
				throw new ArgumentNullException("tablePopulator");
			return this.Contains(tablePopulator.Table);
		}

		public bool Contains(DataTable table)
		{
			if (table==null)
				throw new ArgumentNullException("table");
			return this.Contains(table);
		}

		public bool Contains(String tableName)
		{
			if (tableName==null)
				throw new ArgumentNullException("tableName");
			return this.tablePopulators.Contains(tableName);
		}

		public void Clear()
		{
			this.tablePopulators.Clear();
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
				return this.tablePopulators.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.tablePopulators.Values.CopyTo(array,index);
		}

		public object SyncRoot
		{
			get
			{
				return tablePopulators.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return this.tablePopulators.Values.GetEnumerator();
		}

		#endregion
	}
}
