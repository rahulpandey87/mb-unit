using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework.Pattern;
using Gallio.Reflection;
using Gallio.Model;
using System.Reflection;

namespace MbUnit.Framework.ContractVerifiers
{
    /// <summary>
    /// <para>
    /// The <see cref="ContractVerifierAttribute" /> class qualifies the field-based contract verifiers.
    /// It designates a test fixture field as a contract verifier. 
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ContractVerifierAttribute : PatternAttribute
    {
        /// <inheritdoc />
        public override bool IsPrimary
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public override bool IsTest(PatternEvaluator evaluator, ICodeElementInfo codeElement)
        {
            return true;
        }

        /// <inheritdoc />
        public override void Consume(PatternEvaluationScope containingScope, ICodeElementInfo codeElement, bool skipChildren)
        {
            var fixtureType = GetFixtureType(containingScope);
            var fixtureInstance = GetFixtureInstance(fixtureType);
            var fieldInfo = GetFieldInfo(codeElement);
            var fieldInstance = GetFieldInstance(fieldInfo, fixtureInstance);
            var contractTest = new PatternTest(codeElement.Name, codeElement, containingScope.TestDataContext.CreateChild());
            contractTest.IsTestCase = false;
            contractTest.Metadata.SetValue(MetadataKeys.TestKind, TestKinds.Group);
            var scope = containingScope.AddChildTest(contractTest);

            foreach (var item in fieldInstance.GetContractPatterns())
            {
                item.Build(scope, codeElement.Name);
            }
        }

        private Type GetFixtureType(PatternEvaluationScope containingScope)
        {
            return ((ITypeInfo)containingScope.CodeElement).Resolve(true);
        }

        private object GetFixtureInstance(Type fixtureType)
        {
            return Activator.CreateInstance(fixtureType);
        }

        private FieldInfo GetFieldInfo(ICodeElementInfo codeElement)
        {
            return ((IFieldInfo)codeElement).Resolve(true);
        }

        private IContractVerifier GetFieldInstance(FieldInfo fieldInfo, object fixtureInstance)
        {
            return (IContractVerifier)fieldInfo.GetValue(fixtureInstance);
        }
    }
}
