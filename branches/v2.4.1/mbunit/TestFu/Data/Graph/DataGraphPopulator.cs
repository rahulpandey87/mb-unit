using System;
using System.Data;

using QuickGraph;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;

namespace TestFu.Data.Graph
{
	/// <summary>
	/// A populator of <see cref="DataGraph"/> instance.	
	/// </summary>
	public class DataGraphPopulator
	{
		private DataSet dataSource = null;
		private DataGraph graph = null;
		
		public DataGraphPopulator()
		{}
		
		public DataSet DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				this.dataSource = value;
			}
		}
		
		public DataGraph Graph
		{
			get
			{
				return this.graph;
			}
		}
		
		public void Populate()
		{
			// check dataset is not null
			if (this.DataSource==null)
				throw new InvalidOperationException("DataSource not set");
			
			this.graph = new DataGraph();
			
			// add tables;
			foreach(DataTable table in this.DataSource.Tables)
			{
				this.graph.AddVertex(table);
			}
			
			// adding relations
			foreach(DataRelation relation in this.DataSource.Relations)
			{
				this.graph.AddEdge(relation);
			}
		}
	}
}
