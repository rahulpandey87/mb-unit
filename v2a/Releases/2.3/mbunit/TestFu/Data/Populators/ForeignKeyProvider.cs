using System;
using System.Data;

namespace TestFu.Data.Populators
{
	/// <summary>
	/// Default implementation of <see cref="IForeignKeyProvider"/>
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"ForeignKeyProvider")]'
	///		/>
	public class ForeignKeyProvider : ForeignKeyProviderBase
	{
        public ForeignKeyProvider(ITablePopulator foreignTable, ForeignKeyConstraint foreignKey)
            :base(foreignTable,foreignKey)
		{}

		public override bool IsEmpty
		{
			get
			{
				return this.ForeignTable.Uniques.PrimaryKey.IsEmpty;
			}
		}

		public override void Provide(DataRow row)
		{
            // check if nullable
            if (this.FillNull(row))
                return;

            if (this.IsEmpty)
            {
                // check if relation is nullable
                if (this.IsNullable)
                {
                    for (int ic = 0; ic < this.ForeignKey.Columns.Length; ++ic)
                    {
                        // inserting in row
                        row[this.ForeignKey.Columns[ic]] = DBNull.Value;
                    }
                    return;
                }

                string msg = String.Format("Key set of the {0} table is empty"
                    ,this.ForeignTable.Table.TableName
                    );
                throw new InvalidOperationException(msg);
            }

            DataRow keyRow = this.ForeignTable.Uniques.PrimaryKey.GetKey();

			for(int ic =0;ic<this.ForeignKey.Columns.Length;++ic)
			{
				// getting data
				Object data = keyRow[ this.ForeignKey.RelatedColumns[ic] ];
				// inserting in row
				row[ this.ForeignKey.Columns[ic] ] = data;
			}
		}
	}
}
