using System;
using System.Data;
using System.Reflection;

namespace TestFu.Data.Populators
{
    public class MethodUniqueValidator : UniqueValidatorBase
    {
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        private MethodInfo method;

        public MethodUniqueValidator(ITablePopulator table, UniqueConstraint unique, MethodInfo method)
            :base(table,unique)
        {
            this.method = method;
        }

        public override void AddKey(DataRow row)
        {}

        public override bool Contains(DataRow row)
        {
            Object[] keys = new Object[this.Unique.Columns.Length];
            for (int i = 0;i<this.Unique.Columns.Length;++i)
            {
                keys[i] = row[ this.Unique.Columns[i] ];
            }

            Object idRow = this.method.Invoke(this.Table.Table, keys);
            return idRow !=null;
        }

        public override DataRow GetKey()
        {
            int index= this.rnd.Next( this.Table.Table.Rows.Count);
            return this.Table.Table.Rows[index];
        }

        public override bool IsEmpty
        {
            get 
            {
                return this.Table.Table.Rows.Count== 0;
            }
        }


        public override void RemoveKey(DataRow row)
        {}
    }
}