using System;
using System.Collections;
using System.Data;
namespace TestFu.Data
{	
	/// <summary>
	/// An instance that can fill a <see cref="DataRow"/> with
	/// data that are compatible with a 
	/// given <see cref="ForeignKeyConstraint"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IForeignKeyProvider")]'
	///		/>
	public interface IForeignKeyProvider
	{
		/// <summary>
		/// Gets the table populator associated to the foreign table.
		/// </summary>
		/// <value>
		/// The <see cref="ITablePopulator"/> instance
		/// associated to the foreign
		/// table.
		/// </value>
		ITablePopulator ForeignTable{get;}

		/// <summary>
		/// Gets the foreign constraint that needs to be satisfied.
		/// </summary>
		/// <value>
		/// The <see cref="ForeignKeyConstraint"/> associated to this
		/// provider.
		/// </value>
		ForeignKeyConstraint ForeignKey{get;}

		/// <summary>
		/// Gets a value indicating that the foreign table is empty and
		/// cannot provide keys.
		/// </summary>
		/// <value>
		/// true if the foreign table is empty; otherwise false.
		/// </value>
		bool IsEmpty {get;}

		/// <summary>
		/// Fill the row with data that is compatible with
		/// the foreign key.
		/// </summary>
		/// <param name="row">
		/// <see cref="DataRow"/> instance to fill.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="row"/> is a null reference (Nothing
		/// in Visual Basic).
		/// </exception>
		void Provide(DataRow row);

        bool IsNullable { get;}
    }
}
