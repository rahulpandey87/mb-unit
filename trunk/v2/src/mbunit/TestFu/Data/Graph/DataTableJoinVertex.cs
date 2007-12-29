using System;
using System.Data;
using QuickGraph;
using QuickGraph.Providers;

namespace TestFu.Data.Graph
{
    public class DataTableJoinVertex : DataTableVertex
    {
        private string alias = null;

        public DataTableJoinVertex(int id)
		:base(id)
        { }

        public string Alias
        {
            get
            {
                return this.alias;
            }
            set
            {
                this.alias = value;
            }
        }

        public bool HasAlias
        {
            get
            {
                return this.alias != null;
            }
        }
    }

    public class DataTableJoinVertexProvider : TypedVertexProvider
    {
        public DataTableJoinVertexProvider()
		:base(typeof(DataTableJoinVertex))
        { }
    }
}