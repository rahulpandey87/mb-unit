using System;
using System.ComponentModel;

namespace MbUnit.Framework.ComponentModel
{
    public interface ITestDecoratorComponent  : IComponent
    {
        ITestSuite Decorate(ITestSuite suite);
    }
}
