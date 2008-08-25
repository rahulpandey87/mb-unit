using System;

namespace MbUnit.Framework
{
    /// <summary>
    /// Delegate used to identify test methods being wrapped in a <see cref="TestCase"/> object
    /// for inclusion in a <see cref="TestSuite"/>
    /// </summary>
    public delegate Object TestDelegate(Object context);
}
