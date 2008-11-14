using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Gallio.Framework.Data;
using Gallio.Framework.Assertions;
using Gallio.Reflection;
using MbUnit.Framework.NewContractVerifiers.Patterns;
using MbUnit.Framework.NewContractVerifiers.Patterns.HasAttribute;
using MbUnit.Framework.NewContractVerifiers.Patterns.ObjectHashCode;
using System.Reflection;
using MbUnit.Framework.NewContractVerifiers.Patterns.Equality;

namespace MbUnit.Framework.NewContractVerifiers
{
    /// <summary>
    /// Field-based contract verifier for the implementation of
    /// the generic <see cref="IEquatable{T}"/> interface. 
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public class EqualityContractVerifier<TTarget> : AbstractContractVerifier
        where TTarget : IEquatable<TTarget>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EqualityContractVerifier()
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

            // Is Object equality method OK?
            yield return new EqualityPatternBuilder<TTarget>()
                .SetName("ObjectEquals")
                .SetSignatureDescription("bool Equals(Object)")
                .SetEqualityMethodInfo(typeof(TTarget).GetMethod("Equals", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(object) }, null))
                .SetEquivalenceClassSource(equivalenceClassSource)
                .ToPattern();

            // Is Object hash code calculcation well implemented?
            yield return new ObjectHashCodePatternBuilder<TTarget>()
                .SetEquivalenceClassSource(equivalenceClassSource)
                .ToPattern();

            // Is IEquatable equality method OK?
            yield return new EqualityPatternBuilder<TTarget>()
                .SetName("EquatableEqual")
                .SetSignatureDescription(String.Format("bool Equals({0})", typeof(TTarget).Name))
                .SetEqualityMethodInfo(GetIEquatableInterface().GetMethod("Equals", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(TTarget) }, null))
                .SetEquivalenceClassSource(equivalenceClassSource)
                .ToPattern();

            if (ImplementsOperatorOverloads)
            {
                // Is equality operator overload OK?
                yield return new EqualityPatternBuilder<TTarget>()
                    .SetName("OperatorEquals")
                    .SetSignatureDescription(String.Format("static bool operator ==({0}, {0})", typeof(TTarget).Name))
                    .SetEqualityMethodInfo(typeof(TTarget).GetMethod("op_Equality", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                    .SetEquivalenceClassSource(equivalenceClassSource)
                    .ToPattern();

                // Is inequality operator overload OK?
                yield return new EqualityPatternBuilder<TTarget>()
                    .SetName("OperatorNotEquals")
                    .SetSignatureDescription(String.Format("static bool operator !=({0}, {0})", typeof(TTarget).Name))
                    .SetEqualityMethodInfo(typeof(TTarget).GetMethod("op_Inequality", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(TTarget), typeof(TTarget) }, null))
                    .SetInequality(true)
                    .SetEquivalenceClassSource(equivalenceClassSource)
                    .ToPattern();
            }
        }

        private Type GetIEquatableInterface()
        {
            return GetInterface(typeof(TTarget), typeof(IEquatable<>)
                .MakeGenericType(typeof(TTarget)));
        }
    }
}
