using System;
using System.Reflection;
using System.Collections;

namespace TestFu.Data.Adapters
{
	public class ListDataAdapter : IDataAdapter
	{
		private IList dataSource;
		private Type dataType;
		
		public ListDataAdapter(IList dataSource)
		{
			this.dataSource=dataSource;
			this.ReflectDataType();
		}
		
		public IList DataSource
		{
			get
			{
				return this.dataSource;
			}
		}
		
		Object IDataAdapter.DataSource 
		{
			get
			{
				return this.dataSource;
			}
		}
		
		public Type DataType 
		{
			get
			{
				return this.dataType;
			}
		}
		
		public int Count
		{ 
			get
			{
				return this.dataSource.Count;			
			}
		}
		
		public Object this[int i] 
		{
			get
			{
				return this.dataSource[i];
			}
		}
		
		protected virtual void ReflectDataType()
		{
			PropertyInfo pi = this.dataSource.GetType().GetProperty("Item");
			this.dataType = pi.PropertyType;
		}
	}
}
