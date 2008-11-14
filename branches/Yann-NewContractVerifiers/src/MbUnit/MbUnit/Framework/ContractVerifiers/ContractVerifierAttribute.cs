using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework.Pattern;
using Gallio.Reflection;
using Gallio.Model;

namespace MbUnit.Framework.NewContractVerifiers
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
            var fixtureType = ((ITypeInfo)containingScope.CodeElement).Resolve(true);
            var fixtureInstance = Activator.CreateInstance(fixtureType);
            var fieldInfo = ((IFieldInfo)codeElement).Resolve(true);
            var fieldInstance = (IContractVerifier)fieldInfo.GetValue(fixtureInstance);
            var contractTest = new PatternTest(codeElement.Name, codeElement, containingScope.TestDataContext.CreateChild());
            contractTest.IsTestCase = false;
            contractTest.Metadata.SetValue(MetadataKeys.TestKind, TestKinds.Group);
            var scope = containingScope.AddChildTest(contractTest);

            foreach (var item in fieldInstance.GetContractPatterns())
            {
                item.Build(scope, codeElement.Name);
            }
        }
    }
}
