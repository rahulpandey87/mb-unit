using System;
using System.Data;
using System.Collections;
using System.IO;

namespace TestFu.Data.Populators
{
	/// <summary>
	/// A <see cref="IUniqueValidator"/> querying the databse.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DbCommandUniqueValidator")]'
	///		/>
	public abstract class DbCommandUniqueValidatorBase : UniqueValidatorBase
	{
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        private IDbFactory dbFactory;
        private string connectionString;

        public DbCommandUniqueValidatorBase(
            ITablePopulator table, 
            UniqueConstraint unique,
            IDbFactory dbFactory,
            string connectionString
            )
		:base(table,unique)
		{
            if (dbFactory == null)
                throw new ArgumentNullException("factory");
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            this.dbFactory = dbFactory;
            this.connectionString = connectionString;
        }

        public abstract string GetSelectKeyQuery();
        public abstract string GetSelectCountByKeyQuery(DataRow row);

        protected virtual IDataReader SelectKeys()
        {
            IDbConnection connection = null;
            try
            {
                connection = this.DbFactory.CreateConnection(this.connectionString);
                string selectQuery = this.GetSelectKeyQuery();
                connection.Open();
                using (IDbCommand command = this.DbFactory.CreateCommand(selectQuery, connection))
                {
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                if (connection != null)
                    connection.Dispose();

                throw new ApplicationException("Error while selecting keys", ex);
            }
        }

        public override bool Contains(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            string selectCountByKeyQuery = this.GetSelectCountByKeyQuery(row);
            using (IDbConnection connection = this.DbFactory.CreateConnection(this.ConnectionString))
            {
                connection.Open();
                using (IDbCommand command = this.DbFactory.CreateCommand(selectCountByKeyQuery, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    return count != 0;
                }
            }
        }

        protected virtual int SelectKeyCount()
		{
            return this.DbFactory.SelectCount(this.Table.Table,this.ConnectionString);
		}

        public IDbFactory DbFactory
        {
            get
            {
                return this.dbFactory;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }

        public IEnumerable Keys
		{
			get
			{
				return new KeyCollection(this.Table.Table, this.SelectKeys());
			}
		}

		public override bool IsEmpty
		{
			get
			{
				return this.SelectKeyCount()==0;
			}
		}

		public override DataRow GetKey()
		{
			int index = this.rnd.Next(this.SelectKeyCount());
			
			int i=0;
			using (IDataReader dr = this.SelectKeys())
			{
				while(dr.Read())
				{
					// if i == index, we get the row
					if (i==index)
					{
						// fill a datarow with the data
						DataRow row = this.Table.Table.NewRow();
						for(int j=0;j<dr.FieldCount;++j)
							row[j]=dr[j];
						return row;
					}
					// increment i
					i++;
				}
			}
			throw new Exception("Error while looking for a key");
		}

		public override void AddKey(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");
		}

		public override void RemoveKey(DataRow row)
		{
			if (row==null)
				throw new ArgumentNullException("row");
		}
		
		internal class KeyCollection : IEnumerable
		{
			private DataTable table;
			private IDataReader dr;
			
			public KeyCollection(DataTable table,IDataReader dr)
			{
				this.table=table;
				this.dr=dr;
			}
			
			public KeyEnumerator GetEnumerator()
			{
				return new KeyEnumerator(this.table,this.dr);	
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			
			#region KeyEnumerator
			internal class KeyEnumerator : IEnumerator, IDisposable
			{
				private DataTable table;
				private IDataReader dr;
				private DataRow row=null;
				public KeyEnumerator(DataTable table, IDataReader dr)
				{
					this.table = table;
					this.dr=dr;
				}
				
				#region IEnumerator
				public void Reset()
				{
					throw new NotSupportedException();
				}
				
				public bool MoveNext()
				{
					if (dr==null)
						throw new InvalidOperationException("DataReader out of range");
					if (!dr.Read())
					{
						this.Dispose();
						return false;
					}
					
					this.row=table.NewRow();
					for(int j=0;j<this.dr.FieldCount;++j)
						this.row[j]=dr[j];
					return true;
				}
				
				public DataRow Current
				{
					get
					{
						return this.row;
					}
				}
				
				Object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}
				#endregion
				
				#region IDisposable
				public void Dispose()
				{
					if (this.dr!=null)
					{
						this.dr.Close();
						this.dr=null;
					}
				}
				#endregion
			}
			#endregion
		}
	}
}
