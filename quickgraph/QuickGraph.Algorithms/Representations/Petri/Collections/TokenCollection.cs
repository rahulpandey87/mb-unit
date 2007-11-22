using System;
using System.Collections;
using QuickGraph.Concepts.Petri;
using QuickGraph.Concepts.Petri.Collections;

namespace QuickGraph.Representations.Petri.Collections
{
	using QuickGraph.Concepts.Petri;
	public class TokenCollection : CollectionBase, ITokenCollection
	{
		#region ITokenCollection Members
		public void RemoveAll(object token)
		{
			while (this.List.Contains(token))
				this.List.Remove(token);
		}

		public bool Contains(object token)
		{
			return this.List.Contains(token);
		}

		public void Remove(object token)
		{
			this.List.Remove(token);
		}

		public void Add(object token)
		{
			this.List.Add(token);
		}

		public void AddRange(ITokenCollection tokens)
		{
			if (tokens==null)
				throw new ArgumentNullException("tokens");
			foreach(Object token in tokens)
				this.List.Add(token);
		}
		public void RemoveRange(ITokenCollection tokens)
		{
			if (tokens==null)
				throw new ArgumentNullException("tokens");
			foreach(Object token in tokens)
				this.List.Remove(token);
		}
		#endregion
	}
}
