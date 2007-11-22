using System;
using System.Collections;
using System.Drawing;

namespace QuickGraph.Concepts.Collections
{
	public interface IVertexPointFDictionary : IDictionary
	{
		void Add(IVertex v, PointF p);
		void Remove(IVertex v);
		bool Contains(IVertex v);
		PointF this[IVertex v] {get;set;}
	}
}
