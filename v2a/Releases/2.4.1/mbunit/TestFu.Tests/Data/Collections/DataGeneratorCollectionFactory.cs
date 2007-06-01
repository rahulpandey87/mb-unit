using System;
using MbUnit.Core.Framework;

namespace TestFu.Tests.Data.Collections
{
	using TestFu.Data.Collections;

	public class DataGeneratorCollectionFactory
	{
		private DataGeneratorCollection col=new DataGeneratorCollection();

        [Factory]
		public DataGeneratorCollection Empty
		{
			get
			{
				return new DataGeneratorCollection();
			}
		}
	}
}
