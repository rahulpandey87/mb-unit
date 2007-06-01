namespace TestFu.Grammars
{
	/// <summary>
	/// System implementation of <see cref="IRandom"/>
	/// </summary>
	public class Random : System.Random, IRandom
	{
		/// <summary>
		/// Creates an instance initialized using <see cref="System.DateTime"/>.Now.Ticks.
		/// </summary>
		public Random()
			:base((int)System.DateTime.Now.Ticks)
		{}
	}
}
