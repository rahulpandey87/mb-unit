using System.ComponentModel;

namespace MbUnit.Framework
{
    /// <summary>
    /// Interface defining a test component containing a test suite
    /// </summary>
    public interface ITestComponent : IComponent
    {
        [Category("Data")]
        string Name { get;set;}
        ITestSuite GetTests();
    }
}
