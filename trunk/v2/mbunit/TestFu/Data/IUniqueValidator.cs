using System;
using System.Collections;
using System.Data;
namespace TestFu.Data
{	
	/// <summary>
	/// A validator for <see cref="UniqueConstraint"/> constraints.
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"IUniqueValidator")]'
	///		/>
	public interface IUniqueValidator
	{
        bool IsIdentity { get;}
        ITablePopulator Table{get;}
		UniqueConstraint Unique{get;}

        bool IsEmpty{get;}
		DataRow GetKey();
        void AddKey(DataRow row);
		void RemoveKey(DataRow row);
		bool Contains(DataRow row);
	}
}
