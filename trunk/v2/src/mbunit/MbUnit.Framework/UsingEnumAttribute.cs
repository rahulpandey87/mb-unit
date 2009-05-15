﻿using System;
using MbUnit.Framework;
using TestFu.Operations;
using System.Reflection;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a parameter within a [CombinatorialTest]-tagged method to indicate that it should
    /// use values from the named enumeration
    /// </summary>
    /// <example>
    /// <para>In the following example, TestEnumValues is run three times against each of the values in the FooEnum enumeration</para>
    /// <code>
    ///     [TestFixture]
    ///     public class UsingEnum {
    /// 
    ///         [CombinatorialTest]
    ///         public void TestEnumValues([UsingEnum(typeof(FooEnum))] FooEnum item) 
    ///         {
    ///             StringAssert.AreEqualIgnoreCase("a", item.ToString());
    ///         }
    ///     }
    /// 
    ///     public enum FooEnum {
    ///         A, B, C
    ///     }
    /// </code>
    /// </example>
    /// <seealso cref="CombinatorialTestAttribute"/>
    /// <seealso cref="UsingBaseAttribute"/>
    /// <seealso cref="UsingFactoriesAttribute"/>
    /// <seealso cref="UsingImplementationsAttribute"/>
    /// <seealso cref="UsingLinearAttribute"/>
    /// <seealso cref="UsingLiteralsAttribute"/>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class UsingEnumAttribute : UsingBaseAttribute {
        private Type enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingEnumAttribute"/> class.
        /// </summary>
        /// <param name="enumType"><see cref="Type"/> of the enumeration containing the values for use by the parameter</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumType"/> is null</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="enumType"/> is not an enumeration</exception>
        public UsingEnumAttribute(Type enumType) {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum)
                throw new ArgumentException("Type " + enumType.FullName + " is not a enum", "enumType");
            this.enumType = enumType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the enumeration containing the values for use by the parameter
        /// </summary>
        /// <value>The <see cref="Type"/> of the enumeration containing the values for use by the parameter</value>
        public Type EnumType {
            get { return this.enumType; }
        }

        /// <summary>
        /// Gets the set of values (the collection of domains) for the parameter.
        /// </summary>
        /// <param name="domains">The <see cref="IDomainCollection"/> the values generated by the source of data</param>
        /// <param name="parameter"><see cref="ParameterInfo"/> for the parameter that wants the values.</param>
        /// <param name="fixture">The test fixture.</param>
        /// <remarks>See <a href="http://blog.dotnetwiki.org/CombinatorialTestingWithTestFu1.aspx">here</a> for more on
        /// domain generation</remarks>
        public override void GetDomains(
            IDomainCollection domains,
            ParameterInfo parameter,
            object fixture) {
            ArrayDomain domain = new ArrayDomain(Enum.GetValues(this.EnumType));
            domains.Add(domain);
        }
    }
}