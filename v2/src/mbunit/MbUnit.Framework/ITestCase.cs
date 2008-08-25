using System;
using System.Collections;

namespace MbUnit.Framework
{
    /// <summary>
    /// Interface defining a test case to be added to a test suite
    /// </summary>
    public interface ITestCase
    {
        string Name { get;}
        string Description { get;}
        Object Invoke(Object o, IList args);
    }
}
