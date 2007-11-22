using System;
using System.Data;
using System.Collections;

namespace TestFu.Data
{
	/// <summary>
	/// A collection of <see cref="IForeignKeyProvider"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IForeignKeyProviderCollection")]'
	///		/>
	public interface IForeignKeyProviderCollection : ICollection
	{
		ITablePopulator TablePopulator{get;}

        IForeignKeyProvider this[ForeignKeyConstraint fk] { get;set;}

        void Add(IForeignKeyProvider fk);

		void Remove(IForeignKeyProvider fk);
		void Remove(ForeignKeyConstraint kk);
		
		bool Contains(IForeignKeyProvider fk);
		bool Contains(ForeignKeyConstraint fk);
		
		void Clear();
	}
}
