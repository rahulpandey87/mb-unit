using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
    /// <summary>
    /// Provides a row of values using in conjunction with <see cref="RowTestAttribute" />
    /// to bind values to the parameters of a row test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class RowAttribute : TestPatternAttribute
    {
        private Object[] row = null;
        private Type expectedException = null;

        /// <summary>
        /// Provides a row of values using in conjunction with <see cref="RowTestAttribute" />
        /// to bind values to the parameters of a row test method.
        /// </summary>
        /// <param name="row">The row of values to bind</param>
        public RowAttribute(params object[] row)
        {
            if (row == null)
            {
                // This resolves bug MBUNIT-45 that has to do with issues passing
                // null as a single parameter value.  Under other circumstances the
                // array reference could never be null.
                this.row = new object[] { null };
            }
            else
            {
                this.row = row;
            }
        }

        /// <summary>
        /// Gets or sets the type of exception that is expected to be thrown when this
        /// row is tested, or null if none.
        /// </summary>
        public Type ExpectedException
        {
            get
            {
                return this.expectedException;
            }
            set
            {
                this.expectedException = value;
            }
        }

        /// <summary>
        /// Gets the row of values.
        /// </summary>
        /// <returns>The row of values</returns>
        public Object[] GetRow()
        {
            return this.row;
        }
    }
}
