using System;
using System.Collections;

namespace TestFu.Data.Adapters
{
	/// <summary>
	/// Static helper class for creating data binders
	/// </summary>	
	public sealed class DataAdapt
	{
		private DataAdapt(){}
		
		public static IDataAdapter From(Object dataSource)
		{
			if (dataSource==null)
				throw new ArgumentNullException("dataSource");
			
			// check if IList
			IList list = dataSource as IList;
			if (dataSource!=null)
				return new ListDataAdapter(list);
			
			throw new ArgumentException("Unsupported object type " + dataSource.GetType().Name);
		}
	}
}
