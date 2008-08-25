using System;
using TestFu.Operations;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a parameter within a [CombinatorialTest]-tagged method to indicate that it should
    /// use values from provided by the named factory method(s)
    /// </summary>
    /// <example>
    /// <para>In the following example, TestFactoryValues is run three times against each of the values returned 
    /// by the GetValues method which has been tagged as a Factory method</para>
    /// <code>
    ///     [TestFixture]
    ///     public class UsingFactories {
    /// 
    ///         [CombinatorialTest]
    ///         public void TestEnumValues(
    ///             [UsingFactories("GetValues")] string item) {
    ///             StringAssert.AreEqualIgnoreCase("a", item);
    ///         }
    /// 
    ///         [Factory(typeof(string))]
    ///         public IEnumerable GetValues() {
    ///             yield return "A";
    ///             yield return "B";
    ///             yield return "C";
    ///         }
    ///     }
    /// </code>
    /// </example>
    /// <seealso cref="CombinatorialTestAttribute"/>
    /// <seealso cref="FactoryAttribute"/>
    /// <seealso cref="UsingEnumAttribute"/>
    /// <seealso cref="UsingBaseAttribute"/>
    /// <seealso cref="UsingImplementationsAttribute"/>
    /// <seealso cref="UsingLinearAttribute"/>
    /// <seealso cref="UsingLiteralsAttribute"/>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class UsingFactoriesAttribute : UsingBaseAttribute {
        private Type factoryType = null;
        private string memberNames = null;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingFactoriesAttribute"/> class.
        /// </summary>
        public UsingFactoriesAttribute() {
            this.factoryType = null;
            this.memberNames = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingFactoriesAttribute"/> class.
        /// </summary>
        /// <param name="memberNames">A semi-colon delimited list of the factory method to use</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberNames"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberNames"/> is an empty string</exception>
        public UsingFactoriesAttribute(string memberNames) {
            if (memberNames == null)
                throw new ArgumentNullException("memberNames");
            if (memberNames.Length == 0)
                throw new ArgumentException("Length is zero", "memberNames");
            this.factoryType = null;
            this.memberNames = memberNames;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingFactoriesAttribute"/> class.
        /// </summary>
        /// <param name="factoryType">The factory class to be used.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryType"/> is null</exception>
        public UsingFactoriesAttribute(Type factoryType) {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            this.factoryType = factoryType;
            this.memberNames = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingFactoriesAttribute"/> class.
        /// </summary>
        /// <param name="factoryType">The factory class to be used</param>
        /// <param name="memberNames">A semi-colon delimited list of the factory methods to be used with the <paramref name="factoryType"/> class</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="memberNames"/> or <paramref name="factoryType"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="memberNames"/> is an empty string</exception>
        public UsingFactoriesAttribute(Type factoryType, string memberNames) {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (memberNames == null)
                throw new ArgumentNullException("memberNames");
            if (memberNames.Length == 0)
                throw new ArgumentException("Length is zero", "memberNames");
            this.factoryType = factoryType;
            this.memberNames = memberNames;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns a semicolon delimited list of factory methods to be used to provide values to the test
        /// </summary>
        /// <value>A list of factory method names</value>
        public string MemberNames {
            get {
                return this.memberNames;
            }
            set {
                this.memberNames = value;
            }
        }

        /// <summary>
        /// Gets or sets the factory class.
        /// </summary>
        /// <value>The <see cref="Type"/> of the factory.</value>
        public Type FactoryType {
            get {
                return this.factoryType;
            }
            set {
                this.factoryType = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets the set of values (the collection of domains) for the parameter.
        /// </summary>
        /// <param name="domains">The <see cref="IDomainCollection"/> the values generated by the source of data</param>
        /// <param name="parameter"><see cref="ParameterInfo"/> for the parameter that wants the values.</param>
        /// <param name="fixture">The test fixture.</param>
        /// <remarks>See <a href="http://blog.dotnetwiki.org/CombinatorialTestingWithTestFu1.aspx">here</a> for more on
        /// domain generation</remarks>
        public override void GetDomains(IDomainCollection domains, ParameterInfo parameter, object fixture) {
            Type t = null;
            if (this.factoryType != null)
                t = factoryType;
            else
                t = parameter.Member.DeclaringType;

            if (this.MemberNames == null) {
                GetAllDomains(domains, parameter, t);
            } else {
                GetNamedDomains(domains, parameter, t);
            }
        }

        private void GetNamedDomains(IDomainCollection domains, ParameterInfo parameter, Type t) {
            foreach (string memberName in this.MemberNames.Split(';')) {
                MethodInfo domainMethod = t.GetMethod(memberName, Type.EmptyTypes);
                if (domainMethod == null)
                    Assert.Fail("Could not find domain method {0} for parameter {1}",
                        memberName, parameter.Name);

                object result = this.InvokeMethod(t, domainMethod);

                IDomain domain = Domains.ToDomain(result);
                domain.Name = domainMethod.Name;
                domains.Add(domain);
            }
        }

        private void GetAllDomains(IDomainCollection domains, ParameterInfo parameter, Type t) {
            foreach (MethodInfo factoryMethod in TypeHelper.GetAttributedMethods(t, typeof(FactoryAttribute))) {
                if (factoryMethod.GetParameters().Length > 0)
                    continue;
                Type returnType = factoryMethod.ReturnType;

                // check single object return
                if (parameter.ParameterType.IsAssignableFrom(returnType)) {
                    object result = this.InvokeMethod(t, factoryMethod);
                    IDomain domain = Domains.ToDomain(result);
                    domain.Name = factoryMethod.Name;
                    domains.Add(domain);
                    continue;
                }

                // check array
                if (returnType.HasElementType) {
                    Type elementType = returnType.GetElementType();
                    if (parameter.ParameterType == elementType) {
                        object result = this.InvokeMethod(t, factoryMethod);
                        IDomain domain = Domains.ToDomain(result);
                        domain.Name = factoryMethod.Name;
                        domains.Add(domain);
                        continue;
                    }
                }

                // check factory type
                FactoryAttribute factoryAttribute = TypeHelper.TryGetFirstCustomAttribute(factoryMethod, typeof(FactoryAttribute)) as FactoryAttribute;
                if (factoryAttribute != null) {
                    Type factoredType = factoryAttribute.FactoredType;
                    if (parameter.ParameterType == factoredType) {
                        object result = this.InvokeMethod(t, factoryMethod);
                        IDomain domain = Domains.ToDomain(result);
                        domain.Name = factoryMethod.Name;
                        domains.Add(domain);
                        continue;
                    }
                }
            }
        }
    }
}
