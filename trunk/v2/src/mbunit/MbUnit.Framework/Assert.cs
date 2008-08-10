// MbUnit Test Framework
//
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty.
//
// In no event will the authors be held liable for any damages arising from
// the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented;
//		you must not claim that you wrote the original software.
//		If you use this software in a product, an acknowledgment in the product
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source
//		distribution.
//
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using MbUnit.Core.Reports.Serialization;

using MbUnit.Core.Exceptions;

namespace MbUnit.Framework {
    /// <summary>
    /// Class containing generic assert methods for the comparison of values and object references, the existence of objects within a collection type and 
    /// basic object properties - for example, whether or not it is assignable to. Also contains a set of Fail asserts which will automatically fail a test
    /// straight away.
    /// </summary>
    public sealed class Assert {
        #region Static fields
        private static ArrayList warnings = ArrayList.Synchronized(new ArrayList());
        private static volatile int assertCount = 0;
        #endregion

        #region Private stuff
        /// <summary>
        /// The Equals method throws an <see cref="MbUnit.Core.Exceptions.AssertionException" />. This is done
        /// to make sure there is no mistake by calling this function. Use
        /// <see cref="AreEqual(object,object)">AreEqual</see> instead or one of its overloads.
        /// </summary>
        /// <param name="a">The first <see cref="System.Object"/> to compare</param>
        /// <param name="b">The second <see cref="System.Object"/> to compare</param>
        /// <returns>True if the values are equal, false otherwise</returns>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Always thrown as this method should not be used.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b) {
            throw new AssertionException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// Overrides the default <see cref="System.Object.ReferenceEquals"/> method inherited from <see cref="System.Object"/>
        /// to throw an <see cref="MbUnit.Core.Exceptions.AssertionException" /> instead. This is to ensure that there is no mistake in 
        /// calling this function as part of an Assert in your tests. Use <see cref="AreSame(object,object,string)">AreSame()</see>
        /// instead or one of its overloads. 
        /// </summary>
        /// <param name="a">The first <see cref="System.Object"/> to compare</param>
        /// <param name="b">The second <see cref="System.Object"/> to compare</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Always thrown as this method should not be used.</exception>
        public static new void ReferenceEquals(object a, object b) {
            throw new AssertionException("Assert.ReferenceEquals should not be used for Assertions");
        }

        /// <summary>
        /// Checks the type of the object, returning true if the object is a numeric type.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if the object is a numeric type</returns>
        static private bool IsNumericType(Object obj) {
            if (null != obj) {
                if (obj is byte) return true;
                if (obj is sbyte) return true;
                if (obj is decimal) return true;
                if (obj is double) return true;
                if (obj is float) return true;
                if (obj is int) return true;
                if (obj is uint) return true;
                if (obj is long) return true;
                if (obj is short) return true;
                if (obj is ushort) return true;

                if (obj is System.Byte) return true;
                if (obj is System.SByte) return true;
                if (obj is System.Decimal) return true;
                if (obj is System.Double) return true;
                if (obj is System.Single) return true;
                if (obj is System.Int32) return true;
                if (obj is System.UInt32) return true;
                if (obj is System.Int64) return true;
                if (obj is System.UInt64) return true;
                if (obj is System.Int16) return true;
                if (obj is System.UInt16) return true;
            }
            return false;
        }

