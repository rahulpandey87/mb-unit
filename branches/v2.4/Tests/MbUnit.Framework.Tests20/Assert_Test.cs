using System;
using System.Collections.Generic;
using System.Text;

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
            GenericAssert.IsEmpty(new List<string>(), "Failed on empty List");
        }
        #endregion

        #region IsNotEmpty

        [Test]
        public void IsNotEmpty()
        {
            List<string> arr = new List<string>();
            arr.Add("Testing");

            GenericAssert.IsNotEmpty(arr, "Failed on non empty ArrayList");
       
        }
    
        #endregion
    }
}
