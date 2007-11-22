using System;
using System.Collections;

namespace MbUnit.Framework
{
    public interface ITestSuite
    {
        string Name { get;}
        ICollection TestCases { get;}
    }
}
