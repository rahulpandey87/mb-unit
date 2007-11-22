using System;
using MbUnit.Core.Framework;
using System.Reflection;
using MbUnit.Core;
using System.Globalization;

namespace MbUnit.Framework
{
    public enum SpecialValue
    {
        /// <summary>
        /// When used as parameter in a row test, it will be replaced by null (Nothing in VB).
        /// </summary>
        Null
    }

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

        /// <summary>
        /// Gets the row of values. Each one will be converted (if posible) to the type of
        /// the corresponding argument in the test method.
        /// </summary>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>The row of values.</returns>
        public Object[] GetRow(ParameterInfo[] parameters)
        {
            object[] args = new object[parameters.Length];

            // If the lengths are different a TargetParameterCountException exception will be thrown
            if (row.Length == parameters.Length)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    FormatParameter(parameters, args, i);
                }
                return args;
            }
            else if (parameters[parameters.Length - 1].ParameterType == typeof(Object[]))
            {
                // Test function has params object[] argument
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    FormatParameter(parameters, args, i);
                }
                if (row.Length - parameters.Length > 0)
                {
                    // Copy remaining parameters to object[]
                    object[] paramsArray = new object[row.Length - (parameters.Length - 1)];
                    Array.Copy(row, parameters.Length - 1, paramsArray, 0, row.Length - parameters.Length);
                    args[parameters.Length - 1] = paramsArray;
                }
                else
                {
                    args[parameters.Length - 1] = new object[0];
                }
                return args;
            }
            return row;
        }

        private void FormatParameter(ParameterInfo[] parameters, object[] args, int index)
        {
            // Skip nulls
            // If the item is SpecialValue.Null then return null
            if (row[index] == null || row[index] is SpecialValue)
            {
                args[index] = null;
            }
            else
            {
                // If the current item in the row is a Type and the test method is expecting
                // an instance, then create one
                if (row[index] == parameters[index].ParameterType)
                {
                    if (!parameters[index].ParameterType.IsEnum)
                    {
                        args[index] = TypeHelper.CreateInstance(parameters[index].ParameterType);
                    }
                }
                // Try to convert the type to the one expected by the test method
                else if ((row[index] as IConvertible) != null)
                {
                    IFormatProvider formatProvider = GetFormatProvider(parameters[index].ParameterType);
                    args[index] = Convert.ChangeType(row[index], parameters[index].ParameterType, formatProvider);
                }
                // Give up, the value is an array or a Type.  Leave it alone!
                else
                {
                    args[index] = row[index];
                }
            }
        }

        private IFormatProvider GetFormatProvider(Type t)
        {
            IFormatProvider formatProvider = null;
            if (IsNumericType(t))
            {
                formatProvider = NumberFormatInfo.InvariantInfo;
            }
            else if (t == typeof(DateTime))
            {
                formatProvider = DateTimeFormatInfo.InvariantInfo;
            }
            return formatProvider;
        }

        private bool IsNumericType(Type t)
        {
            return t == typeof(double) ||
                   t == typeof(float) ||
                   t == typeof(decimal) ||
                   t == typeof(int) ||
                   t == typeof(long) ||
                   t == typeof(short);
        }

    }
}
