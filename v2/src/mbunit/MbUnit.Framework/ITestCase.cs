using System;
using System.Collections;

namespace MbUnit.Framework
{
    public interface ITestCase
    {
        string Name { get;}
        string Description { get;}
        Object Invoke(Object o, IList args);
    }
}
