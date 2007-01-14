using System.ComponentModel;

namespace MbUnit.Framework.ComponentModel
{
    public interface ITestComponent : IComponent
    {
        [Category("Data")]
        string Name { get;set;}
        ITestSuite GetTests();
    }
}
