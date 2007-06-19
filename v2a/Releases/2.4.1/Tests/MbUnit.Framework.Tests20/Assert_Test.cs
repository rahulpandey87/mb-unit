using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Core.Exceptions;

namespace MbUnit.Framework.Tests20
{
    [TestFixture]
    public class Assert_Test
    {
        #region IsEmpty
        //NUnit Code
        [Test]
        public void IsEmpty()
        {
            GenericAssert.IsEmpty(new List<string>());
        }

        [Test, ExpectedException(typeof(AssertionException), "List expected to be empty")]
        public void IsEmptyFail()
        {
            List<string> arr = new List<string>();
            arr.Add("Testing");

            GenericAssert.IsEmpty(arr, "List");
        }

        #endregion

        #region IsNotEmpty

        [Test]
        public void IsNotEmpty()
        {
            List<string> arr = new List<string>();
            arr.Add("Testing");

            GenericAssert.IsNotEmpty(arr);
        }

        [Test, ExpectedException(typeof(AssertionException), "List expected not to be empty")]
        public void IsNotEmptyFail()
        {
            GenericAssert.IsNotEmpty(new List<string>(), "List");
        }

        #endregion
    }
}
