using System;

namespace QuickGraph.Algorithms.AllShortestPath
{
	using QuickGraph.Concepts;
	
	public class FloydWarshallEventArgs : EventArgs
	{
		private IVertex source;
		private IVertex target;
		private IVertex intermediate;

		public FloydWarshallEventArgs(IVertex source, IVertex target)
		{
			if (source==null)
				throw new ArgumentNullException(@"source");
			if (target==null)
				throw new ArgumentNullException(@"target");
			this.source = source;
			this.target = target;
			this.intermediate = null;
		}

		public FloydWarshallEventArgs(IVertex source, IVertex target, IVertex intermediate)
		{
			if (source==null)
				throw new ArgumentNullException(@"source");
			if (target==null)
				throw new ArgumentNullException(@"target");
			if (intermediate==null)
				throw new ArgumentNullException(@"intermediate");
			this.source = source;
			this.target = target;
			this.intermediate = intermediate;
		}
	
		public IVertex Source
		{
			get
			{
				return this.source;
			}
		}
	
		public IVertex Target
		{
			get
			{
				return this.target;
			}
		}
	
		public IVertex Intermediate
		{
			get
			{
				return this.intermediate;
			}
		}
	}

	public delegate void FloydWarshallEventHandler(Object sender, FloydWarshallEventArgs e);
}
