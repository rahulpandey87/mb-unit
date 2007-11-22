using System;

namespace TestFu.Data.Adapters
{
	public interface IDataAdapter
	{
		Object DataSource {get;}
		Type DataType {get;}
		int Count{ get;}
		Object this[int i] {get;}
	}
}
