using System;
using System.Data;
using System.Collections;

namespace TestFu.Data.Collections
{
	public class UniqueValidatorCollection : ICollection, IUniqueValidatorCollection
	{
		private IUniqueValidator primaryKey=null;
		private WeakReference tablePopulator;
		private Hashtable contraints =new Hashtable();

		public UniqueValidatorCollection(ITablePopulator tablePopulator)
		{
			if (tablePopulator==null)
				throw new ArgumentNullException("tablePopulator");
			this.tablePopulator=new WeakReference(tablePopulator);
		}
		#region IUniqueValidatorCollection Members

		public ITablePopulator TablePopulator
		{
			get
			{
				return (ITablePopulator)this.tablePopulator.Target;
			}
		}

		public IUniqueValidator PrimaryKey
		{
			get
			{
				return this.primaryKey;
			}
		}

		public IUniqueValidator this[System.Data.UniqueConstraint unique]
		{
			get
			{
				return (IUniqueValidator)this.contraints[unique];
			}
            set
            {
                if (unique == null)
                    throw new ArgumentNullException("unique");
                if (value == null)
                    throw new ArgumentNullException("value");
                this.contraints[unique] = value;
            }
        }

		public void Add(IUniqueValidator unique)
		{
			if (unique==null)
				throw new ArgumentNullException("unique");
			if (unique.Unique.IsPrimaryKey && this.primaryKey!=null)
				throw new InvalidOperationException("duplicate primary key");
			else
				this.primaryKey=unique;

			this.contraints.Add(unique.Unique,unique);
		}

		public void Remove(IUniqueValidator unique)
		{
			if (unique==null)
				throw new ArgumentNullException("unique");
			if (unique==this.primaryKey)
				this.primaryKey=null;
			this.contraints.Remove(unique.Unique);
		}

		public void Remove(UniqueConstraint unique)
		{
			if (unique==null)
				throw new ArgumentNullException("unique");
			IUniqueValidator uv = this.contraints[unique] as IUniqueValidator;
			if (uv!=null && uv==this.primaryKey)
				this.primaryKey=null;
			this.contraints.Remove(unique);
		}

		public bool Contains(IUniqueValidator unique)
		{
			if (unique==null)
				throw new ArgumentNullException("unique");
			return this.Contains(unique.Unique);
		}

		public bool Contains(UniqueConstraint unique)
		{
			if (unique==null)
				throw new ArgumentNullException("unique");
			return this.contraints.Contains(unique);
		}
		
		public void Clear()
		{
			this.contraints.Clear();
			this.primaryKey=null;
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
