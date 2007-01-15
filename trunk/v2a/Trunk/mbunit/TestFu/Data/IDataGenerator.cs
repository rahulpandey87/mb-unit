using System;
using System.Data;
namespace TestFu.Data
{	
	/// <summary>
	/// An random data generator.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IDataGenerator")]'
	///		/>
	public interface IDataGenerator
	{
		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated <see cref="Type"/>.
		/// </value>
		Type GeneratedType {get;}

		/// <summary>
		/// Resets the generator
		/// </summary>
		void Reset();
		
		/// <summary>
		/// Generates a new value and fills it in the corresponding <see cref="DataRow"/>.
		/// </summary>
		/// <remarks>
		/// <param>
		/// It is the user job to ensure that the <see cref="DataRow"/> instance
		/// provided is compatible with the generator definition and more 
		/// specifically the <see cref="DataColumn"/> it is associated with.
		/// </param>
		/// </remarks>
		void GenerateData(DataRow row);
		
		/// <summary>
		/// Gets or sets the probability to produce a NULL
		/// </summary>
		/// <remarks>
		/// This value determines the probability to produce a null value. 
		/// The probability ranges from
		/// 0 - never, to 1  - always.
		/// </remarks>
		/// <value>
		/// The probability to produce a null object.
		/// </value>
		double NullProbability {get;set;}
		
		/// <summary>
		/// Gets the target column
		/// </summary>
		/// <value>
		/// Target <see cref="DataColumn"/> instance.
		/// </value>
		DataColumn Column {get;set;}
	}
}
