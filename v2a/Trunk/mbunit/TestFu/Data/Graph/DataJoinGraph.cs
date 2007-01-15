using System;
using System.Collections;
using System.Diagnostics;
using System.Data;

namespace TestFu.Data.Graph
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Collections;
    using QuickGraph.Exceptions;
    using QuickGraph.Predicates;
    using QuickGraph.Representations;

    public class DataJoinGraph : BidirectionalGraph
    {
        private DataGraph dataGraph;
        private IDictionary aliasVertices = new Hashtable();

        public DataJoinGraph(DataGraph dataGraph)
            :base(
                new DataTableJoinVertexProvider(),
                new DataRelationJoinEdgeProvider(),
                false
            )
        {
            if (dataGraph == null)
                throw new ArgumentNullException("dataGraph");
            this.dataGraph = dataGraph;
        }

        public DataGraph DataGraph
        {
            get
            {
                return this.dataGraph;
            }
        }

        public DataTableJoinVertex GetVertex(string alias)
        {
            if (alias == null)
                throw new ArgumentNullException("alias");
            return (DataTableJoinVertex)this.aliasVertices[alias];
        }

        public bool ContainsVertex(string alias)
        {
            if (alias == null)
                throw new ArgumentNullException("alias");
            return this.aliasVertices.Contains(alias);
        }

        public DataTableJoinVertex AddVertex(DataTable table, string alias)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (alias == null)
                throw new ArgumentNullException("alias");
            if (alias.Length == 0)
                throw new ArgumentException("Alias length is 0");
            if (this.aliasVertices.Contains(alias))
                throw new ArgumentException("Alias " + alias + " already in graph");

            DataTableJoinVertex v = (DataTableJoinVertex)this.AddVertex();
            v.Table = table;
            v.Alias = alias;

            this.aliasVertices.Add(v.Alias, v);
            return v;
        }

        public override void RemoveVertex(IVertex v)
        {
            DataTableJoinVertex tv = (DataTableJoinVertex)v;
            this.aliasVertices.Remove(tv.Alias);
            base.RemoveVertex(v);
        }

        public virtual DataRelationJoinEdge AddEdge(
                   DataTableJoinVertex source,
                   DataTableJoinVertex target,
                   JoinType joinType
                   )
        {
            // look for the relation in the original graph
            DataRelation relation = null;
            DataTableVertex sourceVertex = this.DataGraph.GetVertex(source.Table);
            foreach (DataRelationEdge edge in this.DataGraph.OutEdges(sourceVertex))
            {
                if (edge.Target.Table == target.Table)
                {
                    if (relation != null)
                        throw new ArgumentNullException("Multiple relation, cannot choose one");
                    relation = edge.Relation;
                }
            }
            if (relation == null)
                throw new ArgumentNullException("Could not find relation between tables "
                    + source.Table.TableName + " and " + target.Table.TableName
                    );
            return this.AddEdge(source, target, joinType, relation);
        }


        public virtual DataRelationJoinEdge AddEdge(
            DataTableJoinVertex source,
            DataTableJoinVertex target,
            JoinType joinType,
            DataRelation relation
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");
            if (relation == null)
                throw new ArgumentNullException("relation");

            DataRelationJoinEdge edge = (DataRelationJoinEdge)this.AddEdge(source, target);
            edge.Relation = relation;
            edge.JoinType = joinType;

            return edge;
        }

        public override void Clear()
        {
            this.aliasVertices.Clear();
            base.Clear();
        }
    }
}

