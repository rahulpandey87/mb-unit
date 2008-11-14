using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Gallio.Framework.Assertions;
using Gallio.Framework.Data;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Gallio.Reflection;
using MbUnit.Framework.NewContractVerifiers.Patterns;
using MbUnit.Framework.NewContractVerifiers.Patterns.Comparison;

namespace MbUnit.Framework.NewContractVerifiers
{
    /// <summary>
    /// Field-based contract verifier for the implementation of
    /// the generic <see cref="IComparable{T}"/> interface. 
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class ComparisonContractVerifier<TTarget> : AbstractContractVerifier
        where TTarget : IComparable<TTarget>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ComparisonContractVerifier()
        {
            this.ImplementsOperatorOverloads = true;
        }

        /// <summary>
        /// <para>
        /// Determines whether the verifier will evaluate the presence and the 
        /// behavior of the equality and the inequality operator overloads.
        /// The default value is 'true'.
        /// </para>
        /// Built-in verifications:
        /// <list type="bullet">
        /// <item>The type has a static equality operator (==) overload which 
        /// behaves correctly against the provided equivalence classes.</item>
        /// <item>The type has a static inequality operator (!=) overload which 
        /// behaves correctly against the provided equivalence classes.</item>
        /// </list>
        /// </summary>
        public bool ImplementsOperatorOverloads
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of equivalence instance classes
        /// to feed the contract verifier.
        /// </summary>
        public EquivalenceClassCollection<TTarget> EquivalenceClasses
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override IEnumerable<ContractVerifierPattern> GetContractPatterns()
        {
            var equivalenceClassSource = GetType().GetProperty("EquivalenceClasses", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance);

            // Is IComparable.CompareTo implementation OK?
            yield return new ComparisonPatternBuilder<TTarget, int>()
                .SetName("ComparableCompareTo")
                .SetSignatureDescription(String.Format("public bool CompareTo({0})", typeof(TTarget).Name))
                .SetComparisonMethodInfo(GetIComparableInterface().GetMethod("CompareTo", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(TTarget) }, null))
                .SetFunctionRefers((i, j) => i.CompareTo(j))
                .SetFunctionFormats(x => (x == 0) ? "0" : ((x > 0) ? "A Positive Value" : "A Negative Value"))
                .SetFunctionPostProcesses(x => Math.Sign(x))
                .SetEquivalenceClassSource(equivalenceClassSource)
                .ToPattern();

            if (ImplementsOperatorOverloads)
            {
                // Is "Greater Than" operator overload implementation OK?
                yield return new ComparisonPatternBuilder<TTarget, bool>()
                    .SetName("OperatorGreaterThan")
                    .SetSignatureDescription(String.Format("static bool operator >({0}, {0})", typeof(TTarget).Name))
                    .SetComparisonMethodInfo(typeof(TTarget).GetMethod("op_GreaterThan", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                    .SetFunctionRefers((i, j) => i > j)
                    .SetEquivalenceClassSource(equivalenceClassSource)
                    .ToPattern();

                // Is "Greater Than Or Equal" operator overload implementation OK?
                yield return new ComparisonPatternBuilder<TTarget, bool>()
                   .SetName("OperatorGreaterThanOrEqual")
                   .SetSignatureDescription(String.Format("static bool operator >=({0}, {0})", typeof(TTarget).Name))
                   .SetComparisonMethodInfo(typeof(TTarget).GetMethod("op_GreaterThanOrEqual", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                   .SetFunctionRefers((i, j) => i >= j)
                   .SetEquivalenceClassSource(equivalenceClassSource)
                   .ToPattern();

                // Is "Less Than" operator overload implementation OK?
                yield return new ComparisonPatternBuilder<TTarget, bool>()
                    .SetName("OperatorLessThan")
                    .SetSignatureDescription(String.Format("static bool operator <({0}, {0})", typeof(TTarget).Name))
                    .SetComparisonMethodInfo(typeof(TTarget).GetMethod("op_LessThan", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                    .SetFunctionRefers((i, j) => i < j)
                    .SetEquivalenceClassSource(equivalenceClassSource)
                    .ToPattern();

                // Is "Less Than Or Equal" operator overload implementation OK?
                yield return new ComparisonPatternBuilder<TTarget, bool>()
                    .SetName("OperatorLessThanOrEqual")
                    .SetSignatureDescription(String.Format("static bool operator <=({0}, {0})", typeof(TTarget).Name))
                    .SetComparisonMethodInfo(typeof(TTarget).GetMethod("op_LessThanOrEqual", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                    .SetFunctionRefers((i, j) => i <= j)
                    .SetEquivalenceClassSource(equivalenceClassSource)
                    .ToPattern();
            }
        }

        private Type GetIComparableInterface()
        {
            return GetInterface(typeof(TTarget), typeof(IComparable<>)
                .MakeGenericType(typeof(TTarget)));
        }
    }
}
