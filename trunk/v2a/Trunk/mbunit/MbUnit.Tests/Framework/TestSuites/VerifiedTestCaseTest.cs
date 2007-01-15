using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Framework.TestSuites
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="VerifiedTestCase"/> class.
    /// </summary>
    [TestFixture]
    public class VerifiedTestCaseTest
    {

        public Object Hello(Object context)
        {
            return context;
        }
    }
}

