
using System;
using System.Data;
using QuickGraph;
using QuickGraph.Providers;

namespace TestFu.Data.Graph
{
	public class DataTableVertex : Vertex
	{
		private DataTable table = null;
		
		public DataTableVertex(int id)
		:base(id)
		{}
		
		public DataTable Table
		{
			get
			{
				if (this.table==null)
					throw new InvalidOperationException("table not initialized");
				return this.table;
			}
			set
			{
				this.table = value;
			}
		}
	}
	
	public class DataTableVertexProvider : TypedVertexProvider
	{
		public DataTableVertexProvider()
		:base(typeof(DataTableVertex))
		{}
	}
}
