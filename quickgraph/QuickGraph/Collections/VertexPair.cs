using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;

	/// <summary>
	/// A class containing a pair of <see cref="IVertex"/>.
	/// </summary>
	/// <remarks>
	/// Mini-class useful in a number of situations.
	/// </remarks>
	public struct VertexPair : IComparable
	{
		private IVertex first;
		private IVertex second;

		/// <summary>
		/// Create a <see cref="IVertex"/> pair
		/// </summary>
		/// <param name="first">first <see cref="IVertex"/> instance</param>
		/// <param name="second">second <see cref="IVertex"/> instance</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="first"/> or <paramref name="second"/> is a null 
		/// reference
		/// </exception>
		public VertexPair(IVertex first, IVertex second)
		{
			if (first==null)
				throw new ArgumentNullException(@"first");
			if (second==null)
				throw new ArgumentNullException(@"second");

			this.first = first;
			this.second = second;
		}

		/// <summary>
		/// Gets or sets the first <see cref="IVertex"/> instance
		/// </summary>
		/// <value>
		/// First <see cref="IVertex"/> instance.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference
		/// </exception>
		public IVertex First
		{
			get
			{
				return this.first;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException(@"first");
				this.first = value;
			}
		}

		/// <summary>
		/// Gets or sets the second <see cref="IVertex"/> instance
		/// </summary>
		/// <value>
		/// Second <see cref="IVertex"/> instance.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference
		/// </exception>
		public IVertex Second
		{
			get
			{
				return this.second;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException(@"second");
				this.second = value;
			}
		}

		#region IComparable Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			if (!(obj is VertexPair))
				throw new ArgumentException("obj is not of type VertexPair");

			VertexPair vp = (VertexPair)obj;

			// comparing first and second
			int c = First.CompareTo(vp.First);
			if (c!=0)
				return c;
			else
				return Second.CompareTo(vp.Second);
		}

		#endregion
	}
}
