using System;
using System.Data;

namespace TestFu.Data.Populators
{
    /// <summary>
    /// Default implementation of <see cref="IForeignKeyProvider"/>
    /// </summary>
    /// <include 
    ///		file='Data/TestFu.Data.Doc.xml' 
    ///		path='//example[contains(descendant-or-self::*,"ForeignKeyProviderBase")]'
    ///		/>
    public abstract class ForeignKeyProviderBase : IForeignKeyProvider
    {
        private static Random rnd = new Random((int)DateTime.Now.Ticks);
        private ITablePopulator foreignTable;
        private ForeignKeyConstraint foreignKey;
        private bool isNullable;
        private double nullProbability = 0.4;

        public ForeignKeyProviderBase(ITablePopulator foreignTable, ForeignKeyConstraint foreignKey)
        {
            if (foreignTable == null)
                throw new ArgumentNullException("foreignTable");
            if (foreignKey == null)
                throw new ArgumentNullException("foreignKey");
            this.foreignTable = foreignTable;
            this.foreignKey = foreignKey;

            isNullable = true;
            foreach (DataColumn column in foreignKey.Columns)
            {
                if (!column.AllowDBNull)
                {
                    isNullable = false;
                    break;
                }
            }
        }

        public ITablePopulator ForeignTable
        {
            get
            {
                return this.foreignTable;
            }
        }

        public System.Data.ForeignKeyConstraint ForeignKey
        {
            get
            {
                return this.foreignKey;
            }
        }

        public bool IsNullable
        {
            get
            {
                return this.isNullable;
            }
        }

        protected bool FillNull(DataRow row)
        {
            if (!this.IsNullable)
                return false;

            if (rnd.NextDouble() < this.nullProbability)
                return false;

            for (int ic = 0; ic < this.ForeignKey.Columns.Length; ++ic)
            {
                // inserting in row
                row[this.ForeignKey.Columns[ic]] = DBNull.Value;
            }
            return true;
        }

        protected static Random Rnd
        {
            get
            {
                lock (typeof(ForeignKeyProviderBase))
                {
                    return rnd;
                }
            }
        }

        public abstract bool IsEmpty {get;}
        public abstract void Provide(DataRow row);
    }
}
