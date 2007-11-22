using System;
using System.Data;
using QuickGraph;
using QuickGraph.Providers;
using QuickGraph.Concepts;

namespace TestFu.Data.Graph
{
	/// <summary>
	/// A <see cref="IEdge"/> with a <see cref="DataRelation"/> instance.
	/// </summary>
	public class DataRelationEdge : Edge
	{
		private DataRelation relation = null;
		
		public DataRelationEdge(int id, IVertex source, IVertex target)
		:base(id,source,target)
		{}
		
		public new DataTableVertex Source
		{
			get
			{
				return (DataTableVertex)base.Source;
			}
		}

		
		public new DataTableVertex Target
		{
			get
			{
				return (DataTableVertex)base.Target;
			}
		}
		
		public DataRelation Relation
		{
			get
			{
				if (this.relation==null)
					throw new InvalidOperationException("relation is not initialized");
				return this.relation;				
			}
			set
			{
				this.relation  = value;
			}
		}
	}
	
	public class DataRelationEdgeProvider : TypedEdgeProvider
	{
		public DataRelationEdgeProvider()
		:base(typeof(DataRelationEdge))
		{}		
	}
}
