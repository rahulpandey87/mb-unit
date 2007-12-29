using System;
using System.Data;
using TestFu.Data.Graph;

namespace TestFu.Data
{
	/// <summary>
	/// A database populator instance.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IDatabasePopulator")]'
	///		/>
	public interface IDatabasePopulator
	{
		/// <summary>
		/// Sets up the generators for the given <see cref="DataSet"/>
		/// instance.
		/// </summary>
		/// <param name="dataSet">
		/// A <see cref="DataSet"/> representing the structure of the 
		/// database to populate.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="dataSet"/> is a null reference (Nothing in 
		/// Visual Basic)
		/// </exception>
		void Populate(DataSet dataSet);

		/// <summary>
		/// Gets the <see cref="DataSet"/> instance associated
		/// to this populator.
		/// </summary>
		/// <value>
		/// A <see cref="DataSet"/> schema used to set-up the generators.
		/// </value>
		DataSet DataSet{get;}

		/// <summary>
		/// Gets a collection <see cref="ITablePopulator"/> associated
		/// to each table.
		/// </summary>
		/// <value>
		/// A <see cref="ITablePopulatorCollection"/> containing
		/// populator associated to each <see cref="DataTable"/>.
		/// </value>
		ITablePopulatorCollection Tables {get;}

        /// <summary>
        /// Gets the <see cref="DataGraph"/> associated to the
        /// database.
        /// </summary>
        /// <value></value>
        DataGraph Graph { get;}
    }
}
