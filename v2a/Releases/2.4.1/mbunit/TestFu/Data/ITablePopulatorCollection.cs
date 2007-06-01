using System;
using System.Data;
using System.Collections;

namespace TestFu.Data
{
	/// <summary>
	/// A collection of <see cref="ITablePopulator"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"ITablePopulatorCollection")]'
	///		/>
	public interface ITablePopulatorCollection  : ICollection
	{
        ITablePopulator this[DataTable table] { get;set;}
        ITablePopulator this[string tableName] { get;set;}

        void Add(ITablePopulator tablePopulator);
		
		void Remove(ITablePopulator tablePopulator);
		void Remove(DataTable table);
		void Remove(String tableName);
		
		bool Contains(ITablePopulator tablePopulator);
		bool Contains(DataTable table);
		bool Contains(String tableName);
		
		void Clear();
	}
}
