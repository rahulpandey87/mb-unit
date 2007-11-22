using System;
using MbUnit.Framework;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RestoreDatabaseFirstAttribute : RestoreDatabaseAttribute
    {
        public RestoreDatabaseFirstAttribute()
            :base(TestSchedule.BeforeTest)
        { }
    }
}