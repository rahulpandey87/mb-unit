using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;

namespace TestFu.Data.Populators
{	
	using TestFu.Data.Generators;
	using TestFu.Data.Collections;

	/// <summary>
	/// An smart random <see cref="DataRow"/> generator.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"TablePopulator")]'
	///		/>
	public class TablePopulator : ITablePopulator
	{
        private static Random rnd = new Random((int)DateTime.Now.Ticks);
        private IDatabasePopulator database=null;
		private DataTable table=null;
		private IUniqueValidatorCollection uniques=null;
		private IForeignKeyProviderCollection foreignKeys=null;
		private IDataGeneratorCollection dataGenerators=new DataGeneratorCollection();
        private ICheckValidator checkValidator = null;
        private DataRow row=null;

        private int maxGenerationCount=100;
		private int generationCount=0;
		
		public TablePopulator(IDatabasePopulator database,DataTable table)
		{
			if (database==null)
				throw new ArgumentNullException("database");
			if (table==null)
				throw new ArgumentNullException("table");

			this.database = database;
			this.table = table;
			this.uniques=new UniqueValidatorCollection(this);
			this.foreignKeys=new ForeignKeyProviderCollection(this);

            this.PopulateDataGenerators();
			this.PopulateUniques();
		}
		
		#region ITablePopulator		
		public IDatabasePopulator Database
		{
			get
			{
				return this.database;
			}
		}

		public DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		public IUniqueValidatorCollection Uniques
		{
			get
			{
				return this.uniques;
			}
		}

		public IForeignKeyProviderCollection ForeignKeys
		{
			get
			{
				return this.foreignKeys;
			}
		}

		public IDataGeneratorCollection Columns
		{
			get
			{
				return this.dataGenerators;
			}
		}
        public ICheckValidator CheckValidator
        {
            get
            {
                return this.checkValidator;
            }
            set
            {
                this.checkValidator = value;
            }
        }

        public DataRow Generate()
		{			
			// create a new row
			this.CreateEmptyRow();

            this.generationCount=0;			
			while(this.generationCount<this.maxGenerationCount)
			{
				// count the number of generations
				this.generationCount++;
				
				// generate data randomly
				this.GenerateData();
				
				// first generate/fetch valid foreign keys
				this.GenerateForeignKeys();

                // enfore check constraint
                // otherwize regenerate
                if (!this.EnforceCheck())
                    break;

                // verify unique key constraints
				// if not valid, try again
				if (this.AreUniqueKeysValid(false))
					break;
			}
			
			if (this.generationCount==this.maxGenerationCount)
				throw new Exception("Could not generate valid row");
			
			// add key to uniques
			foreach(IUniqueValidator unique in this.Uniques)
				unique.AddKey(this.row);

			return this.row;
		}

        public virtual void ChangeRowValues(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException("row");
            this.ChangeRowValues(row, false);
        }

        public virtual void ChangeRowValues(DataRow row, bool updateForeignKeys)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            this.row = row;

            this.generationCount = 0;
            while (this.generationCount < this.maxGenerationCount)
            {
                // count the number of generations
                this.generationCount++;

                // generate data randomly
                this.GenerateDataForNonKeyFields(updateForeignKeys);

                if (updateForeignKeys)
                    this.GenerateForeignKeys();

                // enfore check constraint
                // otherwize regenerate
                if (!this.EnforceCheck())
                    break;

                // verify unique key constraints
                // if not valid, try again
                if (this.AreUniqueKeysValid(true))
                    break;
            }

            if (this.generationCount == this.maxGenerationCount)
                throw new Exception("Could not update valid row");
        }
        
        protected virtual void CreateEmptyRow()
		{
			this.row = this.table.NewRow();
		}
	
		protected virtual void GenerateData()
		{
			foreach(IDataGenerator dataGenerator in this.Columns)
			{
				dataGenerator.GenerateData(this.row);
			}
		}

