using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;

	/// <summary>
	/// Summary description for ReversedEdge.
	/// </summary>
	public  sealed class ReversedEdge : IEdge
	{
		private IEdge wrapped;

		public ReversedEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");

			this.wrapped = e;
		}

		public IEdge Wrapped
		{
			get
			{
				return this.wrapped;
			}
		}
		#region IEdge Members

		public IVertex Target
		{
			get
			{
				return this.wrapped.Source;
			}
			set
			{
				this.wrapped.Source = value;
			}
		}

		public IVertex Source
		{
			get
			{
				return this.wrapped.Target;
			}
			set
			{
				this.wrapped.Target=value;
			}
		}

		public int ID
		{
			get
			{
				return this.wrapped.ID;
			}
			set
			{
				this.wrapped.ID = value;
			}		
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// Defines the == operator
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator == (ReversedEdge e1, ReversedEdge e2)
		{
			return e1.CompareTo(e2)==0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator != (ReversedEdge e1, ReversedEdge e2)
		{
			return e1.CompareTo(e2)!=0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			ReversedEdge re = obj as ReversedEdge;
			if (re==null)
				throw new ArgumentException("obj is not ReversedEdge");
			return this.wrapped.CompareTo(re.wrapped)==0;
		}

		public int CompareTo(object obj)
		{
			return this.wrapped.CompareTo(obj);
		}

		public override int GetHashCode()
		{
			return this.wrapped.GetHashCode();
		}


		#endregion
	}
}
