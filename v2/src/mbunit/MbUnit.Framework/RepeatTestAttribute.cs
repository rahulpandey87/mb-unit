using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    /// <summary>
    /// This tag defines test method that will be repeated the specified number 
    /// of times.
    /// </summary>
	public sealed class RepeatTestAttribute : TestPatternAttribute
	{
        private int count;

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public RepeatTestAttribute(int count)
        {
            this.count = count;
        }
    }
}
