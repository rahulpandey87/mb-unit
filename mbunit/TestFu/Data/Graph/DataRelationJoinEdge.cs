using System;
using System.Data;
using QuickGraph;
using QuickGraph.Providers;
using QuickGraph.Concepts;

namespace TestFu.Data.Graph
{
    public class DataRelationJoinEdge : DataRelationEdge
    {
        private JoinType joinType = JoinType.Inner;

        public DataRelationJoinEdge(int id, IVertex source, IVertex target)
		:base(id,source,target)
        { }

        public JoinType JoinType
        {
            get
            {
                return this.joinType;
            }
            set
            {
                this.joinType = value;
            }
        }

        public new DataTableJoinVertex Source
        {
            get
            {
                return (DataTableJoinVertex)base.Source;
            }
        }


        public new DataTableJoinVertex Target
        {
            get
            {
                return (DataTableJoinVertex)base.Target;
            }
        }
    }

    public class DataRelationJoinEdgeProvider : TypedEdgeProvider
    {
        public DataRelationJoinEdgeProvider()
		:base(typeof(DataRelationJoinEdge))
        { }
    }
}