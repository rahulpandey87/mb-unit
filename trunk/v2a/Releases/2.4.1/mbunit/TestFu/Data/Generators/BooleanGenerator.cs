using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random generator of <see cref="System.Boolean"/> values.
	/// </summary>
	public class BooleanGenerator : DataGeneratorBase
	{
		private double trueProbability = 0.5;
		
		/// <summary>
		/// Creates an instance with <see cref="TrueProbability"/> equal to 0.5.
		/// </summary>
		public BooleanGenerator(DataColumn column)
		:base(column)
		{}

		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated type.
		/// </value>
		public override Type GeneratedType 
		{
			get
			{
				return typeof(System.Boolean);
			}
		}
		
		/// <summary>
		/// Gets or sets the probability to return true.
		/// </summary>
		/// <value>
		/// Probability to return true.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="trueProbability"/> is not in <c>[0,1]</c>.
		/// </exception>
		public double TrueProbability
		{
			get
			{
				return this.trueProbability;
			}
			set
			{
				if (value<0)
					throw new ArgumentOutOfRangeException("TrueProbability",value,"Must be greater than 0");
				if (value>1)
					throw new ArgumentOutOfRangeException("TrueProbability",value,"Must be lower than 1");
				this.trueProbability = value;
			}
		}
		
		/// <summary>
		/// Generates a new value
		/// </summary>
		/// <returns>
		/// New random data.
		/// </returns>		
		public override void GenerateData(DataRow row)
		{
			// check if null	
			if (this.FillNull(row))
				return;
			
			// generate bool
			this.FillData(row,Rnd.NextDouble() < this.TrueProbability);
		}		
	}
}
