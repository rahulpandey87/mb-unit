using System;
using System.Collections;
using System.Data;
namespace TestFu.Data
{	
	/// <summary>
	/// An random <see cref="DataRow"/> generator compatible with the schema
	/// of a given <see cref="DataTable"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"ITablePopulator")]'
	///		/>
	public interface ITablePopulator
	{
		/// <summary>
		/// Gets an instance of the <see cref="IDatabasePopulator"/>$
		/// that contains this populator.
		/// </summary>
		/// <value>
		/// Parent <see cref="IDatabasePopulator"/> instance.
		/// </value>
		IDatabasePopulator Database{get;}

		/// <summary>
		/// Gets the <see cref="DataTable"/> instance that is the model
		/// to be populated.
		/// </summary>
		/// <value>
		/// A <see cref="DataTable"/> instance whos schema is used to create
		/// new <see cref="DataRow"/>.
		/// </value>
		DataTable Table {get;}

		/// <summary>
		/// Gets a collection of <see cref="IUniqueValidator"/>
		/// associated to each <see cref="UniqueConstraint"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IUniqueValidatorCollection"/> instance
		/// containing the validators associated to each unique constraint.
		/// </value>
		IUniqueValidatorCollection Uniques {get;}

		/// <summary>
		/// Gets a collection of <see cref="IForeignKeyProvider"/>
		/// associated to each <see cref="ForeignKeyConstraint"/>.
		/// </summary>
		/// <value>
		/// A <see cref="IForeignKeyProviderCollection"/> instance
		/// containing the providers associated to each foreign key.
		/// </value>
		IForeignKeyProviderCollection ForeignKeys{get;}

		/// <summary>
		/// Gets a collection of <see cref="IDataGenerator"/> associated
		/// to each column of the table.
		/// </summary>
		/// <value>
		/// A <see cref="IDataGeneratorCollection"/> instance
		/// containing the generators associated to each column.
		/// </value>
		IDataGeneratorCollection Columns{get;}

        /// <summary>
        /// Gets the <see cref="ICheckValidator"/> that ensures CHECK constraints.
        /// </summary>
        /// <value>
        /// A <see cref="ICheckValidator"/> instance if any check constraint to verify;
        /// otherwize a null reference.
        /// </value>
        ICheckValidator CheckValidator { get;set;}

        /// <summary>
		/// Generates a new <see cref="DataRow"/>.
		/// </summary>
		/// <returns>
		/// Generated <see cref="DataRow"/> instance.
		/// </returns>
		DataRow Generate();

        /// <summary>
        /// Updates randomly a number of rows
        /// </summary>
        /// <param name="row"></param>
        void ChangeRowValues(DataRow row);

        /// <summary>
        /// Updates randomly a number of rows
        /// </summary>
        /// <param name="row"></param>
        void ChangeRowValues(DataRow row, bool updateForeignKeys);

        /// <summary>
		/// Gets the latest generated <see cref="DataRow"/>.
		/// </summary>
		/// <value>
		/// Latest generated <see cref="DataRow"/>.
		/// </value>
		DataRow Row{get;}
    }
}
