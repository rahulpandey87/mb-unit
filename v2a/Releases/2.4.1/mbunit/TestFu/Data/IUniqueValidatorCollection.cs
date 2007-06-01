using System;
using System.Data;
using System.Collections;

namespace TestFu.Data
{
	/// <summary>
	/// A collection of <see cref="IUniqueValidator"/>.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IUniqueValidatorCollection")]'
	///		/>
	public interface IUniqueValidatorCollection : ICollection
	{
		ITablePopulator TablePopulator{get;}
		IUniqueValidator PrimaryKey {get;}

        IUniqueValidator this[UniqueConstraint unique] { get;set;}

        void Add(IUniqueValidator unique);
		
		void Remove(IUniqueValidator unique);
		void Remove(UniqueConstraint unique);
		
		bool Contains(IUniqueValidator unique);
		bool Contains(UniqueConstraint unique);
		
		void Clear();
	}
}
