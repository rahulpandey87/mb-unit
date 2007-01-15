using System;
using System.Collections.Generic;
using System.Text;

namespace MbUnit.Framework.Tests20.Reflection
{
    public class TestSample
    {
        private int counter = 0;
        private bool accessed = false;

        private void IncCounter()
        {
            counter++;
        }

        private void SetAsAccessed()
        {
            accessed = true;
        }
    }
}
