using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random generator of <see cref="System.Guid"/> values.
	/// </summary>	
	public class GuidGenerator : DataGeneratorBase
	{		
		public GuidGenerator(DataColumn column)
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
				return typeof(System.Guid);
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
			if (this.FillNull(row))
				return;
			
			this.FillData(row, Guid.NewGuid());
		}		
	}
}
