using System;
using System.Data;
using System.Collections;

namespace TestFu.Data
{
	/// <summary>
	/// A collection of <see cref="IDataGenerator"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IDataGeneratorCollection")]'
	///		/>
	public interface IDataGeneratorCollection  : ICollection
	{
		/// <summary>
		/// Gets the <see cref="IDataGenerator"/> associated to the
		/// <paramref name="column"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="column"/> is a null reference 
		/// (Nothing in Visual Basic)
		/// </exception>
		IDataGenerator this[DataColumn column]{get;}
		
		/// <summary>
		/// Gets the <see cref="IDataGenerator"/> associated to the column named
		/// <paramref name="columnName"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="columnName"/> is a null reference 
		/// (Nothing in Visual Basic)
		/// </exception>		
		IDataGenerator this[String columnName]{get;}

		/// <summary>
		/// Adds a <see cref="IDataGenerator"/> to the collection.
		/// </summary>
		/// <param name="dataGenerator">
		/// <see cref="IDataGenerator"/> to add to the collection.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="dataGenerator"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>
		void Add(IDataGenerator dataGenerator);
		
		/// <summary>
		/// Removes a <see cref="IDataGenerator"/> from the collection.
		/// </summary>
		/// <param name="dataGenerator">
		/// <see cref="IDataGenerator"/> to remove from the collection.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="dataGenerator"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>		
		void Remove(IDataGenerator dataGenerator);
		
		/// <summary>
		/// Removes a <see cref="IDataGenerator"/> associated to
		/// <paramref name="column"/> from the collection.
		/// </summary>
		/// <param name="column">
		/// <see cref="DataColumn"/> whose generator is to be removed from the collection.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="column"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>				
		void Remove(DataColumn column);
		
		/// <summary>
		/// Removes a <see cref="IDataGenerator"/> associated to
		/// <paramref name="column"/> from the collection.
		/// </summary>
		/// <param name="columnName">
		/// Column named <paramref name="columnName"/> whose generator is to be removed from the collection.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="columnName"/> is a null reference
		/// (Nothing in Visual Basic)
		/// </exception>						
		void Remove(String columnName);
		
		bool Contains(IDataGenerator dataGenerator);
		bool Contains(DataColumn column);
		bool Contains(String columnName);

		void Clear();
	}
}
