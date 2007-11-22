using System;
using System.Data;
using System.Collections;
using System.IO;

namespace TestFu.Data.Populators
{
	/// <summary>
	/// Base class for <see cref="IUniqueValidator"/> implementation.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"UniqueValidatorBase")]'
	///		/>
	public abstract class UniqueValidatorBase : IUniqueValidator
	{
		private WeakReference table;
		private UniqueConstraint unique;
        private bool isIdentity = false;

		public UniqueValidatorBase(ITablePopulator table, UniqueConstraint unique)
		{
			if(table==null)
				throw new ArgumentNullException("table");
			if(unique==null)
				throw new ArgumentNullException("unique");
			this.table=new WeakReference(table);
			this.unique=unique;
		}

        public virtual bool IsIdentity
        {
            get
            {
                return this.isIdentity;
            }
            set
            {
                this.isIdentity = value;
            }
        }

        public virtual ITablePopulator Table
		{
			get
			{
				return (ITablePopulator)table.Target;
			}
		}

		public virtual  UniqueConstraint Unique
		{
			get
			{
				return unique;
			}
		}

		public abstract bool IsEmpty {get;}

		public abstract DataRow GetKey();

		public abstract void AddKey(DataRow row);

		public abstract void RemoveKey(DataRow row);

		public abstract bool Contains(DataRow row);
	}
}
