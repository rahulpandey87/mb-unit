using System;
using MbUnit.Core.Framework;

namespace TestFu.Tests.Data
{
    public class DataSetFactory
    {
        [Factory]
        public UserOrderProductDataSet UserOrderProduct
        {
            get
            {
                UserOrderProductDataSet ds = new UserOrderProductDataSet();
                return ds;
            }
        }

        [Factory]
        public OneTableDataSet OneTable
        {
            get
            {
                return new OneTableDataSet();
            }
        }

        [Factory]
        public ParentChildDataSet ParentChild
        {
            get
            {
                return new ParentChildDataSet();
            }
        }
/*
        [Factory]
        public NorthwindDataSet Northwind
        {
            get
            {
                return new NorthwindDataSet();
            }
        }
*/
    }
}
