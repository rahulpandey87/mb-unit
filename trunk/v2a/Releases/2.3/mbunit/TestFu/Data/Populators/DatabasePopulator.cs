using System;
using System.Data;
using TestFu.Data.Graph;

namespace TestFu.Data.Populators
{
	using TestFu.Data.Collections;

	/// <summary>
	/// Default <see cref="IDatabasePopulator"/> implementation.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DatabasePopulator")]'
	///		/>
	public class DatabasePopulator : IDatabasePopulator
	{
		#region fields
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        private DataSet dataSet = null;
        private DataGraph graph = null;
        private ITablePopulatorCollection tables = new TablePopulatorCollection();
		#endregion

        public Random Rnd
        {
            get
            {
                lock (typeof(DatabasePopulator))
                {
                    return rnd;
                }
            }
        }

        /// <summary>
		/// Initiliazes a <see cref="DatabasePopulator"/> instance.
		/// </summary>
		public DatabasePopulator()
		{}

		public virtual void Populate(DataSet dataSet)
		{
			if (dataSet==null)
				throw new ArgumentNullException("dataSet");			
			this.tables.Clear();
			this.dataSet=dataSet;
            this.BuildGraph();
            this.PopulateTables();
            this.PopulateForeignKeys();
        }

        protected virtual void BuildGraph()
        {
            DataGraphPopulator gp = new DataGraphPopulator();
            gp.DataSource = this.DataSet;
            gp.Populate();
            this.graph = gp.Graph;
        }

        protected virtual void PopulateTables()
        {
            // create tables
            foreach (DataTable table in this.dataSet.Tables)
            {
                TablePopulator pop = new TablePopulator(this, table);
                this.Tables.Add(pop);
            }
        }

        protected virtual void PopulateForeignKeys()
        {
            foreach (TablePopulator pop in this.Tables)
                pop.PopulateForeignKeys();
        }

		#region IDatabasePopulator Members

		public DataSet DataSet
		{
			get
			{
				return this.dataSet;
			}
		}

        public DataGraph Graph
        {
            get
            {
                return this.graph;
            }
        }

		public ITablePopulatorCollection Tables
		{
			get
			{
				return this.tables;
			}
		}
		#endregion
	}
}