        protected virtual void GenerateDataForNonKeyFields(bool updateForeignKeys)
        {
            foreach (IDataGenerator dg in this.Columns)
            {
                // check it is not a pk
                if (this.IsInPrimaryKey(dg.Column))
                    continue;
                // if notupdate foreign keys, check it is not
                if (!updateForeignKeys)
                {
                    if (this.IsInForeignKey(dg.Column))
                        continue;
                }

                // generate
                dg.GenerateData(this.row);
            }
        }

        protected virtual bool IsInPrimaryKey(DataColumn column)
        {
            foreach (DataColumn pkColumn in this.Table.PrimaryKey)
                if (column == pkColumn)
                    return true;
            return false;
        }

        protected virtual bool IsInForeignKey(DataColumn column)
        {
            foreach (DataRelation relation in this.Table.ParentRelations)
            {
                foreach (DataColumn fkColumn in relation.ChildColumns)
                    if (fkColumn == column)
                        return true;
            }
            return false;
        }

        protected virtual void GenerateForeignKeys()
		{
            foreach (IForeignKeyProvider foreignKey in this.ForeignKeys)
            {
                foreignKey.Provide(this.Row);
            }
		}

        protected virtual bool EnforceCheck()
        {
            if (this.checkValidator == null)
                return true;

            return this.checkValidator.Enforce(this.Row);
        }

        protected virtual bool AreUniqueKeysValid(bool ignorePrimaryKey)
		{
			foreach(IUniqueValidator uniqueKey in this.Uniques)
			{
                // if the unique key is autoincrement, ignore it
                if (uniqueKey.IsIdentity)
                    continue;
                // if we want to ignore primary keys for an update
                if (ignorePrimaryKey && uniqueKey.Unique.IsPrimaryKey)
                    continue;
                if (uniqueKey.Contains(this.row))
					return false;
			}
			return true;
		}
		
		public DataRow Row
		{
			get
			{
				return this.row;
			}
		}
		#endregion
		
		#region Properties
		public int MaxGenerationCount
		{
			get
			{
				return this.maxGenerationCount;
			}
			set
			{
				this.maxGenerationCount=value;
			}
		}
		
		public int GenerationCount
		{
			get
			{
				return this.generationCount;
			}
		}
		#endregion

		#region Population
		protected virtual void PopulateDataGenerators()
		{
			foreach(DataColumn column in this.Table.Columns)
			{
				IDataGenerator gen = DataGeneratorConverter.FromColumn(column);
				this.dataGenerators.Add(gen);
			}
		}

		protected virtual void PopulateUniques()
		{
			foreach(Constraint constraint in this.Table.Constraints)
			{
				UniqueConstraint unique = constraint as UniqueConstraint;
				if (unique==null)
					continue;

                MethodUniqueValidator muv = this.CreateMethodUniqueValidator(unique);
                if (muv != null)
                {
                    this.uniques.Add(muv);
                    continue;
                }

                IUniqueValidator uv =new DictionaryUniqueValidator(this,unique);
				this.uniques.Add(uv);
			}
		}

		internal void PopulateForeignKeys()
		{
			foreach(Constraint constraint in this.Table.Constraints)
			{
				ForeignKeyConstraint fk = constraint as ForeignKeyConstraint;
				if (fk==null)
					continue;

                ITablePopulator foreignTable = null;
                if (fk.RelatedTable == this.Table)
                    foreignTable = this;
                else
    				foreignTable = this.database.Tables[fk.RelatedTable];

				// create and add
				IForeignKeyProvider fkp = new ForeignKeyProvider(foreignTable,fk);
				this.foreignKeys.Add(fkp);
			}
		}

        protected MethodUniqueValidator CreateMethodUniqueValidator(UniqueConstraint unique)
        {
            StringBuilder methodName = new StringBuilder("FindBy");
            foreach (DataColumn col in unique.Columns)
                methodName.Append(col.ColumnName);

            MethodInfo method =this.Table.GetType().GetMethod(methodName.ToString());
            if (method == null)
                return null;

            MethodUniqueValidator validator = new MethodUniqueValidator(this, unique, method);
            return validator;
        }
		#endregion

        public override string ToString()
        {
            return this.Table.TableName;
        }
    }
}
