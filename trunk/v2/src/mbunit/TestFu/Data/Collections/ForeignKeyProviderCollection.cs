using System;
using System.Collections;
using System.Data;

namespace TestFu.Data.Collections
{
	public class ForeignKeyProviderCollection : ICollection, IForeignKeyProviderCollection
	{
		private WeakReference tablePopulator;
		private Hashtable contraints =new Hashtable();

		public ForeignKeyProviderCollection(ITablePopulator tablePopulator)
		{
			if (tablePopulator==null)
				throw new ArgumentNullException("tablePopulator");
			this.tablePopulator=new WeakReference(tablePopulator);
		}
		#region IForeignKeyProviderCollection Members

		public ITablePopulator TablePopulator
		{
			get
			{
				return (ITablePopulator)this.tablePopulator.Target;
			}
		}

		public IForeignKeyProvider this[System.Data.ForeignKeyConstraint foreignKey]
		{
			get
			{
				return (IForeignKeyProvider)this.contraints[foreignKey];
			}
            set
            {
                if (foreignKey == null)
                    throw new ArgumentNullException("foreignKey");
                if (value == null)
                    throw new ArgumentNullException("value");
                this.contraints[foreignKey] = value;
            }
        }

		public void Add(IForeignKeyProvider foreignKey)
		{
			if (foreignKey==null)
				throw new ArgumentNullException("foreignKey");
			this.contraints.Add(foreignKey.ForeignKey,foreignKey);
		}

		public void Remove(IForeignKeyProvider foreignKey)
		{
			if (foreignKey==null)
				throw new ArgumentNullException("foreignKey");
			this.Remove(foreignKey.ForeignKey);
		}
		
		public void Remove(ForeignKeyConstraint foreignKey)
		{
			if (foreignKey==null)
				throw new ArgumentNullException("foreignKey");
			this.contraints.Remove(foreignKey);
		}

		public bool Contains(IForeignKeyProvider foreignKey)
		{
			if (foreignKey==null)
				throw new ArgumentNullException("foreignKey");
			return this.Contains(foreignKey.ForeignKey);
		}
		
		public bool Contains(ForeignKeyConstraint foreignKey)
		{
			if (foreignKey==null)
				throw new ArgumentNullException("foreignKey");
			return this.contraints.Contains(foreignKey);
		}

		public void Clear()
		{
			this.contraints.Clear();
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
				return this.contraints.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.contraints.Values.CopyTo(array,index);
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

		public IEnumerator GetEnumerator()
		{
			return this.contraints.Values.GetEnumerator();
		}

		#endregion
	}
}
