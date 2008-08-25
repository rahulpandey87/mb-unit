using System;
using System.Collections;

namespace MbUnit.Framework
{
    /// <summary>
    /// Interface defining a test suite
    /// </summary>
    /// <remarks>A test suite is a dynamic test fixture</remarks>
    public interface ITestSuite
    {
        string Name { get;}
        ICollection TestCases { get;}
    }
}