        /// <summary>
        /// Used to compare numeric types.  Comparisons between
        /// same types are fine (Int32 to Int32, or Int64 to Int64),
        /// but the Equals method fails across different types.
        /// This method was added to allow any numeric type to
        /// be handled correctly, by using <c>ToString</c> and
        /// comparing the result
        /// </summary>
        /// <param name="expected">The first <see cref="System.Object"/> to compare</param>
        /// <param name="actual">The first <see cref="System.Object"/> to compare</param>
        /// <returns><c>True</c> or <c>False</c></returns>
        static internal bool ObjectsEqual(Object expected, Object actual) {
            if (IsNumericType(expected) &&
                IsNumericType(actual)) {
                //
                // Convert to strings and compare result to avoid
                // issues with different types that have the same
                // value
                //
                string sExpected = expected.ToString();
                string sActual = actual.ToString();
                return sExpected.Equals(sActual);
            } else if (ArrayAssert.IsArrayType(expected) && ArrayAssert.IsArrayType(actual)) {
                string failMessage;
                return CollectionAssert.ElementsEqual((IEnumerable)expected, (IEnumerable)actual, out failMessage);
            } else {
                return object.Equals(expected, actual);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// A private constructor disallows any instances of this object.
        /// </summary>
        private Assert() { }
        #endregion

        #region IsTrue, IsFalse
        /// <summary>
        /// Asserts that a <paramref name="condition"/> is true. If false, the method throws
        /// an <see cref="MbUnit.Core.Exceptions.AssertionException" /> with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, object[])">String.Format</see>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown when <paramref name="condition"/> is not true. </exception>
        /// <example>
        /// The following code example demonstrates a success (IsTrue_True) and a failed test (IsTrue_False) together with the exception's formatted message
        /// <code>
        /// using MbUnit.Framework;
        /// using System;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsTrue_True()
        ///       {
        ///          Assert.IsTrue(true, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsTrue_False()
        ///       {
        ///          Assert.IsTrue(false, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsTrue(bool)"/>
        /// <seealso cref="IsTrue(bool,string)"/>
        static public void IsTrue(bool condition, string format, params object[] args) {
            Assert.IncrementAssertCount();
            if (!condition)
                Assert.Fail(format, args);
        }

        /// <summary>
        /// Asserts that a <paramref name="condition"/> is true. If false, the method throws an <see cref="MbUnit.Core.Exceptions.AssertionException" /> 
        /// with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown when <paramref name="condition"/> is not true.</exception>
        /// <example>
        /// The following code example demonstrates a success (IsTrue_True) and a failed test (IsTrue_False) together with the exception's message
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsTrue_True()
        ///       {
        ///          Assert.IsTrue(true, "This test failed. Please get it working");
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsTrue_False()
        ///       {
        ///          Assert.IsTrue(false, "This test failed. Please get it working");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsTrue(bool)"/>
        /// <seealso cref="IsTrue(bool,string,object[])"/>
        static public void IsTrue(bool condition, string message) {
            Assert.IncrementAssertCount();
            if (!condition)
                Assert.Fail(message);
        }

        /// <summary>
        /// Asserts that a <paramref name="condition"/> is true. 
        /// If false, the method throws an <see cref="MbUnit.Core.Exceptions.AssertionException" /> with no explanatory message. 
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="condition"/> is not true. 
        /// </exception>
        /// <remarks>Use <see cref="IsTrue(bool,string)"/> or <see cref="IsTrue(bool,string,object[])"/> to specify a message for the exception if required.</remarks>
        /// <example>
        /// The following code example demonstrates a success (IsTrue_True) and a failed test (IsTrue_False) together with the exception's message
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsTrue_True()
        ///       {
        ///          Assert.IsTrue(true);
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsTrue_False()
        ///       {
        ///          Assert.IsTrue(false);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsTrue(bool,string)"/>
        /// <seealso cref="IsTrue(bool,string,object[])"/>
        static public void IsTrue(bool condition) {
            Assert.IsTrue(condition, string.Empty);
        }

        /// <summary>
        /// Asserts that a <paramref name="condition"/> is false. If true, the method throws
        /// an <see cref="MbUnit.Core.Exceptions.AssertionException"/> with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown when <paramref name="condition"/> is not false.</exception>
        /// <example>
        /// The following code example demonstrates a success (IsFalse_False) and a failed test (IsFalse_True) together with the exception's formatted message
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsFalse_False()
        ///       {
        ///          Assert.IsFalse(false, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsFalse_True()
        ///       {
        ///          Assert.IsFalse(true, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsFalse(bool)"/>
        /// <seealso cref="IsFalse(bool,string)"/>
        static public void IsFalse(bool condition, string format, params object[] args) {
            Assert.IncrementAssertCount();
            if (condition)
                Assert.Fail(format, args);
        }

        /// <summary>
        /// Asserts that a <paramref name="condition"/> is false. 
        /// If true, the method throws an <see cref="MbUnit.Core.Exceptions.AssertionException"/> with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown when <paramref name="condition"/> is not false. 
        /// </exception>
        /// <example>
        /// The following code example demonstrates a success (IsFalse_False) and a failed test (IsFalse_True) together with the exception's message
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsFalse_False()
        ///       {
        ///          Assert.IsFalse(false, "This test failed. Please get it working");
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsFalse_True()
        ///       {
        ///          Assert.IsFalse(true, "This test failed. Please get it working");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsFalse(bool)"/>
        /// <seealso cref="IsFalse(bool,string,object[])"/>
        static public void IsFalse(bool condition, string message) {
            Assert.IncrementAssertCount();
            if (condition)
                Assert.Fail(message);
        }

        /// <summary>
        /// Asserts that a <paramref name="condition"/> is false. 
        /// If true, the method throws an <see cref="MbUnit.Core.Exceptions.AssertionException"/> with no explanatory message. 
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown when <paramref name="condition"/> is not false. 
        /// </exception>
        /// <remarks>Use <see cref="IsFalse(bool,string)"/> or <see cref="IsFalse(bool,string,object[])"/> instead to specify a message for the exception</remarks>
        /// <example>
        /// The following code example demonstrates a success (IsFalse_False) and a failed test (IsFalse_True) together with the exception's message
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class Asserts
        ///    {
        ///       // This test succeeds
        ///       [Test]
        ///       public void IsFalse_False()
        ///       {
        ///          Assert.IsFalse(true);
        ///       }
        ///  
        ///       //This test fails
        ///       [Test]
        ///       public void IsFalse_True()
        ///       {
        ///          Assert.IsFalse(false);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IsFalse(bool,string)"/>
        /// <seealso cref="IsFalse(bool,string,object[])"/>
        static public void IsFalse(bool condition) {
            Assert.IsFalse(condition, string.Empty);
        }
        #endregion

        #region AreEqual

        #region Doubles
        /// <summary>
        /// Verifies that two doubles, <paramref name="expected"/> and <paramref name="actual"/>, 
        /// are equal given a <paramref name="delta"/>. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is
        /// thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> 
        /// are not values within the given <paramref name="delta"/>.</exception>
        /// <example>
        /// The following example demonstrates <c>Assert.AreEqual</c> using a different variety of finite and infinite values
        /// <code>
        /// using MbUnit.Framework;
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, 0.0d, "These values are not equal");
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_ValuesWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.1d, 0.2d, "These values are not equal");
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ValuesNotWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 2.0d, 0.2d, "These values are not equal");
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_OneValueIsInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.MaxValue, 1.0d, "These values are not equal");
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_BothValuesSameInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity, 1.0d, "These values are not equal");
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValuesOfInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.NegativeInfinity, 0.0d, "These values are not equal");
        ///       }
        /// 
        ///       //This test fails with a ArgumentException
        ///       [Test]
        ///       public void AreEqual_NegativeDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, -0.1d, "These values are not equal");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(double,double,double)" />
        /// <seealso cref="AreEqual(double,double,double,string,Object[])"/>
        static public void AreEqual(double expected, double actual, double delta, string message) {
            // Delta must be positive otherwise the following case fails
            if (delta < 0)
                throw new ArgumentException("delta", "Delta must be a positive value.");

            Assert.IncrementAssertCount();
            // handle infinity specially since subtracting two infinite values gives
            // NaN and the following test fails
            if (double.IsInfinity(expected)) {
                if (!(expected == actual))
                    Assert.FailNotEquals(expected, actual, message);
            } else if (!(Math.Abs(expected - actual) <= delta))
                Assert.FailNotEquals(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two doubles, <paramref name="expected"/> and <paramref name="actual"/>, are equal considering a <paramref name="delta"/>. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is
        /// thrown with no explanation for the failure. Use <see cref="AreEqual(double,double,double,string)"/> if you want to provide an explanation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not values within the given <paramref name="delta"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of finite and infinite values
        /// <code>
        /// using MbUnit.Framework;
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, 0.0d);
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_ValuesWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.1d, 0.2d);
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ValuesNotWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 2.0d, 0.2d);
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_OneValueIsInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.MaxValue, 1.0d);
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_BothValuesSameInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity, 1.0d);
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValuesOfInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.NegativeInfinity, 0.0d);
        ///       }
        /// 
        ///       //This test fails with an ArgumentException
        ///       [Test]
        ///       public void AreEqual_NegativeDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, -0.1d);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(double,double,double,string)"/>
        /// <seealso cref="AreEqual(double,double,double,string,object[])"/>
        static public void AreEqual(double expected, double actual, double delta) {
            Assert.AreEqual(expected, actual, delta, string.Empty);
        }

        /// <summary>
        /// Verifies that two doubles, <paramref name="expected"/> and <paramref name="actual"/>, are equal considering a <paramref name="delta"/>. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, Object[])"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not values within the given <paramref name="delta"/>.
        /// Exception message is generated through <paramref name="format"/> and <paramref name="args"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of finite and infinite values
        /// <code>
        /// using MbUnit.Framework;
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, 0.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_ValuesWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.1d, 0.2d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ValuesNotWithinDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 2.0d, 0.2d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_OneValueIsInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.MaxValue, 1.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test passes
        ///       [Test]
        ///       public void AreEqual_BothValuesSameInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity, 1.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValuesOfInfinity()
        ///       {
        ///          Assert.AreEqual(double.PositiveInfinity, double.NegativeInfinity, 0.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        /// 
        ///       //This test fails with an ArgumentException
        ///       [Test]
        ///       public void AreEqual_NegativeDelta()
        ///       {
        ///          Assert.AreEqual(1.0d, 1.0d, -0.1d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(double,double,double,string)"/>
        /// <seealso cref="AreEqual(double,double,double)"/>
        static public void AreEqual(double expected, double actual, double delta, string format, params object[] args) {
            AreEqual(expected, actual, delta, String.Format(format, args));
        }

        #endregion

        #region floats
        /// <summary>
        /// Verifies that two floats, <paramref name="expected"/> and <paramref name="actual"/>, 
        /// are equal considering a <paramref name="delta"/>. If the
        /// <paramref name="expected"/> value is infinity then the <paramref name="delta"/> value is ignored. If
        /// they are not equal then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is
        /// thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> 
        /// are not values within the given <paramref name="delta"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of finite and infinite values
        /// <code>
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreEqualTests
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreEqual_SameValues()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, 0.0f, "These values are not equal");
        ///      }
        ///      
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_ValuesWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.1f, 0.2f, "These values are not equal");
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_ValuesNotWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 2.0f, 0.2f, "These values are not equal");
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_OneValueIsInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.MaxValue, 1.0d, "These values are not equal");
        ///      }
        ///
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_BothValuesSameInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.PositiveInfinity, 1.0d, "These values are not equal");
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_DifferentValuesOfInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.NegativeInfinity, 0.0d, "These values are not equal");
        ///      }
        ///
        ///      //This test fails with a ArgumentException
        ///      [Test]
        ///      public void AreEqual_NegativeDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, -0.1f, "These values are not equal");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(float,float,float)"/>
        /// <seealso cref="AreEqual(float,float,float,string,Object[])"/>
        static public void AreEqual(float expected, float actual, float delta, string message) {
            // Delta must be positive otherwise the following case fails
            if (delta < 0)
                throw new ArgumentException("delta", "Delta must be a positive value.");

            Assert.IncrementAssertCount();
            // handle infinity specially since subtracting two infinite values gives
            // NaN and the following test fails
            if (float.IsInfinity(expected)) {
                if (!(expected == actual))
                    Assert.FailNotEquals(expected, actual, message);
            } else if (!(Math.Abs(expected - actual) <= delta))
                Assert.FailNotEquals(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two floats, <paramref name="expected"/> and <paramref name="actual"/>, are equal considering a <paramref name="delta"/>. If the
        /// <paramref name="expected"/> value is infinity then the <paramref name="delta"/> value is ignored. If
        /// they are not equals then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, Object[])" />.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not values within the given <paramref name="delta"/>.
        /// Exception message is generated through <paramref name="format"/> and <paramref name="args"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of finite and infinite values
        /// <code>
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreEqualTests
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreEqual_SameValues()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, 0.0f, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///      
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_ValuesWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.1f, 0.2f, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_ValuesNotWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 2.0f, 0.2f, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_OneValueIsInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.MaxValue, 1.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_BothValuesSameInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.PositiveInfinity, 1.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_DifferentValuesOfInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.NegativeInfinity, 0.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test fails with a ArgumentException
        ///      [Test]
        ///      public void AreEqual_NegativeDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, -0.1f, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(float,float,float)"/>
        /// <seealso cref="AreEqual(float,float,float,string)"/>
        static public void AreEqual(float expected, float actual, float delta, string format, params object[] args) {
            AreEqual(expected, actual, delta, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that two floats, <paramref name="expected"/> and <paramref name="actual"/>, 
        /// are equal considering a <paramref name="delta"/>. If the
        /// <paramref name="expected"/> value is infinity then the <paramref name="delta"/> value is ignored. If
        /// they are not equals then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between <paramref name="expected"/> and <paramref name="actual"/></param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="delta"/> has been given a negative value.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> 
        /// are not values within the given <paramref name="delta"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of finite and infinite values
        /// <code>
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreEqualTests
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreEqual_SameValues()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, 0.0f);
        ///      }
        ///      
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_ValuesWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.1f, 0.2f);
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_ValuesNotWithinDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 2.0f, 0.2f);
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_OneValueIsInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.MaxValue, 1.0d);
        ///      }
        ///
        ///      //This test passes
        ///      [Test]
        ///      public void AreEqual_BothValuesSameInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.PositiveInfinity, 1.0d);
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreEqual_DifferentValuesOfInfinity()
        ///      {
        ///         Assert.AreEqual(float.PositiveInfinity, float.NegativeInfinity, 0.0d);
        ///      }
        ///
        ///      //This test fails with a ArgumentException
        ///      [Test]
        ///      public void AreEqual_NegativeDelta()
        ///      {
        ///         Assert.AreEqual(1.0f, 1.0f, -0.1f);
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(float,float,float,string)"/>
        /// <seealso cref="AreEqual(float,float,float,string,object[])"/>
        static public void AreEqual(float expected, float actual, float delta) {
            Assert.AreEqual(expected, actual, delta, string.Empty);
        }

        #endregion

        #region decimals

        /// <summary>
        /// Verifies that two decimals, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException"><paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0m, 1.0m, "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(decimal.Zero, decimal.MaxValue, "These values are not equal");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(decimal,decimal)"/>
        /// <seealso cref="AreEqual(decimal,decimal,string,Object[])"/>
        static public void AreEqual(decimal expected, decimal actual, string message) {
            Assert.IncrementAssertCount();
            if (!(expected == actual))
                Assert.FailNotEquals(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two decimals, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0m, 1.0m, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(decimal.Zero, decimal.MaxValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(decimal,decimal)"/>
        /// <seealso cref="AreEqual(decimal,decimal,string)"/>
        static public void AreEqual(decimal expected, decimal actual, string format, params object[] args) {
            Assert.IncrementAssertCount();
            if (!(expected == actual))
                Assert.FailNotEquals(expected, actual, format, args);
        }

        /// <summary>
        /// Verifies that two decimals, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown. 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1.0m, 1.0m);
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(decimal.Zero, decimal.MaxValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(decimal,decimal,string)"/>
        /// <seealso cref="AreEqual(decimal,decimal,string,object[])"/>
        static public void AreEqual(decimal expected, decimal actual) {
            Assert.AreEqual(expected, actual, string.Empty);
        }

        #endregion

        #region integers

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1, 1, "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(int.MaxValue, int.MinValue, "These values are not equal");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(int,int)"/>
        /// <seealso cref="AreEqual(int,int,string,Object[])"/>
        static public void AreEqual(int expected, int actual, string message) {
            Assert.IncrementAssertCount();
            if (!(expected == actual))
                Assert.FailNotEquals(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1, 1, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(int.MaxValue, int.MinValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(int,int)"/>
        /// <seealso cref="AreEqual(int,int,string)"/>
        static public void AreEqual(int expected, int actual, string format, params object[] args) {
            Assert.IncrementAssertCount();
            if (!(expected == actual))
                Assert.FailNotEquals(expected, actual, format, args);
        }

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown. 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual(1, 1);
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual(int.MaxValue, int.MinValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AreEqual(int,int,string)"/>
        /// <seealso cref="AreEqual(int,int,string,object[])"/>
        static public void AreEqual(int expected, int actual) {
            Assert.AreEqual(expected, actual, string.Empty);
        }

        #endregion

        #region objects
        /// <summary>
        /// Verifies that two objects, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, Object[])" />.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">
        /// Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Hello", "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Goodbye", "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ExpectedIsNull()
        ///       {
        ///          Assert.AreEqual(null, "Hello", "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ActualIsNull()
        ///       {
        ///          Assert.AreEqual("Hello", null, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_BothAreNull()
        ///       {
        ///          Assert.AreEqual(null, null, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_EqualsNotDefinedForObject()
        ///       {
        ///          Assert.AreEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"), "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        ///
        ///    public class TestObject
        ///    {
        ///       public string city;
        ///       public string state;
        ///
        ///       public TestObject(string a, string b)
        ///       {
        ///          city = a;
        ///          state = b;
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// In this case, the two objects are considered equal if either
        /// <list type="bullet">
        /// <item>Both objects are null</item>
        /// <item>Both objects have the same value when compared using their <c>Equals</c> method.</item>
        /// </list>
        /// If the object does not have an Equals method defined for it, a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown. 
        /// Compare and contrast this with <see cref="AreSame(Object,Object,string,object[])"/> which returns true if <paramref name="expected"/> and <paramref name="actual"/>
        /// are references to the same object rather than two objects which are equal in some defined way.
        /// </remarks>
        /// <seealso cref="AreEqual(Object,Object)"/>
        /// <seealso cref="AreEqual(Object,Object,string)"/>
        static public void AreEqual(Object expected, Object actual, string format, params object[] args) {
            AreEqual(expected, actual, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that two objects, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown with a given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Hello", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Goodbye", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ExpectedIsNull()
        ///       {
        ///          Assert.AreEqual(null, "Hello", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ActualIsNull()
        ///       {
        ///          Assert.AreEqual("Hello", null, "These values are not equal");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_BothAreNull()
        ///       {
        ///          Assert.AreEqual(null, null, "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_EqualsNotDefinedForObject()
        ///       {
        ///          Assert.AreEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"), "These values are not equal");
        ///       }
        ///    }
        ///
        ///    public class TestObject
        ///    {
        ///       public string city;
        ///       public string state;
        ///
        ///       public TestObject(string a, string b)
        ///       {
        ///          city = a;
        ///          state = b;
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// In this case, the two objects are considered equal if either
        /// <list type="bullet">
        /// <item>Both objects are null</item>
        /// <item>Both objects have the same value when compared using their <c>Equals</c> method.</item>
        /// </list>
        /// If the object does not have Equals defined for it, a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown.
        /// Compare and contrast this with <see cref="AreSame(Object,Object,string)"/> which returns true if <paramref name="expected"/> and <paramref name="actual"/>
        /// are references to the same object rather than two objects which are equal in some defined way.
        /// </remarks>
        /// <seealso cref="AreEqual(Object,Object)"/>
        /// <seealso cref="AreEqual(Object,Object,string,object[])"/>
        static public void AreEqual(Object expected, Object actual, string message) {
            Assert.IncrementAssertCount();
            if (expected == null && actual == null) return;

            if (expected != null && actual != null) {
                if (ObjectsEqual(expected, actual)) {
                    return;
                }
            }
            Assert.FailNotEquals(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two objects, <paramref name="expected"/> and <paramref name="actual"/>, are equal.
        /// If they are not equal then an <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <exception cref="MbUnit.Core.Exceptions.NotEqualAssertionException"><paramref name="expected"/> and <paramref name="actual"/> are not equal.</exception>
        /// <example>
        /// The following example demonstrates Assert.AreEqual using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreEqualTests
        ///    {
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_SameValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Hello", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_DifferentValues()
        ///       {
        ///          Assert.AreEqual("Hello", "Goodbye", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ExpectedIsNull()
        ///       {
        ///          Assert.AreEqual(null, "Hello", "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_ActualIsNull()
        ///       {
        ///          Assert.AreEqual("Hello", null, "These values are not equal");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreEqual_BothAreNull()
        ///       {
        ///          Assert.AreEqual(null, null, "These values are not equal");
        ///       }
        ///
        ///       // This test fails with a NotEqualAssertionException
        ///       [Test]
        ///       public void AreEqual_EqualsNotDefinedForObject()
        ///       {
        ///          Assert.AreEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"), "These values are not equal");
        ///       }
        ///    }
        ///
        ///    public class TestObject
        ///    {
        ///       public string city;
        ///       public string state;
        ///
        ///       public TestObject(string a, string b)
        ///       {
        ///          city = a;
        ///          state = b;
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// In this case, the two objects are considered equal if either
        /// <list type="bullet">
        /// <item>Both objects are null</item>
        /// <item>Both objects have the same value when compared using their <c>Equals</c> method.</item>
        /// </list>
        /// If the object does not have Equals defined for it, a <see cref="MbUnit.Core.Exceptions.NotEqualAssertionException"/> is thrown.
        /// Compare and contrast this with <see cref="AreSame(Object,Object)"/> which returns true if <paramref name="expected"/> and <paramref name="actual"/>
        /// are references to the same object rather than two objects which are equal in some defined way.      
        /// </remarks>
        /// <seealso cref="AreEqual(Object,Object,string)"/>
        /// <seealso cref="AreEqual(Object,Object,string,object[])"/>
        static public void AreEqual(Object expected, Object actual) {
            Assert.AreEqual(expected, actual, string.Empty);
        }

        /// <summary>
        /// Verifies that given two objects, <paramref name="expected"/> and <paramref name="actual"/>, 
        /// the property described by the <see cref="System.Reflection.PropertyInfo" /> object <paramref name="pi"/>
        /// is present in both objects, is not null, and that the value of the property in both objects is equal.
        /// </summary>
        /// <param name="pi">The <see cref="System.Reflection.PropertyInfo" /> object indicating the property to be tested</param>
        /// <param name="expected">The object containing the expected value of the property</param>
        /// <param name="actual">The object containing the actual value of the property</param>
        /// <param name="indices">The index of the value in the property if it is an indexed property</param>
        /// <exception cref="AssertionException">One or both of <paramref name="expected"/> and <paramref name="actual"/> are null</exception>
        /// <exception cref="AssertionException">The property that <paramref name="pi"/> describes is not present in either <paramref name="expected"/> or <paramref name="actual"/></exception>
        /// <exception cref="NotEqualAssertionException">The property values in <paramref name="expected"/> and <paramref name="actual"/> are not equal</exception>
        /// <remarks>Uses the <see cref="System.Reflection.PropertyInfo.GetValue(Object, Object[])" /> method to retrieve the actual values for comparison</remarks>
        /// <example>
        /// <para>The following example demonstrates <c>Assert.AreValueEqual()</c> in five different ways showing its various pass and fail scenarios.</para> 
        /// <para>NB: <c>AreValueEqual_PropertyNotPresentInOneObject()</c> below fails because the <see cref="System.String"/> class has a Chars
        /// property while the string value type does not. (One capitalised letter makes all the difference)</para>
        /// <code>
        ///using System;
        ///using System.Reflection;
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///   [TestFixture]
        ///   public class AreValueEqualTests
        ///   {
        ///      //This test passes
        ///      [Test]
        ///      public void AreValueEqual_SameValues()
        ///      {
        ///         // Create two arrays
        ///         String[] a = new String[4] { "this", "is", "a", "test" };
        ///         String[] b = new String[4] { "this", "is", "a", "camel" };
        ///
        ///         // Generate the PropertyInfo object for an array's length
        ///         PropertyInfo pi = typeof(Array).GetProperty("Length");
        ///
        ///         Assert.AreValueEqual(pi, a, b);
        ///      }
        ///
        ///      //This test passes
        ///      [Test]
        ///      public void AreValueEqual_SameValuesUsingIndices()
        ///      {
        ///         // Create two strings
        ///         String a = "this is a test";
        ///         String b = "this is a camel";
        ///
        ///         // Generate the PropertyInfo object for the string as a Char array 
        ///         PropertyInfo pi = typeof(String).GetProperty("Chars");
        ///
        ///         // Test the fourth letter
        ///         Assert.AreValueEqual(pi, a, b, new Object[] {4});
        ///      }
        ///
        ///      // This test fails with an AssertionException
        ///      [Test]
        ///      public void AreValueEqual_OneValueIsNull()
        ///      {
        ///         // Create two arrays
        ///         String[] a = new String[4] { "this", "is", "a", "test" };
        ///
        ///         // Generate the PropertyInfo object for an array's length
        ///         PropertyInfo pi = typeof(Array).GetProperty("Length");
        ///
        ///         Assert.AreValueEqual(pi, a, null);
        ///      }
        ///
        ///      //This test fails with an AssertionException
        ///      [Test]
        ///      public void AreValueEqual_PropertyNotPresentInOneObject()
        ///      {
        ///         // Create two arrays
        ///         String[] a = new String[4] { "this", "is", "a", "test" };
        ///         string[] b = new string[4] { "this", "is", "a", "camel" };
        ///
        ///         // Generate the PropertyInfo object for an array's length
        ///         PropertyInfo pi = typeof(Array).GetProperty("Chars");
        ///
        ///         Assert.AreValueEqual(pi, a, b, new Object[] {4});
        ///      }
        ///
        ///      //This test fails with a NotEqualAssertionException
        ///      [Test]
        ///      public void AreValueEqual_DifferentValues()
        ///      {
        ///         // Create two strings
        ///         String a = "this is a test";
        ///         String b = "this is a camel";
        ///
        ///         // Generate the PropertyInfo object for the string as a Char array 
        ///         PropertyInfo pi = typeof(String).GetProperty("Chars");
        ///
        ///         // Test the tenth letter
        ///         Assert.AreValueEqual(pi, a, b, new Object[] { 10 });
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreValueEqual(PropertyInfo pi, Object expected, Object actual, params Object[] indices) {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            Assert.IncrementAssertCount();
            // check types
            if (!pi.DeclaringType.IsAssignableFrom(expected.GetType()))
                Assert.Fail("Property declaring type does not match with expected type");
            if (!pi.DeclaringType.IsAssignableFrom(actual.GetType()))
                Assert.Fail("Property declaring type does not match with expected type");

            Assert.AreEqual(pi.GetValue(expected, indices),
                            pi.GetValue(actual, indices),
                            String.Format("{0}.{1} property does not have the same value",
                                          pi.DeclaringType.Name,
                                          pi.Name
                                          )
                            );
        }

        #endregion

        #endregion

        #region AreNotEqual

        #region Objects

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="Object"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <remarks>
        /// In this context, two objects are considered not equal if
        /// <list type="bullet">
        /// <item>One of <paramref name="expected"/> and <paramref name="actual"/> are null - but not both</item>
        /// <item>The objects are numeric and their string representation are not equal</item>
        /// <item>The objects are arrays and their dimensions and contents are not equal</item>
        /// <item>Their values according to their Equals() method are not equal. 
        ///   <b>NB: If a class does not define an Equals method, two objects of that class will always be inequal even if they have the same value</b>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="AreNotEqual(Object,Object)"/>
        /// <seealso cref="AreNotEqual(Object,Object,String)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotEqualObjects
        ///   {
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectsNotEqual()
        ///      {
        ///         Assert.AreNotEqual("Test", "Exam",  "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      // This test fails : objects are equal
        ///      [Test]
        ///      public void AreNotEqual_EqualObjects()
        ///      {
        ///         Assert.AreNotEqual("Test", "Test",  "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test passes : expected value is null
        ///      [Test]
        ///      public void AreNotEqual_ExpectedIsNull()
        ///      {
        ///         Assert.AreNotEqual(null, "Test",  "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test passes : actual value is null
        ///      [Test]
        ///      public void AreNotEqual_ActualIsNull()
        ///      {
        ///         Assert.AreNotEqual("Test", null,  "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      //This test fails : both values are null
        ///      [Test]
        ///      public void AreNotEqual_BothObjectsAreNull()
        ///      {
        ///         Assert.AreNotEqual(null, null, "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///
        ///      // This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectHasNoEqualsMethod()
        ///      {
        ///         Assert.AreNotEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"), "Test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///   }
        ///
        ///   public class TestObject
        ///   {
        ///      public string city;
        ///      public string state;
        ///
        ///      public TestObject(string a, string b)
        ///      {
        ///         city = a;
        ///         state = b;
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreNotEqual(Object expected, Object actual, string format, params object[] args) {
            AreNotEqual(expected, actual, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="Object"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are both null</exception>
        /// <remarks>
        /// In this context, two objects are considered not equal if
        /// <list type="bullet">
        /// <item>One of <paramref name="expected"/> and <paramref name="actual"/> are null - but not both</item>
        /// <item>The objects are numeric and their string representation are not equal</item>
        /// <item>The objects are arrays and their dimensions and contents are not equal</item>
        /// <item>Their values according to their Equals() method are not equal. 
        ///   <b>NB: If a class does not define an Equals method, two objects of that class will always be inequal even if they have the same value</b>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="AreNotEqual(Object,Object)"/>
        /// <seealso cref="AreNotEqual(Object,Object,String,Object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotEqualObjects
        ///   {
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectsNotEqual()
        ///      {
        ///         Assert.AreNotEqual("Test", "Exam", "This test failed");
        ///      }
        ///
        ///      // This test fails : objects are equal
        ///      [Test]
        ///      public void AreNotEqual_EqualObjects()
        ///      {
        ///         Assert.AreNotEqual("Test", "Test", "This test failed");
        ///      }
        ///
        ///      //This test passes : expected value is null
        ///      [Test]
        ///      public void AreNotEqual_ExpectedIsNull()
        ///      {
        ///         Assert.AreNotEqual(null, "Test", "This test failed");
        ///      }
        ///
        ///      //This test passes : actual value is null
        ///      [Test]
        ///      public void AreNotEqual_ActualIsNull()
        ///      {
        ///         Assert.AreNotEqual("Test", null, "This test failed");
        ///      }
        ///
        ///      //This test fails : both values are null
        ///      [Test]
        ///      public void AreNotEqual_BothObjectsAreNull()
        ///      {
        ///         Assert.AreNotEqual(null, null, "This test failed");
        ///      }
        ///
        ///      // This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectHasNoEqualsMethod()
        ///      {
        ///         Assert.AreNotEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"), "This test failed");
        ///      }
        ///   }
        ///
        ///   public class TestObject
        ///   {
        ///      public string city;
        ///      public string state;
        ///
        ///      public TestObject(string a, string b)
        ///      {
        ///         city = a;
        ///         state = b;
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreNotEqual(Object expected, Object actual, string message) {
            Assert.IncrementAssertCount();

            if (expected == null ^ actual == null)
                return;

            if (ObjectsEqual(expected, actual))
                Assert.FailSame(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="Object"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are both null</exception>
        /// <remarks>
        /// In this context, two objects are considered not equal if
        /// <list type="bullet">
        /// <item>One of <paramref name="expected"/> and <paramref name="actual"/> are null - but not both</item>
        /// <item>The objects are numeric and their string representation are not equal</item>
        /// <item>The objects are arrays and their dimensions and contents are not equal</item>
        /// <item>Their values according to their Equals() method are not equal. 
        ///   <b>NB: If a class does not define an Equals method, two objects of that class will always be inequal even if they have the same value</b>
        /// </item>
        /// </list>
        /// </remarks>
        /// <seealso cref="AreNotEqual(Object,Object,string)"/>
        /// <seealso cref="AreNotEqual(Object,Object,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotEqualObjects
        ///   {
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectsNotEqual()
        ///      {
        ///         Assert.AreNotEqual("Test", "Exam");
        ///      }
        ///
        ///      // This test fails : objects are equal
        ///      [Test]
        ///      public void AreNotEqual_EqualObjects()
        ///      {
        ///         Assert.AreNotEqual("Test", "Test");
        ///      }
        ///
        ///      //This test passes : expected value is null
        ///      [Test]
        ///      public void AreNotEqual_ExpectedIsNull()
        ///      {
        ///         Assert.AreNotEqual(null, "Test");
        ///      }
        ///
        ///      //This test passes : actual value is null
        ///      [Test]
        ///      public void AreNotEqual_ActualIsNull()
        ///      {
        ///         Assert.AreNotEqual("Test", null);
        ///      }
        ///
        ///      //This test fails : both values are null
        ///      [Test]
        ///      public void AreNotEqual_BothObjectsAreNull()
        ///      {
        ///         Assert.AreNotEqual(null, null);
        ///      }
        ///
        ///      // This test passes
        ///      [Test]
        ///      public void AreNotEqual_ObjectHasNoEqualsMethod()
        ///      {
        ///         Assert.AreNotEqual(new TestObject("London", "drunk"), new TestObject("London", "drunk"));
        ///      }
        ///   }
        ///
        ///   public class TestObject
        ///   {
        ///      public string city;
        ///      public string state;
        ///
        ///      public TestObject(string a, string b)
        ///      {
        ///         city = a;
        ///         state = b;
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreNotEqual(Object expected, Object actual) {
            Assert.AreNotEqual(expected, actual, string.Empty);
        }

        #endregion

        #region Ints

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the integers are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The integer to compare</param>
        /// <param name="actual">The integer being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(int,int)"/>
        /// <seealso cref="AreNotEqual(int,int,string)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1, 1, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(int.MaxValue, int.MinValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(int expected, int actual, string format, params object[] args) {
            if (actual == expected)
                if (args != null)
                    Assert.FailSame(expected, actual, format, args);
                else
                    Assert.FailSame(expected, actual, format);
        }

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the integers are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with a given <paramref name="message"/>.
        /// </summary>
        /// <param name="expected">The integer to compare</param>
        /// <param name="actual">The integer being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(int,int)"/>
        /// <seealso cref="AreNotEqual(int,int,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1, 1, "This test failed");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(int.MaxValue, int.MinValue, "This test failed");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(int expected, int actual, string message) {
            Assert.AreNotEqual(expected, actual, message, null);
        }

        /// <summary>
        /// Verifies that two integers, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the integers are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// </summary>
        /// <param name="expected">The integer to compare</param>
        /// <param name="actual">The integer being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(int,int,string)"/>
        /// <seealso cref="AreNotEqual(int,int,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1, 1);
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(int.MaxValue, int.MinValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(int expected, int actual) {
            Assert.AreNotEqual(expected, actual, string.Empty, null);
        }
        #endregion

        #region UInts

        /// <summary>
        /// Verifies that two <see cref="uint"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="uint"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="uint"/> to compare</param>
        /// <param name="actual">The <see cref="uint"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(uint,uint)"/>
        /// <seealso cref="AreNotEqual(uint,uint,string)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualUnsignedIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1u, 1u, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(UInt32.MaxValue, UInt32.MinValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(uint expected, uint actual, string format, params object[] args) {
            if (actual == expected)
                if (args != null)
                    Assert.FailSame(expected, actual, format, args);
                else
                    Assert.FailSame(expected, actual, format);
        }

        /// <summary>
        /// Verifies that two <see cref="uint"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="uint"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="uint"/> to compare</param>
        /// <param name="actual">The <see cref="uint"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(uint,uint)"/>
        /// <seealso cref="AreNotEqual(uint,uint,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualUnsignedIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1u, 1u, "Test failed");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(UInt32.MaxValue, UInt32.MinValue, "Test failed");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(uint expected, uint actual, string message) {
            Assert.AreNotEqual(expected, actual, message, null);
        }

        /// <summary>
        /// Verifies that two <see cref="uint"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="uint"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The <see cref="uint"/> to compare</param>
        /// <param name="actual">The <see cref="uint"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(uint,uint,string)"/>
        /// <seealso cref="AreNotEqual(uint,uint,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualUnsignedIntegers
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1u, 1u);
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(UInt32.MaxValue, UInt32.MinValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(uint expected, uint actual) {
            Assert.AreNotEqual(expected, actual, string.Empty, null);
        }
        #endregion

        #region Decimals
        /// <summary>
        /// Verifies that two <see cref="decimal"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="decimal"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="decimal"/> to compare</param>
        /// <param name="actual">The <see cref="decimal"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(decimal,decimal)"/>
        /// <seealso cref="AreNotEqual(decimal,decimal,string)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDecimals
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0m, 1.0m, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(decimal.MaxValue, decimal.Zero, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(decimal expected, decimal actual, string format, params object[] args) {
            if (actual == expected)
                if (args != null)
                    Assert.FailSame(expected, actual, format, args);
                else
                    Assert.FailSame(expected, actual, format);
        }

        /// <summary>
        /// Verifies that two <see cref="decimal"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="decimal"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="decimal"/> to compare</param>
        /// <param name="actual">The <see cref="decimal"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(decimal,decimal)"/>
        /// <seealso cref="AreNotEqual(decimal,decimal,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDecimals
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0m, 1.0m, "Test failed");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(decimal.MaxValue, decimal.Zero, "Test failed");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(decimal expected, decimal actual, string message) {
            Assert.AreNotEqual(expected, actual, message, null);
        }

        /// <summary>
        /// Verifies that two <see cref="decimal"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="decimal"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// </summary>
        /// <param name="expected">The <see cref="decimal"/> to compare</param>
        /// <param name="actual">The <see cref="decimal"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(decimal,decimal,string)"/>
        /// <seealso cref="AreNotEqual(decimal,decimal,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDecimals
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0m, 1.0m);
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(decimal.MaxValue, decimal.Zero);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(decimal expected, decimal actual) {
            Assert.AreNotEqual(expected, actual, string.Empty, null);
        }
        #endregion

        #region Floats
        /// <summary>
        /// Verifies that two <see cref="float"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="float"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="float"/> to compare</param>
        /// <param name="actual">The <see cref="float"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(float,float)"/>
        /// <seealso cref="AreNotEqual(float,float,string)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualFloats
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0f, 1.0f, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(float.MaxValue, float.MinValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(float expected, float actual, string format, params object[] args) {
            if (actual == expected)
                if (args != null)
                    Assert.FailSame(expected, actual, format, args);
                else
                    Assert.FailSame(expected, actual, format);
        }

        /// <summary>
        /// Verifies that two <see cref="float"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="float"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="float"/> to compare</param>
        /// <param name="actual">The <see cref="float"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(float,float)"/>
        /// <seealso cref="AreNotEqual(float,float,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualFloats
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0f, 1.0f, "Test failed");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(float.MaxValue, float.MinValue, "Test failed");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(float expected, float actual, string message) {
            Assert.AreNotEqual(expected, actual, message, null);
        }

        /// <summary>
        /// Verifies that two <see cref="float"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="float"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// </summary>
        /// <param name="expected">The <see cref="float"/> to compare</param>
        /// <param name="actual">The <see cref="float"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(float,float,string)"/>
        /// <seealso cref="AreNotEqual(float,float,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualFloats
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0f, 1.0f);
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(float.MaxValue, float.MinValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(float expected, float actual) {
            Assert.AreNotEqual(expected, actual, string.Empty, null);
        }
        #endregion

        #region Doubles
        /// <summary>
        /// Verifies that two <see cref="double"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="double"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="double"/> to compare</param>
        /// <param name="actual">The <see cref="double"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(double,double)"/>
        /// <seealso cref="AreNotEqual(double,double,string)"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDoubles
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0d, 1.0d, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(double.MaxValue, double.MinValue, "Test failed at {0}", DateTime.Now.ToString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(double expected, double actual, string format, params object[] args) {
            if (actual == expected)
                if (args != null)
                    Assert.FailSame(expected, actual, format, args);
                else
                    Assert.FailSame(expected, actual, format);
        }

        /// <summary>
        /// Verifies that two <see cref="double"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="double"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="double"/> to compare</param>
        /// <param name="actual">The <see cref="double"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(double,double)"/>
        /// <seealso cref="AreNotEqual(double,double,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDoubles
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0d, 1.0d, "Test failed");
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(double.MaxValue, double.MinValue, "Test failed");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(double expected, double actual, string message) {
            Assert.AreNotEqual(expected, actual, message, null);
        }

        /// <summary>
        /// Verifies that two <see cref="double"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not equal.
        /// If the <see cref="double"/>s are equal an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// </summary>
        /// <param name="expected">The <see cref="double"/> to compare</param>
        /// <param name="actual">The <see cref="double"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are equal.</exception>
        /// <seealso cref="AreNotEqual(double,double,string)"/>
        /// <seealso cref="AreNotEqual(double,double,string,object[])"/>
        /// <example>
        /// The following example demonstrates Assert.AreNotEqual using a different variety of values
        /// <code>
        ///using System; 
        ///using MbUnit.Framework;
        ///
        /// namespace AssertDocTests
        /// {
        ///    [TestFixture]
        ///    public class AreNotEqualDoubles
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void AreNotEqual_SameValues()
        ///       {
        ///          Assert.AreNotEqual(1.0d, 1.0d);
        ///       }
        ///
        ///       // This test passes
        ///       [Test]
        ///       public void AreNotEqual_DifferentValues()
        ///       {
        ///          Assert.AreNotEqual(double.MaxValue, double.MinValue);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void AreNotEqual(double expected, double actual) {
            Assert.AreNotEqual(expected, actual, string.Empty, null);
        }
        #endregion

        #endregion

        #region IsNull, IsNotNull

        /// <summary>
        /// Verifies that the given <see cref="object"/> is not null. If it is null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is null</exception>
        /// <seealso cref="IsNotNull(object,string)" />
        /// <seealso cref="IsNotNull(object)"/>
        /// <example>
        /// The following code demonstrates Assert.IsNotNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNotNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNotNull_NotNull()
        ///      {
        ///         Assert.IsNotNull(String.Empty, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNotNull_Null()
        ///      {
        ///         Assert.IsNotNull(null, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNotNull(Object anObject, string format, params object[] args) {
            Assert.IsTrue(anObject != null, format, args);
        }

        /// <summary>
        /// Verifies that the given <see cref="object"/> is not null. If it is null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with the given <paramref name="message"/>
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is null</exception>
        /// <seealso cref="IsNotNull(object,string,object[])" />
        /// <seealso cref="IsNotNull(object)"/>
        /// <example>
        /// The following code demonstrates Assert.IsNotNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNotNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNotNull_NotNull()
        ///      {
        ///         Assert.IsNotNull(String.Empty, "This test failed");
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNotNull_Null()
        ///      {
        ///         Assert.IsNotNull(null, "This test failed");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNotNull(Object anObject, string message) {
            Assert.IsTrue(anObject != null, message);
        }

        /// <summary>
        /// Verifies that the given <see cref="object"/> is not null. If it is null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is null</exception>
        /// <seealso cref="IsNotNull(object,string,object[])" />
        /// <seealso cref="IsNotNull(object,string)"/>
        /// <example>
        /// The following code demonstrates Assert.IsNotNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNotNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNotNull_NotNull()
        ///      {
        ///         Assert.IsNotNull(String.Empty);
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNotNull_Null()
        ///      {
        ///         Assert.IsNotNull(null);
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNotNull(Object anObject) {
            Assert.IsNotNull(anObject, string.Empty);
        }

        /// <summary>
        /// Verifies that the given <see cref="object"/> is null. If it is not null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(string, object[])" />
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is not null</exception>
        /// <seealso cref="IsNull(object)" />
        /// <seealso cref="IsNull(object,string)"/>
        /// <example>
        /// The following code demonstrates Assert.IsNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNull_Null()
        ///      {
        ///         Assert.IsNull(null, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNull_NotNull()
        ///      {
        ///         Assert.IsNull(String.Empty, "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNull(Object anObject, string format, params object[] args) {
            Assert.IsTrue(anObject == null, format, args);
        }

        /// <summary>
        /// Verifies that the given <see cref="object"/> is null. If it is not null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with the given <paramref name="message"/>
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is not null</exception>
        /// <seealso cref="IsNull(object)" />
        /// <seealso cref="IsNull(object,string,object[])"/>
        /// <example>
        /// The following code demonstrates Assert.IsNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNull_Null()
        ///      {
        ///         Assert.IsNull(null, "This test failed");
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNull_NotNull()
        ///      {
        ///         Assert.IsNull(String.Empty, "This test failed");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNull(Object anObject, string message) {
            Assert.IsTrue(anObject == null, message);
        }

        /// <summary>
        /// Verifies that the given <see cref="object"/> is null. If it is not null, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// </summary>
        /// <param name="anObject">The <see cref="object"/> to test</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="anObject"/> is not null</exception>
        /// <seealso cref="IsNull(object,string)" />
        /// <seealso cref="IsNull(object,string,object[])"/>
        /// <example>
        /// The following code demonstrates Assert.IsNull
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class IsNull
        ///   {
        ///      // This test succeeds
        ///      [Test]
        ///      public void IsNull_Null()
        ///      {
        ///         Assert.IsNull(null);
        ///      }
        ///
        ///      //This test fails
        ///      [Test]
        ///      public void IsNull_NotNull()
        ///      {
        ///         Assert.IsNull(String.Empty);
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void IsNull(Object anObject) {
            Assert.IsNull(anObject, string.Empty);
        }
        #endregion

        #region Are same
        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are the same object. That is, the two variables reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are not the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreSame(Object,Object)"/>
        /// <seealso cref="AreSame(Object,Object,String,Object[])"/>
        /// <example>
        /// The following code demonstrates Assert.AreSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreSame
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreSame(a, b, "These two variables do not reference the same object");
        ///      }
        ///   
        ///      //This test fails with AssertionException
        ///      [Test]
        ///      public void AreSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreSame(a, b, "These two variables do not reference the same object");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreSame(Object expected, Object actual, string message) {
            Assert.IncrementAssertCount();
            if (object.ReferenceEquals(expected, actual)) return;

            Assert.FailNotSame(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are the same object. That is, the two variables reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are not the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(String, Object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreSame(Object,Object)"/>
        /// <seealso cref="AreSame(Object,Object,String)"/>
        /// <example>
        /// The following code demonstrates Assert.AreSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreSame
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreSame(a, b, "These two variables do not reference the same object as of {0}", DateTime.Now.ToString());
        ///      }
        ///   
        ///      //This test fails with AssertionException
        ///      [Test]
        ///      public void AreSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreSame(a, b, "These two variables do not reference the same object as of {0}", DateTime.Now.ToString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreSame(Object expected, Object actual, string format, params object[] args) {
            AreSame(expected, actual, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are the same object. That is, the two variables reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are not the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are not references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreSame(Object,Object,String)"/>
        /// <seealso cref="AreSame(Object,Object,String,Object[])"/>
        /// <example>
        /// The following code demonstrates Assert.AreSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreSame
        ///   {
        ///      // This test passes
        ///      [Test]
        ///      public void AreSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreSame(a, b);
        ///      }
        ///   
        ///      //This test fails with AssertionException
        ///      [Test]
        ///      public void AreSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreSame(a, b);
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreSame(Object expected, Object actual) {
            Assert.AreSame(expected, actual, string.Empty);
        }
        #endregion

        #region AreNotSame

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not the same object. 
        /// That is, the two variables do not reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreNotSame(Object,Object,String)"/>
        /// <seealso cref="AreNotSame(Object,Object,String,Object[])"/>
        /// <example>
        /// The following code demonstrates Assert.AreNotSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotSame
        ///   {
        ///      // This test fails with AssertionException
        ///      [Test]
        ///      public void AreNotSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreNotSame(a, b);
        ///      }
        ///   
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreNotSame(a, b);
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreNotSame(object expected, object actual) {
            Assert.AreNotSame(expected, actual, string.Empty);
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not the same object. That is, the two variables do not reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="message">The message to include if the test fails</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreNotSame(Object,Object)"/>
        /// <seealso cref="AreNotSame(Object,Object,String,Object[])"/>
        /// <example>
        /// The following code demonstrates Assert.AreNotSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotSame
        ///   {
        ///      // This test fails with AssertionException
        ///      [Test]
        ///      public void AreNotSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreNotSame(a, b, "These two variables do not reference the same object");
        ///      }
        ///   
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreNotSame(a, b, "These two variables do not reference the same object");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>  
        static public void AreNotSame(object expected, object actual, string message) {
            Assert.IncrementAssertCount();
            if (!object.ReferenceEquals(expected, actual)) return;
            Assert.FailSame(expected, actual, message);
        }

        /// <summary>
        /// Verifies that two <see cref="Object"/>s, <paramref name="expected"/> and <paramref name="actual"/>, are not the same object. 
        /// That is, the two variables do not reference the same <see cref="Object"/>.
        /// If the <see cref="Object"/>s are the same, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with 
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/> through <see cref="System.String.Format(String, Object[])" />.
        /// </summary>
        /// <param name="expected">The <see cref="Object"/> to compare</param>
        /// <param name="actual">The <see cref="Object"/> being compared</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="expected"/> and <paramref name="actual"/> are references to the same <see cref="Object"/>.</exception>
        /// <remarks>
        /// The comparison is made by calling <seealso cref="System.Object.ReferenceEquals(Object,Object)"/>
        /// </remarks>
        /// <seealso cref="AreNotSame(Object,Object)"/>
        /// <seealso cref="AreNotSame(Object,Object,String)"/>
        /// <example>
        /// The following code demonstrates Assert.AreNotSame
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class AreNotSame
        ///   {
        ///      // This test fails with AssertionException
        ///      [Test]
        ///      public void AreNotSame_True()
        ///      {
        ///         object a = new Object();
        ///         object b = a;
        ///         Assert.AreNotSame(a, b, "These two variables do not reference the same object as of {0}", DateTime.Now.ToString());
        ///      }
        ///   
        ///      //This test passes
        ///      [Test]
        ///      public void AreNotSame_False()
        ///      {
        ///         object a = new Object();
        ///         object b = new Object();
        ///         Assert.AreNotSame(a, b, "These two variables do not reference the same object as of {0}", DateTime.Now.ToString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        static public void AreNotSame(object expected, object actual, string format, params object[] args) {
            AreNotSame(expected, actual, String.Format(format, args));
        }
        #endregion

        #region Fail

        /// <summary>
        /// Throws an <see cref="MbUnit.Core.Exceptions.AssertionException"/> with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(string, Object[])" />.
        /// </summary>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException" />
        /// <example>
        /// The following code demonstrates Assert.Fail
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class Fail
        ///   {
        ///      [Test]
        ///      public void RunFail()
        ///      {
        ///         Assert.Fail("This test failed at {0}", DateTime.Now.ToString());
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Fail(String)"/>
        /// <seealso cref="Fail()"/>
        static public void Fail(string format, params object[] args) {
            Fail(string.Format(format, args));
        }

        /// <summary>
        /// Throws an <see cref="MbUnit.Core.Exceptions.AssertionException"/> with a given <paramref name="message" />.
        /// </summary>
        /// <param name="message">The message printed out with the failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException" />
        /// <example>
        /// The following code demonstrates Assert.Fail
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class Fail
        ///   {
        ///      [Test]
        ///      public void RunFail()
        ///      {
        ///         Assert.Fail("This test has failed");
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Fail(String,Object[])"/>
        /// <seealso cref="Fail()"/>
        static public void Fail(string message) {
            throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an <see cref="MbUnit.Core.Exceptions.AssertionException"/>.
        /// </summary>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException" />
        /// <example>
        /// The following code demonstrates Assert.Fail
        /// <code>
        ///using System;
        ///using MbUnit.Framework;
        ///
        ///namespace AssertDocTests
        ///{
        ///   [TestFixture]
        ///   public class Fail
        ///   {
        ///      [Test]
        ///      public void RunFail()
        ///      {
        ///         Assert.Fail();
        ///      }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Fail(String,Object[])"/>
        /// <seealso cref="Fail(String)"/>
        static public void Fail() {
            Assert.Fail(string.Empty);
        }
        #endregion

        #region Ignore

        /// <summary>
        /// Causes the test runner to ignore the remainder of the test and mark the test as ignored.
        /// The test runner will display a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// using <see cref="System.String.Format(string, Object[])" />.
        /// </summary>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the message defined via <paramref name="format"/> and <paramref name="args"/> is null</exception>
        /// <remarks>The test is marked ignored by throwing an <see cref="MbUnit.Core.Exceptions.IgnoreRunException" />. </remarks>
        /// <example>
        /// The following code demonstrates Assert.Ignore
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs
        /// {
        ///    [TestFixture]
        ///    public class Ignore_2Arguments
        ///    {
        ///       // This test is ignored
        ///       [Test]
        ///       public void Ignore()
        ///       {
        ///          Assert.Ignore("This test was ignored at {0}", DateTime.Now.ToShortDateString());
        ///       }
        /// 
        ///       // This test fails because fail is called before ignore
        ///       [Test]
        ///       public void Ignore_FailBefore() {
        ///          Assert.Fail("Fail First");
        ///          Assert.Ignore("This test was ignored at {0}", DateTime.Now.ToShortDateString());
        ///       }
        /// 
        ///       // This test is ignored because fail is called after ignore
        ///       [Test]
        ///       public void Ignore_FailAfter() {
        ///          Assert.Ignore("This test was ignored at {0}", DateTime.Now.ToShortDateString());
        ///          Assert.Fail("Fail After");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Ignore(string)"/>
        /// <seealso cref="T:MbUnit.Framework.IgnoreAttribute"/>
        public static void Ignore(string format, params object[] args) {
            Ignore(String.Format(format, args));
        }

        /// <summary>
        /// Causes the test runner to ignore the remainder of the test and mark the test as ignored with a given <paramref name="message"/>
        /// </summary>
        /// <param name="message">The message to be shown in the test runner saying why the test was ignored</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="message"/> is null</exception>
        /// <remarks>The test is marked ignored by throwing an <see cref="MbUnit.Core.Exceptions.IgnoreRunException" />. </remarks>
        /// <example>
        /// The following code demonstrates Assert.Ignore
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs
        /// {
        ///    [TestFixture]
        ///    public class Ignore_1Argument
        ///    {
        ///       // This test is ignored
        ///       [Test]
        ///       public void Ignore()
        ///       {
        ///          Assert.Ignore("This test was ignored");
        ///       }
        /// 
        ///       // This test fails because fail is called before ignore
        ///       [Test]
        ///       public void Ignore_FailBefore() {
        ///          Assert.Fail("Fail First");
        ///          Assert.Ignore("This test was ignored");
        ///       }
        /// 
        ///       // This test is ignored because fail is called after ignore
        ///       [Test]
        ///       public void Ignore_FailAfter() {
        ///          Assert.Ignore("This test was ignored");
        ///          Assert.Fail("Fail After");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Ignore(string, object[])"/>
        /// <seealso cref="T:MbUnit.Framework.IgnoreAttribute"/>
        public static void Ignore(string message) {
            if (message == null)
                throw new ArgumentNullException("message");
            throw new IgnoreRunException(message);
        }
        #endregion

        #region LowerThan
        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1, 1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(int.MaxValue, int.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(int.MinValue, int.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(int,int,string)"/>
        /// <seealso cref="LowerThan(int,int,string,object[])"/>
        static public void LowerThan(int left, int right) {
            Assert.IncrementAssertCount();
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1, 1, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(int.MaxValue, int.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(int.MinValue, int.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(int,int)"/>
        /// <seealso cref="LowerThan(int,int,string,object[])"/>
        static public void LowerThan(int left, int right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(int,int)"/>
        /// <seealso cref="LowerThan(int,int,string)"/>
        static public void LowerThan(int left, int right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((short)1, (short)1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(short.MaxValue, short.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(short.MinValue, short.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(short,short,string)"/>
        /// <seealso cref="LowerThan(short,short,string,object[])"/>
        static public void LowerThan(short left, short right) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((short)1, (short)1, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(short.MaxValue, short.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(short.MinValue, short.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(short,short)"/>
        /// <seealso cref="LowerThan(short,short,string,object[])"/>
        static public void LowerThan(short left, short right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((short)1, (short)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(short.MaxValue, short.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(short.MinValue, short.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(short,short)"/>
        /// <seealso cref="LowerThan(short,short,string)"/>
        static public void LowerThan(short left, short right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((byte)1, (byte)1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(byte.MaxValue, byte.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(byte.MinValue, byte.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(byte,byte,string)"/>
        /// <seealso cref="LowerThan(byte,byte,string,object[])"/>
        static public void LowerThan(byte left, byte right) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((byte)1, (byte)1, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(byte.MaxValue, byte.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(byte.MinValue, byte.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(byte,byte)"/>
        /// <seealso cref="LowerThan(byte,byte,string,object[])"/>
        static public void LowerThan(byte left, byte right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan((byte)1, (byte)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(byte.MaxValue, byte.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(byte.MinValue, byte.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(byte,byte)"/>
        /// <seealso cref="LowerThan(byte,byte,string)"/>
        static public void LowerThan(byte left, byte right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1L, 1L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(long.MaxValue, long.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(long.MinValue, long.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(long,long,string)"/>
        /// <seealso cref="LowerThan(long,long,string,object[])"/>
        static public void LowerThan(long left, long right) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1L, 1L, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(long.MaxValue, long.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(long.MinValue, long.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(long,long)"/>
        /// <seealso cref="LowerThan(long,long,string,object[])"/>
        static public void LowerThan(long left, long right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(long,long)"/>
        /// <seealso cref="LowerThan(long,long,string)"/>
        static public void LowerThan(long left, long right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1d, 1d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(double.MaxValue, double.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(double.MinValue, double.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(double,double,string)"/>
        /// <seealso cref="LowerThan(double,double,string,object[])"/>
        static public void LowerThan(double left, double right) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1d, 1d, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(double.MaxValue, double.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(double.MinValue, double.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(double,double)"/>
        /// <seealso cref="LowerThan(double,double,string,object[])"/>
        static public void LowerThan(double left, double right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(double,double)"/>
        /// <seealso cref="LowerThan(double,double,string)"/>
        static public void LowerThan(double left, double right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1f, 1f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(float.MaxValue, float.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(float.MinValue, float.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(float,float,string)"/>
        /// <seealso cref="LowerThan(float,float,string,object[])"/>
        static public void LowerThan(float left, float right) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1f, 1f, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(float.MaxValue, float.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(float.MinValue, float.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(float,float)"/>
        /// <seealso cref="LowerThan(float,float,string,object[])"/>
        static public void LowerThan(float left, float right,
            string message) {
            Assert.IsTrue(left < right,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerThan_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_SameValues() {
        ///            Assert.LowerThan(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerThan_GreaterThan() {
        ///            Assert.LowerThan(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerThan_LowerThan() {
        ///            Assert.LowerThan(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(float,float)"/>
        /// <seealso cref="LowerThan(float,float,string)"/>
        static public void LowerThan(float left, float right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerThan_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void LowerThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour1, hour2);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(day, hour);
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(hour, day);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(null, hour);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour, null);
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerThan(hour, day);
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(IComparable,IComparable,string)"/>
        /// <seealso cref="LowerThan(IComparable,IComparable,string,object[])"/>
        static public void LowerThan(IComparable left, IComparable right) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) < 0,
                          "{0} is not lower than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>,
        ///  <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerThan_IComparable {
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void LowerThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour1, hour2, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(day, hour, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(hour, day, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(null, hour, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour, null, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerThan(hour, day, "Left is not lower than right");
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(IComparable,IComparable)"/>
        /// <seealso cref="LowerThan(IComparable,IComparable,string,object[])"/>
        static public void LowerThan(IComparable left, IComparable right,
            string message) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) < 0,
                          "{0} is not lower than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is strictly less than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerThan_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void LowerThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour1, hour2, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(day, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(null, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerThan(hour, null, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerThan(IComparable,IComparable)"/>
        /// <seealso cref="LowerThan(IComparable,IComparable,string)"/>
        static public void LowerThan(IComparable left, IComparable right,
            string format, params object[] args) {
            LowerThan(left, right, String.Format(format, args));
        }

        #endregion

        #region LowerEqualThan
        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1, 1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(int.MaxValue, int.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(int.MinValue, int.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(int,int,string)"/>
        /// <seealso cref="LowerEqualThan(int,int,string,object[])"/>
        static public void LowerEqualThan(int left, int right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1, 1, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(int.MaxValue, int.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(int.MinValue, int.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(int,int)"/>
        /// <seealso cref="LowerEqualThan(int,int,string,object[])"/>
        static public void LowerEqualThan(int left, int right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(int,int)"/>
        /// <seealso cref="LowerEqualThan(int,int,string)"/>
        static public void LowerEqualThan(int left, int right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((short)1, (short)1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(short.MaxValue, short.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(short.MinValue, short.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(short,short,string)"/>
        /// <seealso cref="LowerEqualThan(short,short,string,object[])"/>
        static public void LowerEqualThan(short left, short right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((short)1, (short)1, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(short.MaxValue, short.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(short.MinValue, short.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(short,short)"/>
        /// <seealso cref="LowerEqualThan(short,short,string,object[])"/>
        static public void LowerEqualThan(short left, short right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two shorts, <paramref name="left"/> is strictly less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((short)1, (short)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(short.MaxValue, short.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(short.MinValue, short.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(short,short)"/>
        /// <seealso cref="LowerEqualThan(short,short,string)"/>
        static public void LowerEqualThan(short left, short right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((byte)1, (byte)1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(byte.MaxValue, byte.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(byte.MinValue, byte.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(byte,byte,string)"/>
        /// <seealso cref="LowerEqualThan(byte,byte,string,object[])"/>
        static public void LowerEqualThan(byte left, byte right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((byte)1, (byte)1, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(byte.MaxValue, byte.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerEqualThan() {
        ///            Assert.LowerEqualThan(byte.MinValue, byte.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(byte,byte)"/>
        /// <seealso cref="LowerEqualThan(byte,byte,string,object[])"/>
        static public void LowerEqualThan(byte left, byte right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan((byte)1, (byte)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(byte.MaxValue, byte.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(byte.MinValue, byte.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(byte,byte)"/>
        /// <seealso cref="LowerEqualThan(byte,byte,string)"/>
        static public void LowerEqualThan(byte left, byte right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is less or equal to than <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1L, 1L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(long.MaxValue, long.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(long.MinValue, long.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(long,long,string)"/>
        /// <seealso cref="LowerEqualThan(long,long,string,object[])"/>
        static public void LowerEqualThan(long left, long right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1L, 1L, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(long.MaxValue, long.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(long.MinValue, long.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(long,long)"/>
        /// <seealso cref="LowerEqualThan(long,long,string,object[])"/>
        static public void LowerEqualThan(long left, long right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(long,long)"/>
        /// <seealso cref="LowerEqualThan(long,long,string)"/>
        static public void LowerEqualThan(long left, long right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1d, 1d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(double.MaxValue, double.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(double.MinValue, double.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(double,double,string)"/>
        /// <seealso cref="LowerEqualThan(double,double,string,object[])"/>
        static public void LowerEqualThan(double left, double right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1d, 1d, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(double.MaxValue, double.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(double.MinValue, double.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(double,double)"/>
        /// <seealso cref="LowerEqualThan(double,double,string,object[])"/>
        static public void LowerEqualThan(double left, double right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(double,double)"/>
        /// <seealso cref="LowerEqualThan(double,double,string)"/>
        static public void LowerEqualThan(double left, double right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1, 1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(float.MaxValue, float.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(float.MinValue, float.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(float,float,string)"/>
        /// <seealso cref="LowerEqualThan(float,float,string,object[])"/>
        static public void LowerEqualThan(float left, float right) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1f, 1f, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(float.MaxValue, float.MinValue, "Left is not lower than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerEqualThan() {
        ///            Assert.LowerEqualThan(float.MinValue, float.MaxValue, "Left is not lower than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(float,float)"/>
        /// <seealso cref="LowerEqualThan(float,float,string,object[])"/>
        static public void LowerEqualThan(float left, float right,
            string message) {
            Assert.IsTrue(left <= right,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class LowerEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_SameValues() {
        ///            Assert.LowerEqualThan(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void LowerEqualThan_GreaterThan() {
        ///            Assert.LowerEqualThan(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void LowerEqualThan_LowerThan() {
        ///            Assert.LowerEqualThan(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(float,float)"/>
        /// <seealso cref="LowerEqualThan(float,float,string)"/>
        static public void LowerEqualThan(float left, float right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerEqualThan_IComparable 
        /// {    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour1, hour2);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(day, hour);
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(hour, day);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(null, hour);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour, null);
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerEqualThan(hour, day);
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable,string)"/>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable,string,object[])"/>
        static public void LowerEqualThan(IComparable left, IComparable right) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) <= 0,
                          "{0} is not lower equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>,
        ///  <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerEqualThan_IComparable {
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour1, hour2, "Left is not lower than or equal to right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(day, hour, "Left is not lower than or equal to right");
        ///    }
        ///    
        ///    // This test fails passes
        ///    [Test]
        ///    public void LowerEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(hour, day, "Left is not lower than or equal to right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(null, hour, "Left is not lower than or equal to right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour, null, "Left is not lower than or equal to right");
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerEqualThan(hour, day, "Left is not lower than or equal to right");
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable)"/>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable,string,object[])"/>
        static public void LowerEqualThan(IComparable left, IComparable right,
            string message) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) <= 0,
                          "{0} is not lower equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is less than or equal to <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.LowerEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class LowerEqualThan_IComparable 
        /// {    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour1, hour2, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(day, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void LowerEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.LowerEqualThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(null, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void LowerEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.LowerEqualThan(hour, null, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void LowerEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.LowerEqualThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable)"/>
        /// <seealso cref="LowerEqualThan(IComparable,IComparable,string)"/>
        static public void LowerEqualThan(IComparable left, IComparable right,
            string format, params object[] args) {
            LowerEqualThan(left, right, String.Format(format, args));
        }
        #endregion

        #region Less
        //Nunit base - dupe of LowerThan
        #region Ints

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(int,int)"/>
        /// <seealso cref="Less(int,int,string)"/>
        static public void Less(int arg1, int arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1, 1, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(int.MaxValue, int.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(int.MinValue, int.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(int,int)"/>
        /// <seealso cref="Less(int,int,string,object[])"/>
        static public void Less(int arg1, int arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1, 1);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(int.MaxValue, int.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(int.MinValue, int.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(int,int,string)"/>
        /// <seealso cref="Less(int,int,string,object[])"/>
        static public void Less(int arg1, int arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region UInts

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1u, 1u,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(uint.MaxValue, uint.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(uint.MinValue, uint.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(uint,uint)"/>
        /// <seealso cref="Less(uint,uint,string)"/>
        static public void Less(uint arg1, uint arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1u, 1u, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(uint.MaxValue, uint.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(uint.MinValue, uint.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(uint,uint)"/>
        /// <seealso cref="Less(uint,uint,string,object[])"/>
        static public void Less(uint arg1, uint arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1u, 1u);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(uint.MaxValue, uint.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(uint.MinValue, uint.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(uint,uint,string)"/>
        /// <seealso cref="Less(uint,uint,string,object[])"/>
        static public void Less(uint arg1, uint arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region Decimals

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1m, 1m,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(decimal.MaxValue, decimal.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(decimal.MinValue, decimal.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(decimal,decimal)"/>
        /// <seealso cref="Less(decimal,decimal,string)"/>
        static public void Less(decimal arg1, decimal arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1m, 1m, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(decimal.MaxValue, decimal.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(decimal.MinValue, decimal.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(decimal,decimal)"/>
        /// <seealso cref="Less(decimal,decimal,string,object[])"/>
        static public void Less(decimal arg1, decimal arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1m, 1m);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(decimal.MaxValue, decimal.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(decimal.MinValue, decimal.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(decimal,decimal,string)"/>
        /// <seealso cref="Less(decimal,decimal,string,object[])"/>
        static public void Less(decimal arg1, decimal arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region Long

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(long,long)"/>
        /// <seealso cref="Less(long,long,string)"/>
        static public void Less(long arg1, long arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1L, 1L, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(long.MaxValue, long.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(long.MinValue, long.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(long,long)"/>
        /// <seealso cref="Less(long,long,string,object[])"/>
        static public void Less(long arg1, long arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1L, 1L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(long.MaxValue, long.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(long.MinValue, long.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(long,long,string)"/>
        /// <seealso cref="Less(long,long,string,object[])"/>
        static public void Less(long arg1, long arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region Doubles

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">double</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(double,double)"/>
        /// <seealso cref="Less(double,double,string)"/>
        static public void Less(double arg1, double arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">double</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1d, 1d, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(double.MaxValue, double.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(double.MinValue, double.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(double,double)"/>
        /// <seealso cref="Less(double,double,string,object[])"/>
        static public void Less(double arg1, double arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">double</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1d, 1d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(double.MaxValue, double.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(double.MinValue, double.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(double,double,string)"/>
        /// <seealso cref="Less(double,double,string,object[])"/>
        static public void Less(double arg1, double arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region Floats

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(float,float)"/>
        /// <seealso cref="Less(float,float,string)"/>
        static public void Less(float arg1, float arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1f, 1f, "Left is not lower than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(float.MaxValue, float.MinValue, "Left is not lower than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(float.MinValue, float.MaxValue, "Left is not lower than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(float,float)"/>
        /// <seealso cref="Less(float,float,string,object[])"/>
        static public void Less(float arg1, float arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Less_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_SameValues() {
        ///            Assert.Less(1f, 1f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Less_GreaterThan() {
        ///            Assert.Less(float.MaxValue, float.MinValue);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Less_LowerThan() {
        ///            Assert.Less(float.MinValue, float.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(float,float,string)"/>
        /// <seealso cref="Less(float,float,string,object[])"/>
        static public void Less(float arg1, float arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #region IComparables

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="arg1"/> and <paramref name="arg2"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class Less_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void Less_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour1, hour2, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(day, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void Less_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(null, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour, null, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void Less_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Less(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(IComparable,IComparable)"/>
        /// <seealso cref="Less(IComparable,IComparable,string)"/>
        static public void Less(IComparable arg1, IComparable arg2, string message, params object[] args) {
            Assert.LowerThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>,
        ///  <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="arg1"/> and <paramref name="arg2"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class Less_IComparable {
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void Less_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour1, hour2, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(day, hour, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void Less_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(hour, day, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(null, hour, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour, null, "Left is not lower than right");
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void Less_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Less(hour, day, "Left is not lower than right");
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(IComparable,IComparable)"/>
        /// <seealso cref="Less(IComparable,IComparable,string,object[])"/>
        static public void Less(IComparable arg1, IComparable arg2, string message) {
            Assert.LowerThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="arg1"/> is strictly less than <paramref name="arg2"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or greater than <paramref name="arg2"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="arg1"/> and <paramref name="arg2"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Less using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class Less_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void Less_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour1, hour2);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(day, hour);
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void Less_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Less(hour, day);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(null, hour);
        ///    }
        ///    
        ///    // This test fails with an AssertionException
        ///    [Test]
        ///    public void Less_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Less(hour, null);
        ///    }
        ///    
        ///    // This test fails with an ArgumentException
        ///    [Test]
        ///    public void Less_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Less(hour, day);
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Less(IComparable,IComparable,string)"/>
        /// <seealso cref="Less(IComparable,IComparable,string,object[])"/>
        static public void Less(IComparable arg1, IComparable arg2) {
            Assert.LowerThan(arg1, arg2);
        }

        #endregion

        #endregion

        #region GreaterThan
        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1, 1);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(int.MaxValue, int.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, int.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, int.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(int,int,string)"/>
        /// <seealso cref="GreaterThan(int,int,string,object[])"/>
        static public void GreaterThan(int left, int right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1, 1, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(int.MaxValue, int.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, int.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, int.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(int,int)"/>
        /// <seealso cref="GreaterThan(int,int,string,object[])"/>
        static public void GreaterThan(int left, int right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(int.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(int,int)"/>
        /// <seealso cref="GreaterThan(int,int,string)"/>
        static public void GreaterThan(int left, int right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integer</see> values, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((short)1, (short)1);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(short.MaxValue, short.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, short.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, short.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(short,short,string)"/>
        /// <seealso cref="GreaterThan(short,short,string,object[])"/>
        static public void GreaterThan(short left, short right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integer</see> values, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((short)1, (short)1, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(short.MaxValue, short.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, short.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, short.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(short,short)"/>
        /// <seealso cref="GreaterThan(short,short,string,object[])"/>
        static public void GreaterThan(short left, short right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integer</see> values, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> value</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((short)1, (short)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(short.MaxValue, short.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, short.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, short.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(short.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(short,short)"/>
        /// <seealso cref="GreaterThan(short,short,string)"/>
        static public void GreaterThan(short left, short right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((byte)1, (byte)1);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(byte.MaxValue, byte.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, byte.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, byte.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(byte,byte,string)"/>
        /// <seealso cref="GreaterThan(byte,byte,string,object[])"/>
        static public void GreaterThan(byte left, byte right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((byte)1, (byte)1, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(byte.MaxValue, byte.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, byte.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, byte.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(byte,byte)"/>
        /// <seealso cref="GreaterThan(byte,byte,string,object[])"/>
        static public void GreaterThan(byte left, byte right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan((byte)1, (byte)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(byte.MaxValue, byte.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, byte.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, byte.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(byte.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(byte,byte)"/>
        /// <seealso cref="GreaterThan(byte,byte,string)"/>
        static public void GreaterThan(byte left, byte right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1L, 1L);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(long.MaxValue, long.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, long.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, long.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(long,long,string)"/>
        /// <seealso cref="GreaterThan(long,long,string,object[])"/>
        static public void GreaterThan(long left, long right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1L, 1L, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(long.MaxValue, long.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, long.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, long.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(long,long)"/>
        /// <seealso cref="GreaterThan(long,long,string,object[])"/>
        static public void GreaterThan(long left, long right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(long.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(long,long)"/>
        /// <seealso cref="GreaterThan(long,long,string)"/>
        static public void GreaterThan(long left, long right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1d, 1d);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(double.MaxValue, double.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, double.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, double.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(double,double,string)"/>
        /// <seealso cref="GreaterThan(double,double,string,object[])"/>
        static public void GreaterThan(double left, double right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1d, 1d, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(double.MaxValue, double.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, double.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, double.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(double,double)"/>
        /// <seealso cref="GreaterThan(double,double,string,object[])"/>
        static public void GreaterThan(double left, double right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(double.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(double,double)"/>
        /// <seealso cref="GreaterThan(double,double,string)"/>
        static public void GreaterThan(double left, double right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_2Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1f, 1f);
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(float.MaxValue, float.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, float.MaxValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, float.MinValue);
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, null);
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(float,float,string)"/>
        /// <seealso cref="GreaterThan(float,float,string,object[])"/>
        static public void GreaterThan(float left, float right) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_3Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1f, 1f, "Left is not greater than right");
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(float.MaxValue, float.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, float.MaxValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, float.MinValue, "Left is not greater than right");
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, null, "Left is not greater than right");
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(float,float)"/>
        /// <seealso cref="GreaterThan(float,float,string,object[])"/>
        static public void GreaterThan(float left, float right, string message) {
            Assert.IsTrue(left > right,
                          "{0} is not strictly greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class GreaterThan_4Arguments
        ///    {
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_SameValues()
        ///       {
        ///          Assert.GreaterThan(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test succeeds
        ///       [Test]
        ///       public void GreaterThan_GreaterThan()
        ///       {
        ///          Assert.GreaterThan(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LowerThan()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_LeftNull()
        ///       {
        ///          Assert.GreaterThan(null, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///
        ///       // This test fails
        ///       [Test]
        ///       public void GreaterThan_RightNull()
        ///       {
        ///          Assert.GreaterThan(float.MinValue, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///       }
        ///    }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(float,float)"/>
        /// <seealso cref="GreaterThan(float,float,string)"/>
        static public void GreaterThan(float left, float right, string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterThan_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour1, hour2);
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(day, hour);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(hour, day);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(null, hour);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour, null);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterThan(hour, day);
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(IComparable,IComparable,string)"/>
        /// <seealso cref="GreaterThan(IComparable,IComparable,string,object[])"/>
        static public void GreaterThan(IComparable left, IComparable right) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) > 0,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>,
        /// <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterThan_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour1, hour2, "Left is not greater than right");
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(day, hour, "Left is not greater than right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(hour, day, "Left is not greater than right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(null, hour, "Left is not greater than right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour, null, "Left is not greater than right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterThan(hour, day, "Left is not greater than right");
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(IComparable,IComparable)"/>
        /// <seealso cref="GreaterThan(IComparable,IComparable,string,object[])"/>
        static public void GreaterThan(IComparable left, IComparable right, string message) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) > 0,
                          "{0} is not greater than {1} {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is strictly greater than <paramref name="right"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />. 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterThan_IComparable 
        /// {    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour1, hour2, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(day, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(null, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterThan(hour, null, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterThan(IComparable,IComparable)"/>
        /// <seealso cref="GreaterThan(IComparable,IComparable,string)"/>
        static public void GreaterThan(IComparable left, IComparable right,
            string format, params object[] args) {
            GreaterThan(left, right, String.Format(format, args));
        }
        #endregion

        #region Greater
        //NUnit Base, Dupe of GreaterThan
        #region Ints

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(int,int)"/>
        /// <seealso cref="Greater(int,int,string)"/>
        static public void Greater(int arg1,
            int arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1, 1, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(int.MaxValue, int.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(int.MinValue, int.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(int,int)"/>
        /// <seealso cref="Greater(int,int,string,object[])"/>
        static public void Greater(int arg1, int arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An integer</param>
        /// <param name="arg2">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(int.MaxValue, int.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(int.MinValue, int.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(int,int,string)"/>
        /// <seealso cref="Greater(int,int,string,object[])"/>
        static public void Greater(int arg1, int arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region UInts

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1u, 1u,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(uint.MaxValue, uint.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(uint.MinValue, uint.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(uint,uint)"/>
        /// <seealso cref="Greater(uint,uint,string)"/>
        static public void Greater(uint arg1,
            uint arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1u, 1u, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(uint.MaxValue, uint.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(uint.MinValue, uint.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(uint,uint)"/>
        /// <seealso cref="Greater(uint,uint,string,object[])"/>
        static public void Greater(uint arg1, uint arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.UInt32">unsigned integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <param name="arg2">An <see cref="System.UInt32">unsigned integers</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1u, 1u);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(uint.MaxValue, uint.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(uint.MinValue, uint.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(uint,uint,string)"/>
        /// <seealso cref="Greater(uint,uint,string,object[])"/>
        static public void Greater(uint arg1, uint arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region Decimals

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1m, 1m,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(decimal.MaxValue, decimal.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(decimal.MinValue, decimal.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(decimal,decimal)"/>
        /// <seealso cref="Greater(decimal,decimal,string)"/>
        static public void Greater(decimal arg1,
            decimal arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1m, 1m, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(decimal.MaxValue, decimal.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(decimal.MinValue, decimal.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(decimal,decimal)"/>
        /// <seealso cref="Greater(decimal,decimal,string,object[])"/>
        static public void Greater(decimal arg1, decimal arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Decimal">128-bit integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <param name="arg2">A <see cref="System.Decimal">128-bit integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1m, 1m);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(decimal.MaxValue, decimal.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(decimal.MinValue, decimal.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(decimal,decimal,string)"/>
        /// <seealso cref="Greater(decimal,decimal,string,object[])"/>
        static public void Greater(decimal arg1, decimal arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region Long

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(long,long)"/>
        /// <seealso cref="Greater(long,long,string)"/>
        static public void Greater(long arg1,
            long arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1L, 1L, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(long.MaxValue, long.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(long.MinValue, long.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(long,long)"/>
        /// <seealso cref="Greater(long,long,string,object[])"/>
        static public void Greater(long arg1, long arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integers</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Int64">long integer</see></param>
        /// <param name="arg2">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1L, 1L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(long.MaxValue, long.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(long.MinValue, long.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(long,long,string)"/>
        /// <seealso cref="Greater(long,long,string,object[])"/>
        static public void Greater(long arg1, long arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region Doubles

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">doubles</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(double,double)"/>
        /// <seealso cref="Greater(double,double,string)"/>
        static public void Greater(double arg1,
            double arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">double</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1d, 1d, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(double.MaxValue, double.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(double.MinValue, double.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(double,double)"/>
        /// <seealso cref="Greater(double,double,string,object[])"/>
        static public void Greater(double arg1,
            double arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Double">doubles</see>, 
        /// <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Double">double</see></param>
        /// <param name="arg2">A <see cref="System.Double">double</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1d, 1d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(double.MaxValue, double.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(double.MinValue, double.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(double,double,string)"/>
        /// <seealso cref="Greater(double,double,string,object[])"/>
        static public void Greater(double arg1, double arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region Floats

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(float,float)"/>
        /// <seealso cref="Greater(float,float,string)"/>
        static public void Greater(float arg1,
            float arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1f, 1f, "Left is not greater than right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(float.MaxValue, float.MinValue, "Left is not greater than right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(float.MinValue, float.MaxValue, "Left is not greater than right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(float,float)"/>
        /// <seealso cref="Greater(float,float,string,object[])"/>
        static public void Greater(float arg1, float arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Single">floats</see>, <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">A <see cref="System.Single">float</see></param>
        /// <param name="arg2">A <see cref="System.Single">float</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class Greater_2Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_SameValues() {
        ///            Assert.Greater(1f, 1f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Greater_GreaterThan() {
        ///            Assert.Greater(float.MaxValue, float.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Greater_LowerThan() {
        ///            Assert.Greater(float.MinValue, float.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(float,float,string)"/>
        /// <seealso cref="Greater(float,float,string,object[])"/>
        static public void Greater(float arg1, float arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #region IComparables

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="arg1"/> and <paramref name="arg2"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///   [TestFixture]
        ///   public class Greater_IComparable {
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0 ,1, 0);
        ///        Assert.Greater(hour1, hour2,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///
        ///    // This test passes
        ///    [Test]
        ///    public void Greater_Greater() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(day, hour,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(hour, day,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(null, hour,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(hour, null,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_NotSameType()
        ///    {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Greater(hour, day,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(IComparable,IComparable)"/>
        /// <seealso cref="Greater(IComparable,IComparable,string)"/>
        static public void Greater(IComparable arg1,
            IComparable arg2, string message, params object[] args) {
            Assert.GreaterThan(arg1, arg2, message, args);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> 
        /// is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///   [TestFixture]
        ///   public class Greater_IComparable {
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0 ,1, 0);
        ///        Assert.Greater(hour1, hour2, "Left is not greater than right");
        ///    }
        ///
        ///    // This test passes
        ///    [Test]
        ///    public void Greater_Greater() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(day, hour, "Left is not greater than right");
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(hour, day, "Left is not greater than right");
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(null, hour, "Left is not greater than right");
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(hour, null, "Left is not greater than right");
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_NotSameType()
        ///    {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Greater(hour, day, "Left is not greater than right");
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(IComparable,IComparable)"/>
        /// <seealso cref="Greater(IComparable,IComparable,string,object[])"/>
        static public void Greater(IComparable arg1, IComparable arg2, string message) {
            Assert.GreaterThan(arg1, arg2, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="arg1"/> is strictly greater than <paramref name="arg2"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="arg1">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="arg2">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="arg1"/> is equal to or less than <paramref name="arg2"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Greater using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///   [TestFixture]
        ///   public class Greater_IComparable {
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0 ,1, 0);
        ///        Assert.Greater(hour1, hour2);
        ///    }
        ///
        ///    // This test passes
        ///    [Test]
        ///    public void Greater_Greater() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(day, hour);
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.Greater(hour, day);
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(null, hour);
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.Greater(hour, null9);
        ///    }
        ///
        ///    // This test fails
        ///    [Test]
        ///    public void Greater_NotSameType()
        ///    {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.Greater(hour, day);
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Greater(IComparable,IComparable,string)"/>
        /// <seealso cref="Greater(IComparable,IComparable,string,object[])"/>
        static public void Greater(IComparable arg1, IComparable arg2) {
            Assert.GreaterThan(arg1, arg2);
        }

        #endregion

        #endregion

        #region GreaterEqualThan
        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(int.MaxValue, int.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(int.MinValue, int.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(int,int,string)"/>
        /// <seealso cref="GreaterEqualThan(int,int,string,object[])"/>
        static public void GreaterEqualThan(int left, int right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1, 1, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(int.MaxValue, int.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(int.MinValue, int.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(int,int)"/>
        /// <seealso cref="GreaterEqualThan(int,int,string,object[])"/>
        static public void GreaterEqualThan(int left, int right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two integers, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An integer</param>
        /// <param name="right">An integer</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(int.MaxValue, int.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(int.MinValue, int.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(int,int)"/>
        /// <seealso cref="GreaterEqualThan(int,int,string)"/>
        static public void GreaterEqualThan(int left, int right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integers</see>, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see></param>
        /// <param name="right">A <see cref="System.Int16">short integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan((short)1, (short)1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(short.MaxValue, short.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(short.MinValue, short.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(short,short,string)"/>
        /// <seealso cref="GreaterEqualThan(short,short,string,object[])"/>
        static public void GreaterEqualThan(short left, short right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integers</see>, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see></param>
        /// <param name="right">A <see cref="System.Int16">short integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan((short)1, (short)1, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(short.MaxValue, short.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(short.MinValue, short.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(short,short)"/>
        /// <seealso cref="GreaterEqualThan(short,short,string,object[])"/>
        static public void GreaterEqualThan(short left, short right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int16">short integers</see>, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int16">short integer</see></param>
        /// <param name="right">A <see cref="System.Int16">short integer</see></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(short.MaxValue, short.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(short.MinValue, short.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(short,short)"/>
        /// <seealso cref="GreaterEqualThan(short,short,string)"/>
        static public void GreaterEqualThan(short left, short right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan((byte)1, (byte)1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(byte.MaxValue, byte.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(byte.MinValue, byte.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(byte,byte,string)"/>
        /// <seealso cref="GreaterEqualThan(byte,byte,string,object[])"/>
        static public void GreaterEqualThan(byte left, byte right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan((byte)1, (byte)1, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(byte.MaxValue, byte.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(byte.MinValue, byte.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(byte,byte)"/>
        /// <seealso cref="GreaterEqualThan(byte,byte,string,object[])"/>
        static public void GreaterEqualThan(byte left, byte right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }


        /// <summary>
        /// Verifies that for two <see cref="System.Byte" />s, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="right">A <see cref="System.Byte" /> value (0 to 255)</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan((byte)1, (byte)1,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(byte.MaxValue, byte.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(byte.MinValue, byte.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(byte,byte)"/>
        /// <seealso cref="GreaterEqualThan(byte,byte,string)"/>
        static public void GreaterEqualThan(byte left, byte right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1L, 1L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(long.MaxValue, long.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(long.MinValue, long.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(long,long,string)"/>
        /// <seealso cref="GreaterEqualThan(long,long,string,object[])"/>
        static public void GreaterEqualThan(long left, long right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1L, 1L, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(long.MaxValue, long.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(long.MinValue, long.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(long,long)"/>
        /// <seealso cref="GreaterEqualThan(long,long,string,object[])"/>
        static public void GreaterEqualThan(long left, long right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two <see cref="System.Int64">long integer</see>s, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A <see cref="System.Int64">long integer</see></param>
        /// <param name="right">A <see cref="System.Int64">long integer</see></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(long.MaxValue, long.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(long.MinValue, long.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(long,long)"/>
        /// <seealso cref="GreaterEqualThan(long,long,string)"/>
        static public void GreaterEqualThan(long left, long right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1d, 1d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(double.MaxValue, double.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(double.MinValue, double.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(double,double,string)"/>
        /// <seealso cref="GreaterEqualThan(double,double,string,object[])"/>
        static public void GreaterEqualThan(double left, double right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1d, 1d, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(double.MaxValue, double.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(double.MinValue, double.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(double,double)"/>
        /// <seealso cref="GreaterEqualThan(double,double,string,object[])"/>
        static public void GreaterEqualThan(double left, double right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two doubles, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A double</param>
        /// <param name="right">A double</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(double.MaxValue, double.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(double.MinValue, double.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(double,double)"/>
        /// <seealso cref="GreaterEqualThan(double,double,string)"/>
        static public void GreaterEqualThan(double left, double right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_2Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1f, 1f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(float.MaxValue, float.MinValue);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(float.MinValue, float.MaxValue);
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(float,float,string)"/>
        /// <seealso cref="GreaterEqualThan(float,float,string,object[])"/>
        static public void GreaterEqualThan(float left, float right) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_3Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1f, 1f, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(float.MaxValue, float.MinValue, "Left is not greater than or equal to right");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(float.MinValue, float.MaxValue, "Left is not greater than or equal to right");
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(float,float)"/>
        /// <seealso cref="GreaterEqualThan(float,float,string,object[])"/>
        static public void GreaterEqualThan(float left, float right, string message) {
            Assert.IsTrue(left >= right,
                          "{0} is not greater than {1}, {2}",
                          left, right, message);
        }


        /// <summary>
        /// Verifies that for two floats, <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">A float</param>
        /// <param name="right">A float</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using System;
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///    [TestFixture]
        ///    public class GreaterEqualThan_4Arguments {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_SameValues() {
        ///            Assert.GreaterEqualThan(1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void GreaterEqualThan_GreaterThan() {
        ///            Assert.GreaterEqualThan(float.MaxValue, float.MinValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void GreaterEqualThan_LowerThan() {
        ///            Assert.GreaterEqualThan(float.MinValue, float.MaxValue,
        ///                "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///        }
        ///   }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(float,float)"/>
        /// <seealso cref="GreaterEqualThan(float,float,string)"/>
        static public void GreaterEqualThan(float left, float right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterEqualThan_IComparable 
        /// {    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour1, hour2);
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(day, hour);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(hour, day);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(null, hour);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour, null);
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterEqualThan(hour, day);
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable,string)"/>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable,string,object[])"/>
        static public void GreaterEqualThan(IComparable left, IComparable right) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) >= 0,
                          "{0} is not greater equal than {1}",
                          left, right);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">Thrown if <paramref name="left"/> is less than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterEqualThan_IComparable 
        /// {    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour1, hour2, "Left is not greater than or equal to right");
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(day, hour, "Left is not greater than or equal to right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(hour, day, "Left is not greater than or equal to right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(null, hour, "Left is not greater than or equal to right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour, null, "Left is not greater than or equal to right");
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterEqualThan(hour, day, "Left is not greater than or equal to right");
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable)"/>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable,string,object[])"/>
        static public void GreaterEqualThan(IComparable left, IComparable right, string message) {
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);
            Assert.IsTrue(left.CompareTo(right) >= 0,
                          "{0} is not greater equal than {1}, {2}",
                          left, right, message);
        }

        /// <summary>
        /// Verifies that for two objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="left"/> is strictly greater than or equal to <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="left">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="right">An object derived from <see cref="System.IComparable"/></param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        ///     Thrown if <paramref name="left"/> is equal to or greater than <paramref name="right"/> or if either value is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.GreaterEqualThan using a different variety of values
        /// <code>
        /// using MbUnit.Framework;
        /// 
        /// namespace MbUnitAssertDocs 
        /// {
        ///[TestFixture]
        ///public class GreaterEqualThan_IComparable 
        /// {    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_SameValues() {
        ///        TimeSpan hour1 = new TimeSpan(0, 1, 0);
        ///        TimeSpan hour2 = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour1, hour2, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test passes
        ///    [Test]
        ///    public void GreaterEqualThan_GreaterThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(day, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LowerThan() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        TimeSpan day = new TimeSpan(1, 0, 0);
        ///        Assert.GreaterEqualThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_LeftNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(null, hour, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_RightNull() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        Assert.GreaterEqualThan(hour, null, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///    
        ///    // This test fails
        ///    [Test]
        ///    public void GreaterEqualThan_NotSameType() {
        ///        TimeSpan hour = new TimeSpan(0, 1, 0);
        ///        String day = "A Day";
        ///        Assert.GreaterEqualThan(hour, day, "This test failed at {0}", DateTime.Now.ToShortTimeString());
        ///    }
        ///}
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable)"/>
        /// <seealso cref="GreaterEqualThan(IComparable,IComparable,string)"/>
        static public void GreaterEqualThan(IComparable left, IComparable right,
            string format, params object[] args) {
            GreaterEqualThan(left, right, String.Format(format, args));
        }
        #endregion

        #region Between
        /// <summary>
        /// Verifies that for three integers, <paramref name="test"/> is between or equal to 
        /// one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The integer being tested</param>
        /// <param name="left">An integer marking one of end of a range</param>
        /// <param name="right">An integer marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(int,int,int,string)"/>
        /// <seealso cref="Between(int,int,int,string,object[])"/>		 
        static public void Between(int test, int left, int right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three integers, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The integer being tested</param>
        /// <param name="left">An integer marking one of end of a range</param>
        /// <param name="right">An integer marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(int,int,int)"/>
        /// <seealso cref="Between(int,int,int,string,object[])"/>		
        static public void Between(int test, int left, int right, string message) {
            int min = Math.Min(left, right);
            int max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three integers, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The integer being tested</param>
        /// <param name="left">An integer marking one of end of a range</param>
        /// <param name="right">An integer marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(int,int,int)"/>
        /// <seealso cref="Between(int,int,int,string)"/>	
        static public void Between(int test, int left, int right, string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int16">short integers</see>, <paramref name="test"/> is between or equal to 
        /// one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The <see cref="System.Int16">short integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int16">short integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(short,short,short,string)"/>
        /// <seealso cref="Between(short,short,short,string,object[])"/>		 
        static public void Between(short test, short left, short right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int16">short integer</see>s, <paramref name="test"/> is between or equal to one 
        /// of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the 
        /// given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The <see cref="System.Int16">short integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int16">short integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(short,short,short)"/>
        /// <seealso cref="Between(short,short,short,string,object[])"/>		
        static public void Between(short test, short left, short right, string message) {
            short min = Math.Min(left, right);
            short max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int16">short integer</see>s, <paramref name="test"/> is between or equal to one of 
        /// <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The <see cref="System.Int16">short integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int16">short integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(short,short,short)"/>
        /// <seealso cref="Between(short,short,short,string)"/>	
        static public void Between(short test, short left, short right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Byte" />s, <paramref name="test"/> is between or equal to one 
        /// of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value between 0 and 255 being tested</param>
        /// <param name="left">The value between 0 and 255 marking one of end of a range</param>
        /// <param name="right">The value between 0 and 255 marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(byte,byte,byte,string)"/>
        /// <seealso cref="Between(byte,byte,byte,string,object[])"/>		 
        static public void Between(byte test, byte left, byte right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Byte" />s, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The value between 0 and 255 being tested</param>
        /// <param name="left">The value between 0 and 255 marking one of end of a range</param>
        /// <param name="right">The value between 0 and 255 marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(byte,byte,byte)"/>
        /// <seealso cref="Between(byte,byte,byte,string,object[])"/>		
        static public void Between(byte test, byte left, byte right, string message) {
            byte min = Math.Min(left, right);
            byte max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Byte" />s, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The value between 0 and 255 being tested</param>
        /// <param name="left">The value between 0 and 255 marking one of end of a range</param>
        /// <param name="right">The value between 0 and 255 marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1, 1, 1,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15, 1, 10,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(byte,byte,byte)"/>
        /// <seealso cref="Between(byte,byte,byte,string)"/>	
        static public void Between(byte test, byte left, byte right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int64">long integer</see>s, <paramref name="test"/> is between or equal to one 
        /// of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The <see cref="System.Int64">long integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int64">long integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int64">long integer</see> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0L, 1L, 10L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1L, 1L, 10L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5L, 1L, 10L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1L, 1L, 1L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10L, 1L, 10L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15L, 1L, 10L);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(long,long,long,string)"/>
        /// <seealso cref="Between(long,long,long,string,object[])"/>		 
        static public void Between(long test, long left, long right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int64">long integer</see>s, <paramref name="test"/> is between 
        /// or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with 
        /// the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The <see cref="System.Int64">long integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int64">long integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int64">long integer</see> marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0L, 1L, 10L, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1L, 1L, 10L, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5L, 1L, 10L, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1L, 1L, 1L, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10L, 1L, 10L, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15L, 1L, 10L, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(long,long,long)"/>
        /// <seealso cref="Between(long,long,long,string,object[])"/>		
        static public void Between(long test, long left, long right, string message) {
            long min = Math.Min(left, right);
            long max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int64">long integer</see>s, <paramref name="test"/> is between or 
        /// equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The <see cref="System.Int64">long integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int64">long integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int64">long integer</see> marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0L, 1L, 10L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1L, 1L, 10L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5L, 1L, 10L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1L, 1L, 1L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10L, 1L, 10L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15L, 1L, 10L,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(long,long,long)"/>
        /// <seealso cref="Between(long,long,long,string)"/>	
        static public void Between(long test, long left, long right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three doubles, <paramref name="test"/> is between or equal to one of 
        /// <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The double being tested</param>
        /// <param name="left">A double marking one of end of a range</param>
        /// <param name="right">A double marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0d, 1d, 10d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1d, 1d, 10d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5d, 1d, 10d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1d, 1d, 1d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10d, 1d, 10d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15d, 1d, 10d);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(double,double,double,string)"/>
        /// <seealso cref="Between(double,double,double,string,object[])"/>		 
        static public void Between(double test, double left, double right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three doubles, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The double being tested</param>
        /// <param name="left">A double marking one of end of a range</param>
        /// <param name="right">A double marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0d, 1d, 10d, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1d, 1d, 10d, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5d, 1d, 10d, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1d, 1d, 1d, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10d, 1d, 10d, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15d, 1d, 10d, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(double,double,double)"/>
        /// <seealso cref="Between(double,double,double,string,object[])"/>		
        static public void Between(double test, double left, double right, string message) {
            double min = Math.Min(left, right);
            double max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three doubles, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The double being tested</param>
        /// <param name="left">A double marking one of end of a range</param>
        /// <param name="right">A double marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0d, 1d, 10d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1d, 1d, 10d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5d, 1d, 10d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1d, 1d, 1d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10d, 1d, 10d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15d, 1d, 10d,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(double,double,double)"/>
        /// <seealso cref="Between(double,double,double,string)"/>	
        static public void Between(double test, double left, double right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three floats, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The float being tested</param>
        /// <param name="left">A float marking one of end of a range</param>
        /// <param name="right">A float marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0f, 1f, 10f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1f, 1f, 10f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5f, 1f, 10f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1f, 1f, 1f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10f, 1f, 10f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15f, 1f, 10f);
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(float,float,float,string)"/>
        /// <seealso cref="Between(float,float,float,string,object[])"/>		 
        static public void Between(float test, float left, float right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three floats, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The float being tested</param>
        /// <param name="left">A float marking one of end of a range</param>
        /// <param name="right">A float marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_4Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0f, 1f, 10f, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1f, 1f, 10f, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5f, 1f, 10f, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1f, 1f, 1f, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10f, 1f, 10f, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15f, 1f, 10f, "The test value is not within the range defined");
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(float,float,float)"/>
        /// <seealso cref="Between(float,float,float,string,object[])"/>		
        static public void Between(float test, float left, float right, string message) {
            float min = Math.Min(left, right);
            float max = Math.Max(left, right);

            Assert.IsTrue(test >= min, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test <= max, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three floats, <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The float being tested</param>
        /// <param name="left">A float marking one of end of a range</param>
        /// <param name="right">A float marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_5Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            Assert.Between(0f, 1f, 10f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            Assert.Between(1f, 1f, 10f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            Assert.Between(5f, 1f, 10f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            Assert.Between(1f, 1f, 1f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            Assert.Between(10f, 1f, 10f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            Assert.Between(15f, 1f, 10f,
        ///                "This test failed at {0}", DateTime.Now.ToShortDateString());
        ///        }
        ///    }        
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="Between(float,float,float)"/>
        /// <seealso cref="Between(float,float,float,string)"/>	
        static public void Between(float test, float left, float right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        /// <summary>
        /// Verifies that for three objects derived from <see cref="System.IComparable"/>, 
        /// <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The object derived from <see cref="System.IComparable"/> being tested</param>
        /// <param name="left">The object derived from <see cref="System.IComparable"/> marking one of end of a range</param>
        /// <param name="right">The object derived from <see cref="System.IComparable"/> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="test"/>, <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///    [TestFixture]
        ///    public class Between_IComparable {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, minute);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, second);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(minute, hour, second);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            Assert.Between(hour, hour, hour);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, hour, second);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, second, minute);
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void Between_MixedValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            string minute = "minute";
        ///            Assert.Between(1d, hour, minute);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Between(IComparable,IComparable,IComparable,string)"/>
        /// <seealso cref="Between(IComparable,IComparable,IComparable,string,object[])"/>		 
        static public void Between(IComparable test, IComparable left, IComparable right) {
            Between(test, left, right, null);
        }

        /// <summary>
        /// Verifies that for three objects derived from <see cref="System.IComparable"/>,
        /// <paramref name="test"/> is between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with the given <paramref name="message"/>.
        /// </summary>
        /// <param name="test">The object derived from <see cref="System.IComparable"/> being tested</param>
        /// <param name="left">The object derived from <see cref="System.IComparable"/> marking one of end of a range</param>
        /// <param name="right">The object derived from <see cref="System.IComparable"/> marking the other end of a range</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="test"/>, <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///    [TestFixture]
        ///    public class Between_IComparable {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, minute, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, second, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(minute, hour, second, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            Assert.Between(hour, hour, hour, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, hour, second, "The test value is not within the range defined");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, second, minute, "The test value is not within the range defined");
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void Between_MixedValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            string minute = "minute";
        ///            Assert.Between(1d, hour, minute, "The test value is not within the range defined");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Between(int,int,int)"/>
        /// <seealso cref="Between(int,int,int,string,object[])"/>		
        static public void Between(
            IComparable test,
            IComparable left,
            IComparable right,
            string message) {
            Assert.IsNotNull(test);
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);

            IComparable min = (left.CompareTo(right) <= 0) ? left : right;
            IComparable max = (right.CompareTo(left) >= 0) ? right : left;

            Assert.IsTrue(test.CompareTo(min) >= 0, "{0} is smaller than {1}" + (message == null ? "" : ", {2}"), test, min, message);
            Assert.IsTrue(test.CompareTo(max) <= 0, "{0} is greater than {1}" + (message == null ? "" : ", {2}"), test, max, message);
        }

        /// <summary>
        /// Verifies that for three objects derived from <see cref="System.IComparable"/>, <paramref name="test"/> is 
        /// between or equal to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If this is not the case, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="format"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="test">The object derived from <see cref="System.IComparable"/> being tested</param>
        /// <param name="left">The object derived from <see cref="System.IComparable"/> marking one of end of a range</param>
        /// <param name="right">The object derived from <see cref="System.IComparable"/> marking the other end of a range</param>
        /// <param name="format">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="test"/>, <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.Between using a different variety of values
        /// <code>		
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class Between_IComparable {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_LessThanMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, minute,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(second, hour, second,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_Between() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(minute, hour, second,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_AllSameValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            Assert.Between(hour, hour, hour,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Between_EqualToMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, hour, second,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Between_GreaterThanMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.Between(hour, second, minute,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void Between_MixedValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            string minute = "minute";
        ///            Assert.Between(1d, hour, minute,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="Between(IComparable,IComparable,IComparable)"/>
        /// <seealso cref="Between(IComparable,IComparable,IComparable,string)"/>	
        static public void Between(IComparable test, IComparable left, IComparable right,
            string format, params object[] args) {
            Between(test, left, right, String.Format(format, args));
        }

        #endregion

        #region NotBetween
        /// <summary>
        /// Verifies that for three integers, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The integer being tested</param>
        /// <param name="left">An integer marking one of end of a range</param>
        /// <param name="right">An integer marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15, 1, 10);
        ///        }
        /// 
        ///        //This test passes
        ///        [Test]
        ///        public void NotBetween_MixedValues() {
        ///            Assert.NotBetween(1f, 5, 10d);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(int test, int left, int right) {
            Assert.IncrementAssertCount();

            int min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            int max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);

        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int16">short integers</see>, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The <see cref="System.Int16">short integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int16">short integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int16">short integer</see> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15, 1, 10);
        ///        }
        /// 
        ///        //This test passes
        ///        [Test]
        ///        public void NotBetween_MixedValues() {
        ///            Assert.NotBetween(1f, 5, 10d);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(short test, short left, short right) {
            Assert.IncrementAssertCount();

            short min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            short max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Byte">bytes</see> (0 to 255), <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value between 0 and 255 being tested</param>
        /// <param name="left">A value between 0 and 255 marking one of end of a range</param>
        /// <param name="right">A value between 0 and 255 marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1, 1, 1);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5, 1, 10);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10, 1, 10);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15, 1, 10);
        ///        }
        /// 
        ///        //This test passes
        ///        [Test]
        ///        public void NotBetween_MixedValues() {
        ///            Assert.NotBetween(1f, 5, 10d);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(byte test, byte left, byte right) {
            Assert.IncrementAssertCount();

            byte min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            byte max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);
        }

        /// <summary>
        /// Verifies that for three <see cref="System.Int64">long integer</see>s, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The <see cref="System.Int64">long integer</see> being tested</param>
        /// <param name="left">A <see cref="System.Int64">long integer</see> marking one of end of a range</param>
        /// <param name="right">A <see cref="System.Int64">long integer</see> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1L, 1L, 1L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0L, 1L, 10L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1L, 1L, 10L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5L, 1L, 10L);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10L, 1L, 10L);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15L, 1L, 10L);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(long test, long left, long right) {
            Assert.IncrementAssertCount();

            long min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            long max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);

        }

        /// <summary>
        /// Verifies that for three doubles, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The double being tested</param>
        /// <param name="left">An double marking one of end of a range</param>
        /// <param name="right">An double marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1d, 1d, 1d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0d, 1d, 10d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1d, 1d, 10d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5d, 1d, 10d);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10d, 1d, 10d);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15d, 1d, 10d);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(double test, double left, double right) {
            Assert.IncrementAssertCount();

            double min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            double max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);
        }

        /// <summary>
        /// Verifies that for three floats, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The float being tested</param>
        /// <param name="left">An float marking one of end of a range</param>
        /// <param name="right">An float marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_3Arguments {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            Assert.NotBetween(1f, 1f, 1f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            Assert.NotBetween(0f, 1f, 10f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            Assert.NotBetween(1f, 1f, 10f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            Assert.NotBetween(5f, 1f, 10f);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            Assert.NotBetween(10f, 1f, 10f);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            Assert.NotBetween(15f, 1f, 10f);
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        static public void NotBetween(float test, float left, float right) {
            Assert.IncrementAssertCount();

            float min = Math.Min(left, right);
            if (test.CompareTo(min) < 0)
                return;

            float max = Math.Max(left, right);
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);
        }

        /// <summary>
        /// Verifies that for three objects derived from <see cref="System.IComparable"/>, <paramref name="test"/> is not in the range defined by or equal 
        /// to one of <paramref name="left"/> and <paramref name="right"/>.
        /// If it is in the defined range, a <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The object derived from <see cref="System.IComparable"/> being tested</param>
        /// <param name="left">The object derived from <see cref="System.IComparable"/> marking one of end of a range</param>
        /// <param name="right">The object derived from <see cref="System.IComparable"/> marking the other end of a range</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is within the range defined by <paramref name="left"/> or <paramref name="right"/> 
        /// or if one of the three values is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if <paramref name="test"/>, <paramref name="left"/> and <paramref name="right"/> are not of the same type</exception>
        /// <example>
        /// The following example demonstrates Assert.NotBetween using a different variety of values
        /// <code>	
        ///using MbUnit.Framework;
        ///using System;
        ///
        ///namespace MbUnitAssertDocs {
        ///    [TestFixture]
        ///    public class NotBetween_IComparable {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_LessThanMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.NotBetween(second, hour, minute);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMin() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.NotBetween(second, hour, second);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_Between() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.NotBetween(minute, hour, second);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_AllSameValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            Assert.NotBetween(hour, hour, hour);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotBetween_EqualToMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.NotBetween(hour, hour, second);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotBetween_GreaterThanMax() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            TimeSpan minute = new TimeSpan(0, 1, 0);
        ///            TimeSpan second = new TimeSpan(0, 0, 1);
        ///            Assert.NotBetween(hour, second, minute);
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void NotBetween_MixedValues() {
        ///            TimeSpan hour = new TimeSpan(1, 0, 0);
        ///            string minute = "minute";
        ///            Assert.NotBetween(1d, hour, minute);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        static public void NotBetween(IComparable test, IComparable left, IComparable right) {
            Assert.IsNotNull(test);
            Assert.IsNotNull(left);
            Assert.IsNotNull(right);

            IComparable min = (left.CompareTo(right) <= 0) ? left : right;
            IComparable max = (right.CompareTo(left) >= 0) ? right : left;

            if (test.CompareTo(min) < 0)
                return;
            if (test.CompareTo(max) > 0)
                return;

            Assert.Fail("{0} is in {1} - {2}", test, min, max);
        }
        #endregion

        #region In, NotIn
        /// <summary>
        /// Verifies that <paramref name="dic" />, which derives from <see cref="System.Collections.IDictionary" />, contains an item with the key value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The key value that <paramref name="dic" /> should contain</param>
        /// <param name="dic">The <see cref="System.Collections.IDictionary" /> object that should contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="dic" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections.Generic;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_Dictionary {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_DictionaryContainsKey()
        ///        {
        ///            Dictionary&lt;int, string&gt; container = 
        ///                new Dictionary&lt;int, string&gt; {{1, "testing"}};
        ///            Assert.In(1, container);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_DictionaryDoesNotContainKey() {
        ///            Dictionary&lt;int, string&gt; container =
        ///                new Dictionary&lt;int, string&gt; { { 1, "testing" } };
        ///            Assert.In(2, container);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IDictionary,string)"/>		
        static public void In(Object test, IDictionary dic) {
            In(test, dic, null);
        }

        /// <summary>
        /// Verifies that <paramref name="dic" />, which derives from <see cref="System.Collections.IDictionary" />, contains an item with the key value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message" />. 
        /// </summary>
        /// <param name="test">The key value that <paramref name="dic" /> should contain</param>
        /// <param name="dic">The <see cref="System.Collections.IDictionary" /> object that should contain <paramref name="test" /></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="dic" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections.Generic;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_Dictionary {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_DictionaryContainsKey()
        ///        {
        ///            Dictionary&lt;int, string&gt; container = 
        ///                new Dictionary&lt;int, string&gt; {{1, "testing"}};
        ///            Assert.In(1, container, "The dictionary object does not contain this key");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_DictionaryDoesNotContainKey() {
        ///            Dictionary&lt;int, string&gt; container =
        ///                new Dictionary&lt;int, string&gt; { { 1, "testing" } };
        ///            Assert.In(2, container, "The dictionary object does not contain this key");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IDictionary)"/>		
        static public void In(Object test, IDictionary dic, string message) {
            Assert.IsNotNull(test, "tested object is null");
            Assert.IsNotNull(dic, "Dictionary is a null reference");
            Assert.IsTrue(dic.Contains(test),
                          "Dictionary does not contain {0} {1}", test, message);
        }

        /// <summary>
        /// Verifies that <paramref name="list" />, which derives from <see cref="System.Collections.IList" />, contains a value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value that <paramref name="list" /> should contain</param>
        /// <param name="list">The <see cref="System.Collections.IList" /> object that should contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="list" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_List {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_ArrayContainsValue()
        ///        {
        ///            int[] array = {9, 10};
        ///            Assert.In(9, array);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_ArrayDoesNotContainValue() {
        ///            int[] array = { 9, 10 };
        ///            Assert.In(5, array);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IList,string)"/>
        static public void In(Object test, IList list) {
            In(test, list, null);
        }

        /// <summary>
        /// Verifies that <paramref name="list" />, which derives from <see cref="System.Collections.IList" />, contains a value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with a given <paramref name="message" />. 
        /// </summary>
        /// <param name="test">The value that <paramref name="list" /> should contain</param>
        /// <param name="list">The <see cref="System.Collections.IList" /> object that should contain <paramref name="test" /></param>
        /// <param name="message">The message to display on failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="list" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_List {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_ArrayContainsValue()
        ///        {
        ///            int[] array = {9, 10};
        ///            Assert.In(9, array, "The array does not contain this value");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_ArrayDoesNotContainValue() {
        ///            int[] array = { 9, 10 };
        ///            Assert.In(5, array, "The array does not contain this value");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IList)"/>
        static public void In(Object test, IList list, string message) {
            Assert.IsNotNull(list, "List is a null reference");
            Assert.IsTrue(list.Contains(test),
                          "List does not contain {0} {1}", test, message);
        }

        /// <summary>
        /// Verifies that <paramref name="enumerable" />, which derives from <see cref="System.Collections.IEnumerable" />, 
        /// contains a value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>. 
        /// </summary>
        /// <param name="test">The value that <paramref name="enumerable" /> should contain</param>
        /// <param name="enumerable">The <see cref="System.Collections.IEnumerable" /> object that should contain <paramref name="test" /></param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="enumerable" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_Enumerable {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_StackContainsValue()
        ///        {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.In("test", stack, "The stack does not contain the value");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_ArrayDoesNotContainValue() {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.In(212, stack, "The stack does not contain the value");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IEnumerable)"/>
        static public void In(Object test, IEnumerable enumerable, string message) {
            Assert.IsNotNull(enumerable, "Enumerable collection is a null reference");
            foreach (Object o in enumerable) {
                if (object.Equals(o, test))
                    return;
            }
            Assert.Fail("Collection does not contain {0} {1}", test, message);
        }

        /// <summary>
        /// Verifies that <paramref name="enumerable" />, which derives from <see cref="System.Collections.IEnumerable" />, 
        /// contains a value of <paramref name="test"/>.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value that <paramref name="enumerable" /> should contain</param>
        /// <param name="enumerable">The <see cref="System.Collections.IEnumerable" /> object that should contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is not contained in <paramref name="enumerable" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.In
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class In_Enumerable {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void In_StackContainsValue()
        ///        {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.In("test", stack);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void In_ArrayDoesNotContainValue() {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.In(212, stack);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="In(Object,IEnumerable,string)"/>
        static public void In(Object test, IEnumerable enumerable) {
            In(test, enumerable, null);
        }

        /// <summary>
        /// Verifies that <paramref name="dic" />, which derives from <see cref="System.Collections.IDictionary" />, 
        /// does not contains an item with the key value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message" />. 
        /// </summary>
        /// <param name="test">The key value that <paramref name="dic" /> should not contain</param>
        /// <param name="dic">The <see cref="System.Collections.IDictionary" /> object that should not contain <paramref name="test" /></param>
        /// <param name="message">The message printed out upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="dic" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections.Generic;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_Dictionary {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_DictionaryContainsKey()
        ///        {
        ///            Dictionary&lt;int, string&gt; container = 
        ///                new Dictionary&lt;int, string&gt; {{1, "testing"}};
        ///            Assert.NotIn(1, container, "The dictionary object contains this key");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_DictionaryDoesNotContainKey() {
        ///            Dictionary&lt;int, string&gt; container =
        ///                new Dictionary&lt;int, string&gt; { { 1, "testing" } };
        ///            Assert.NotIn(2, container, "The dictionary object contains this key");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IDictionary)"/>		
        static public void NotIn(Object test, IDictionary dic, string message) {
            Assert.IsNotNull(test);
            Assert.IsNotNull(dic, "Dictionary is a null reference");
            Assert.IsFalse(dic.Contains(test),
                          "Dictionary does contain {0} {1}", test, message);
        }

        /// <summary>
        /// Verifies that <paramref name="dic" />, which derives from <see cref="System.Collections.IDictionary" />, 
        /// does not contain an item with the key value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The key value that <paramref name="dic" /> should not contain</param>
        /// <param name="dic">The <see cref="System.Collections.IDictionary" /> object that should not contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="dic" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections.Generic;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_Dictionary {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_DictionaryContainsKey()
        ///        {
        ///            Dictionary&lt;int, string&gt; container = 
        ///                new Dictionary&lt;int, string&gt; {{1, "testing"}};
        ///            Assert.NotIn(1, container);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_DictionaryDoesNotContainKey() {
        ///            Dictionary&lt;int, string&gt; container =
        ///                new Dictionary&lt;int, string&gt; { { 1, "testing" } };
        ///            Assert.NotIn(2, container);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IDictionary,string)"/>		
        static public void NotIn(Object test, IDictionary dic) {
            NotIn(test, dic, null);
        }

        /// <summary>
        /// Verifies that <paramref name="list" />, which derives from <see cref="System.Collections.IList" />, does not contain a value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with a given <paramref name="message" />. 
        /// </summary>
        /// <param name="test">The value that <paramref name="list" /> should not contain</param>
        /// <param name="list">The <see cref="System.Collections.IList" /> object that should not contain <paramref name="test" /></param>
        /// <param name="message">The message to display on failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="list" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_List {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_ArrayContainsValue()
        ///        {
        ///            int[] array = { 9, 10 };
        ///            Assert.NotIn(9, array, "The array contains this value");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_ArrayDoesNotContainValue() {
        ///            int[] array = { 9, 10 };
        ///            Assert.NotIn(5, array, "The array contains this value");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IList)"/>
        static public void NotIn(Object test, IList list, string message) {
            Assert.IsNotNull(list, "List is a null reference");
            Assert.IsFalse(list.Contains(test),
                          "List does contain {0} {1}", test, message);
        }

        /// <summary>
        /// Verifies that <paramref name="list" />, which derives from <see cref="System.Collections.IList" />, does not contains a value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value that <paramref name="list" /> should not contain</param>
        /// <param name="list">The <see cref="System.Collections.IList" /> object that should not contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="list" /> or either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_List {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_ArrayContainsValue()
        ///        {
        ///            int[] array = {9, 10};
        ///            Assert.NotIn(9, array);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_ArrayDoesNotContainValue() {
        ///            int[] array = { 9, 10 };
        ///            Assert.NotIn(5, array);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IList,string)"/>
        static public void NotIn(Object test, IList list) {
            NotIn(test, list, null);
        }

        /// <summary>
        /// Verifies that <paramref name="enumerable" />, which derives from <see cref="System.Collections.IEnumerable" />, 
        /// does not contain a value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>. 
        /// </summary>
        /// <param name="test">The value that <paramref name="enumerable" /> should not contain</param>
        /// <param name="enumerable">The <see cref="System.Collections.IEnumerable" /> object that should not contain <paramref name="test" /></param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="enumerable" /> or if either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_Enumerable {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_StackContainsValue()
        ///        {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.NotIn("test", stack, "The stack contains the value");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_ArrayDoesNotContainValue() {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.NotIn(212, stack, "The stack contains the value");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IEnumerable)"/>
        static public void NotIn(Object test, IEnumerable enumerable, string message) {
            Assert.IsNotNull(enumerable, "Enumerable collection is a null reference");
            foreach (Object o in enumerable) {
                Assert.IsFalse(o == test, "{0} is part of the enumeration {1}", test, message);
            }
        }

        /// <summary>
        /// Verifies that <paramref name="enumerable" />, which derives from <see cref="System.Collections.IEnumerable" />, 
        /// does not contain a value of <paramref name="test"/>.
        /// If this is not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="test">The value that <paramref name="enumerable" /> should not contain</param>
        /// <param name="enumerable">The <see cref="System.Collections.IEnumerable" /> object that should not contain <paramref name="test" /></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="test"/> is contained in <paramref name="enumerable" /> or if either object is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.NotIn
        /// <code>		
        ///using MbUnit.Framework;
        ///using System.Collections;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class NotIn_Enumerable {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void NotIn_StackContainsValue()
        ///        {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.NotIn("test", stack);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void NotIn_ArrayDoesNotContainValue() {
        ///            Stack stack = new Stack();
        ///            stack.Push(9);
        ///            stack.Push("test");
        ///            Assert.NotIn(212, stack);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="NotIn(Object,IEnumerable)"/>
        static public void NotIn(Object test, IEnumerable enumerable) {
            NotIn(test, enumerable, null);
        }
        #endregion

        #region IsEmpty
        //NUnit Code
        //Fails if it is not empty

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="aString">The string that should be empty</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_String {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyString()
        ///        {
        ///            Assert.IsEmpty(string.Empty,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullString()
        ///        {
        ///            Assert.IsEmpty(null as string,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsEmpty("Full",
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(string)"/>
        /// <seealso cref="IsEmpty(string,string)"/>
        public static void IsEmpty(string aString, string message, params object[] args) {
            if (aString != "" || !aString.Equals(string.Empty)) {
                if (args != null)
                    Assert.FailIsNotEmpty(aString, message, args);
                else
                    Assert.FailIsNotEmpty(aString, message);
            }
        }

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="aString">The string that should be empty</param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_String {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyString()
        ///        {
        ///            Assert.IsEmpty(string.Empty, "This string is not empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullString()
        ///        {
        ///            Assert.IsEmpty(null as string, "This string is not empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsEmpty("Full", "This string is not empty");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(string)"/>
        /// <seealso cref="IsEmpty(string,string,object[])"/>
        public static void IsEmpty(string aString, string message) {
            IsEmpty(aString, message, null);
        }

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="aString">The string that should be empty</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_String {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyString()
        ///        {
        ///            Assert.IsEmpty(string.Empty);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullString()
        ///        {
        ///            Assert.IsEmpty(null as string);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsEmpty("Full");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(string,string)"/>
        /// <seealso cref="IsEmpty(string,string,object[])"/>
        public static void IsEmpty(string aString) {
            IsEmpty(aString, string.Empty, null);
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="collection">The collection that should be empty</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_Collection {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(ICollection)"/>
        /// <seealso cref="IsEmpty(ICollection, string)"/>
        public static void IsEmpty(ICollection collection, string message, params object[] args) {
            if (collection.Count != 0) {
                if (args != null)
                    Assert.FailIsNotEmpty(collection, message, args);
                else
                    Assert.FailIsNotEmpty(collection, message);
            }
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>. 
        /// </summary>
        /// <param name="collection">The collection that should be empty</param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_Collection {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsEmpty(array, "This array is not empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsEmpty(array, "This array is not empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsEmpty(array, "This array is not empty");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(ICollection)"/>
        /// <seealso cref="IsEmpty(ICollection, string, object[])"/>
        public static void IsEmpty(ICollection collection, string message) {
            IsEmpty(collection, message, null);
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="collection">The collection that should be empty</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is not empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsEmpty_Collection {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsEmpty(array);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsEmpty(array);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsEmpty(array);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsEmpty(ICollection, string)"/>
        /// <seealso cref="IsEmpty(ICollection, string, object[])"/>
        public static void IsEmpty(ICollection collection) {
            IsEmpty(collection, string.Empty, null);
        }
        #endregion

        #region IsNotEmpty
        //NUnit Code

        //Fail when it is empty

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is not equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="aString">The string that should not be empty</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_String {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyString()
        ///        {
        ///            Assert.IsNotEmpty(string.Empty,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullString()
        ///        {
        ///            Assert.IsNotEmpty(null as string,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsNotEmpty("Full",
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(string)"/>
        /// <seealso cref="IsNotEmpty(string,string)"/>
        public static void IsNotEmpty(string aString, string message, params object[] args) {
            if (aString == "" || aString.Equals(string.Empty)) {
                if (args != null)
                    Assert.FailIsEmpty(aString, message, args);
                else
                    Assert.FailIsEmpty(aString, message);
            }
        }

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is not equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/> 
        /// </summary>
        /// <param name="aString">The string that should not be empty</param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_String {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyString()
        ///        {
        ///            Assert.IsNotEmpty(string.Empty, "This string is empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullString()
        ///        {
        ///            Assert.IsNotEmpty(null as string, "This string is empty");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsNotEmpty("Full", "This string is empty");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(string)"/>
        /// <seealso cref="IsNotEmpty(string,string,object[])"/>
        public static void IsNotEmpty(string aString, string message) {
            IsNotEmpty(aString, message, null);
        }

        /// <summary>
        /// Verifies that the string <paramref name="aString" /> is not equal to string.Empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="aString">The string that should not be empty</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aString"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_String {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyString()
        ///        {
        ///            Assert.IsNotEmpty(string.Empty);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullString()
        ///        {
        ///            Assert.IsNotEmpty(null as string);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyString()
        ///        {
        ///            Assert.IsNotEmpty("Full");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(string,string)"/>
        /// <seealso cref="IsNotEmpty(string,string,object[])"/>
        public static void IsNotEmpty(string aString) {
            IsNotEmpty(aString, string.Empty, null);
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is not empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="collection">The collection that should not be empty</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_Collection {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsNotEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsNotEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsNotEmpty(array,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(ICollection)"/>
        /// <seealso cref="IsNotEmpty(ICollection, string)"/>
        public static void IsNotEmpty(ICollection collection, string message, params object[] args) {
            if (collection.Count == 0) {
                if (args != null)
                    Assert.FailIsEmpty(collection, message, args);
                else
                    Assert.FailIsEmpty(collection, message);
            }
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is not empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given <paramref name="message"/>. 
        /// </summary>
        /// <param name="collection">The collection that should not be empty</param>
        /// <param name="message">The message given upon failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_Collection {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsNotEmpty(array, "This array is not empty");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsNotEmpty(array, "This array is not empty");
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsNotEmpty(array, "This array is not empty");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(ICollection)"/>
        /// <seealso cref="IsNotEmpty(ICollection, string, object[])"/>
        public static void IsNotEmpty(ICollection collection, string message) {
            IsNotEmpty(collection, message, null);
        }

        /// <summary>
        /// Verifies that the object <paramref name="collection" /> derived from <see cref="System.Collections.ICollection" /> is not empty.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="collection">The collection that should not be empty</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="collection"/> is empty or is null.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNotEmpty
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class IsNotEmpty_Collection {
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_EmptyCollection() {
        ///            int[] array = {};
        ///            Assert.IsNotEmpty(array);
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void IsNotEmpty_NullCollection() {
        ///            int[] array = null;
        ///            Assert.IsNotEmpty(array);
        ///        }
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void IsNotEmpty_NonEmptyCollection() {
        ///            int[] array = {9, 10};
        ///            Assert.IsNotEmpty(array);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNotEmpty(ICollection, string)"/>
        /// <seealso cref="IsNotEmpty(ICollection, string, object[])"/>
        public static void IsNotEmpty(ICollection collection) {
            IsNotEmpty(collection, string.Empty, null);
        }
        #endregion

        #region IsNaN
        //NUnit Code
        /// <summary>
        /// Verifies that the double value <paramref name="aDouble" /> is passed is an <c>NaN</c> value.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown 
        /// with a message defined via <paramref name="message"/> and <paramref name="args"/>
        /// through <see cref="System.String.Format(String, Object[]) " />.
        /// </summary>
        /// <param name="aDouble">The value to be tested</param>
        /// <param name="message">A <a href="http://msdn2.microsoft.com/en-gb/library/txafckwd.aspx">composite format string</a></param>
        /// <param name="args">An <see cref="Object"/> array containing zero or more objects to format.</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aDouble"/> is not <c>NaN</c>.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNaN
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class IsNaN
        ///    {
        ///        // This test succeeds
        ///        [Test]
        ///        public void IsNaN_NaN()
        ///        {
        ///            Assert.IsNaN(double.NaN,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void IsNaN_NotNaN()
        ///        {
        ///            Assert.IsNaN(1d,
        ///                "The test failed at {0}", DateTime.Now.ToLongDateString());
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNaN(double)"/>
        /// <seealso cref="IsNaN(double, string)"/>
        static public void IsNaN(double aDouble, string message, params object[] args) {
            if (!double.IsNaN(aDouble)) {
                Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the double value <paramref name="aDouble" /> is passed is an <c>NaN</c> value.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown with the given message.
        /// </summary>
        /// <param name="aDouble">The value to be tested</param>
        /// <param name="message">The message to be given on failure</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aDouble"/> is not <c>NaN</c>.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNaN
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class IsNaN
        ///    {
        ///        // This test succeeds
        ///        [Test]
        ///        public void IsNaN_NaN()
        ///        {
        ///            Assert.IsNaN(double.NaN, "The double is not NaN");
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void IsNaN_NotNaN()
        ///        {
        ///            Assert.IsNaN(1d, "The double is not NaN");
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNaN(double)"/>
        /// <seealso cref="IsNaN(double, string, object[])"/>
        static public void IsNaN(double aDouble, string message) {
            if (!double.IsNaN(aDouble)) {
                Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the double value <paramref name="aDouble" /> is passed is an <c>NaN</c> value.
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// </summary>
        /// <param name="aDouble">The value to be tested</param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="aDouble"/> is not <c>NaN</c>.</exception>
        /// <example>
        /// The following example demonstrates Assert.IsNaN
        /// <code>		
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs
        ///{
        ///    [TestFixture]
        ///    public class IsNaN
        ///    {
        ///        // This test succeeds
        ///        [Test]
        ///        public void IsNaN_NaN()
        ///        {
        ///            Assert.IsNaN(double.NaN);
        ///        }
        ///
        ///        //This test fails
        ///        [Test]
        ///        public void IsNaN_NotNaN()
        ///        {
        ///            Assert.IsNaN(1d);
        ///        }
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <seealso cref="IsNaN(double, string)"/>
        /// <seealso cref="IsNaN(double, string, object[])"/>
        static public void IsNaN(double aDouble) {
            Assert.IsNaN(aDouble, string.Empty);
        }

        #endregion

        #region Contains

        /// <summary>
        /// Verifies that <paramref name="s"/> is a substring of <paramref name="contain"/>. 
        /// </summary>
        /// If this not the case, an <see cref="MbUnit.Core.Exceptions.AssertionException"/> is thrown. 
        /// <param name="s">The string that should be contained within <paramref name="contain"/></param>
        /// <param name="contain">The string that should contain <paramref name="s"/></param>
        /// <exception cref="MbUnit.Core.Exceptions.AssertionException">
        /// Thrown if <paramref name="s"/> is not a substring of <paramref name="contain"/>.</exception>
        /// <example>
        /// The following example demonstrates Assert.Contains
        /// <code>
        ///using MbUnit.Framework;
        ///
        ///namespace MbUnitAssertDocs {
        ///
        ///    [TestFixture]
        ///    public class Contains {
        ///
        ///        // This test passes
        ///        [Test]
        ///        public void Contains_StringContainsString()
        ///        {
        ///            Assert.Contains("Who are you?", "are");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Contains_StringDoesNotContainString() 
        ///        {
        ///            Assert.Contains("Who are you?", "where");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Contains_LeftIsNull()
        ///        {
        ///            Assert.Contains(null, "are");
        ///        }
        ///
        ///        // This test fails
        ///        [Test]
        ///        public void Contains_RightIsNull()
        ///        {
        ///            Assert.Contains("Who are you", null);
        ///        }
        ///    }
        ///} 
        /// </code>
        /// </example>
        static public void Contains(string s, string contain) {
            Assert.IsTrue(s.IndexOf(contain) >= 0, "String [[{0}]] does not contain [[{1}]]",
                s, contain);
        }

        #endregion

        #region TypeAssert
        //Type Asserts modified from NUnit Code.
        #region IsAssignableFrom
        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        static public void IsAssignableFrom(System.Type expected, object actual) {
            IsAssignableFrom(expected, actual, "");
        }

        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The messge to display in case of failure</param>
        static public void IsAssignableFrom(System.Type expected, object actual, string message) {
            IsAssignableFrom(expected, actual, message, null);
        }

        /// <summary>
        /// Asserts that an object may be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        static public void IsAssignableFrom(System.Type expected, object actual, string message, params object[] args) {
            if (!actual.GetType().IsAssignableFrom(expected)) {
                Assert.Fail(message);
            }
        }
        #endregion

        #region IsNotAssignableFrom
        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        static public void IsNotAssignableFrom(System.Type expected, object actual) {
            IsNotAssignableFrom(expected, actual, "");
        }

        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The messge to display in case of failure</param>
        static public void IsNotAssignableFrom(System.Type expected, object actual, string message) {
            IsNotAssignableFrom(expected, actual, message, null);
        }

        /// <summary>
        /// Asserts that an object may not be assigned a  value of a given Type.
        /// </summary>
        /// <param name="expected">The expected Type.</param>
        /// <param name="actual">The object under examination</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        static public void IsNotAssignableFrom(System.Type expected, object actual, string message, params object[] args) {

            bool needToFail = false;
            try {
                IsAssignableFrom(expected, actual, message, args);
                needToFail = true;
            } catch (Core.Exceptions.AssertionException) {
                //Do Nothing as expected
            }

            if (needToFail) {
                Assert.Fail(message, args);
            }

        }
        #endregion

        #region IsInstanceOfType
        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        public static void IsInstanceOfType(System.Type expected, object actual) {
            IsInstanceOfType(expected, actual, string.Empty, null);
        }

        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">A message to display in case of failure</param>
        public static void IsInstanceOfType(System.Type expected, object actual, string message) {
            IsInstanceOfType(expected, actual, message, null);
        }

        /// <summary>
        /// Asserts that an object is an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">A message to display in case of failure</param>
        /// <param name="args">An array of objects to be used in formatting the message</param>
        public static void IsInstanceOfType(System.Type expected, object actual, string message, params object[] args) {
            if (!expected.IsInstanceOfType(actual)) {
                Assert.Fail(message);
            }
        }
        #endregion

        #region IsNotInstanceOfType
        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        public static void IsNotInstanceOfType(System.Type expected, object actual) {
            IsNotInstanceOfType(expected, actual, string.Empty, null);
        }

        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">A message to display in case of failure</param>
        public static void IsNotInstanceOfType(System.Type expected, object actual, string message) {
            IsNotInstanceOfType(expected, actual, message, null);
        }

        /// <summary>
        /// Asserts that an object is not an instance of a given type.
        /// </summary>
        /// <param name="expected">The expected Type</param>
        /// <param name="actual">The object being examined</param>
        /// <param name="message">A message to display in case of failure</param>
        /// <param name="args">An array of objects to be used in formatting the message</param>
        public static void IsNotInstanceOfType(System.Type expected, object actual, string message, params object[] args) {

            bool needToFail = false;

            try {
                IsInstanceOfType(expected, actual, message, args);

                needToFail = true;
            } catch (Core.Exceptions.AssertionException) {
                //Do Nothing as expected
            }

            if (needToFail) {
                Assert.Fail(message, args);
            }

        }
        #endregion

        #endregion

        #region Assert Count
        /// <summary>
        /// Number of Asserts made so far this test run
        /// </summary>
        public static int AssertCount {
            get { return Assert.assertCount; }
        }

        /// <summary>
        /// Resets <see cref="AssertCount"/> to 0
        /// </summary>
        public static void ResetAssertCount() {
            Assert.assertCount = 0;
        }

        /// <summary>
        /// Increments <see cref="AssertCount"/> by 1
        /// </summary>
        public static void IncrementAssertCount() {
            Assert.assertCount++;
        }
        #endregion

        #region Warnings
        public static void Warning(string format, params object[] args) {
            if (format == null)
                throw new ArgumentNullException("format");

            string message = String.Format(format, args);
            Warning(message);
        }

        public static void Warning(string message) {
            if (message == null)
                throw new ArgumentNullException("message");
            ReportWarning warning = new ReportWarning();
            warning.Text = message;
            warnings.Add(warning);
        }
        internal static void ClearCounters() {
            warnings.Clear();
            assertCount = 0;
        }
        internal static void FlushWarnings(ReportRun run) {
            if (run != null) {
                foreach (ReportWarning warning in warnings)
                    run.Warnings.AddReportWarning(warning);
            }
            warnings.Clear();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// This method is called when two objects have been compared and found to be
        /// different. This prints a nice message to the screen.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="format">
        /// The format of the message to display if the assertion fails,
        /// containing zero or more format items.
        /// </param>
        /// <param name="args">
        /// An <see cref="Object"/> array containing zero or more objects to format.
        /// </param>
        /// <remarks>
        /// <para>
        /// The error message is formatted using <see cref="String.Format(string, object[])"/>.
        /// </para>
        /// </remarks>
        static private void FailNotEquals(Object expected, Object actual, string format, params object[] args) {
            throw new NotEqualAssertionException(expected, actual,
                String.Format(format, args));
        }

        /// <summary>
        ///  This method is called when the two objects are not the same.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="format">
        /// The format of the message to display if the assertion fails,
        /// containing zero or more format items.
        /// </param>
        /// <param name="args">
        /// An <see cref="Object"/> array containing zero or more objects to format.
        /// </param>
        /// <remarks>
        /// <para>
        /// The error message is formatted using <see cref="String.Format(string, object[])"/>.
        /// </para>
        /// </remarks>
        static private void FailNotSame(Object expected, Object actual, string format, params object[] args) {
            string formatted = string.Empty;
            if (format != null)
                formatted = format + " ";
            Assert.Fail(format + "The two objects were expected to be the same", args);
        }

        static private void FailSame(Object expected, Object actual, string format, params object[] args) {
            string formatted = string.Empty;
            if (format != null)
                formatted = format + " ";
            Assert.Fail(format + "The two objects were not expected to be the same", args);
        }


        static private void FailIsEmpty(Object expected, string format, params object[] args) {
            string formatted = string.Empty;
            if (format != null)
                formatted = format + " ";
            Assert.Fail(format + "The object was expected to be empty but was not", args);
        }

        static private void FailIsNotEmpty(Object expected, string format, params object[] args) {
            string formatted = string.Empty;
            if (format != null)
                formatted = format + " ";
            Assert.Fail(format + "The object was expected not to be empty but was", args);
        }
        #endregion
    }
}




