using System;
using System.Collections;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TestSuiteAttribute : PatternAttribute
    {
        public TestSuiteAttribute()
        {}
    }
}
