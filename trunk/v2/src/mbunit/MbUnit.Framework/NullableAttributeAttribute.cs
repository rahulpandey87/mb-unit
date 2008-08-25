using System;
using System.Reflection;

namespace MbUnit.Framework {
    using MbUnit.Core;

    /// <summary>
    /// Tag your assembly with this to identify the class you've created to identify parameters 
    /// in your test methods that can be null.
    /// </summary>
    /// <remarks>
    /// MbUnit checks that test case parameter values are not null before it executes tests and throws 
    /// an ArgumentNullException if this is the case. Now there may be cases where arguments can be nullable
    /// but you need to identify them to MbUnit by tagging them as nullable and then pointing to MbUnit
    /// the tag that marks the parameter as nullable
    /// </remarks>
    /// <example>
    /// <para>
    /// To identify a test method with nullable parameters, first create a simple attribute that targets properties.
    /// </para>
    /// <code>
    /// [AttributeUsage(AttributeTargets.Parameter, AllowMultiple=false, Inherited=true)]
    /// public class ThisIsNullableAttribute : Attribute
    /// {}
    /// </code>
    /// <para>Now tag a test method's parameters with the new attribute as appropriate and add it to a test suite</para>
    /// <code>
    /// public class SomeTestCases
    /// {
    ///     public void NullableTest(
    ///         [ThisIsNullable]object testThis)
    ///     { ... }
    /// }
    /// 
    /// [TestSuite]
    /// public ITestSuite MyTestSuite()
    ///{
    ///    SomeTestCases tcs = new SomeTestCases();
    ///    ClassTester suite = new ClassTester("SuiteOfSomeTests",tcs);
    ///
    ///    suite.Add("NullableTest","hello");
    ///    suite.Add("NullableTest", null);
    ///
    ///    return suite.Suite;
    ///}
    /// </code>
    /// <para>And finally use the NullableAttributeAttribute to tag the assembly and identify the <c>[ThisIsNullable]</c> attribute
    /// as the one which identifies nullable parameters</para>
    /// <code>[assembly: NullableAttribute(typeof(ThisIsNullableAttribute))]</code>
    /// </example>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class NullableAttributeAttribute : Attribute {
        private Type nullableType;
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableAttributeAttribute"/> class.
        /// </summary>
        /// <param name="nullableType">Type of the attribute class to identify nullable parameters</param>
        public NullableAttributeAttribute(Type nullableType) {
            if (nullableType == null)
                throw new ArgumentNullException("nullableType");
            this.nullableType = nullableType;
        }

        /// <summary>
        /// Gets or sets the type of the attribute class to identify nullable parameters
        /// </summary>
        /// <value>The type of the attribute class to identify nullable parameters</value>
        public Type NullableType {
            get {
                return this.nullableType;
            }
            set {
                this.nullableType = value;
            }
        }

        /// <summary>
        /// Returns the type of the <see cref="NullableAttributeAttribute"/> for the assembly containing the current method
        /// </summary>
        /// <param name="t">The class containing the method being tested</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="t"/> is null</exception>
        public static NullableAttributeAttribute GetAttribute(Type t) {
            if (t == null)
                throw new ArgumentNullException("t");
            Assembly a = t.Assembly;
            NullableAttributeAttribute attribute =
                (NullableAttributeAttribute)TypeHelper.TryGetFirstCustomAttribute(a, typeof(NullableAttributeAttribute));
            return attribute;
        }
    }
}
