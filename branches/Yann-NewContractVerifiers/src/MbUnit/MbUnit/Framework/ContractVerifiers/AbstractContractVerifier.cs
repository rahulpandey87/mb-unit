using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework.Pattern;
using MbUnit.Framework.NewContractVerifiers.Patterns;

namespace MbUnit.Framework.NewContractVerifiers
{
    /// <summary>
    /// Abstract base class for field-level contract verifier.
    /// </summary>
    public abstract class AbstractContractVerifier : IContractVerifier
    {
        /// <summary>
        /// Provides builders of pattern tests for the contract verifier.
        /// </summary>
        /// <returns>An enumeration of pattern test builders.</returns>
        public abstract IEnumerable<ContractVerifierPattern> GetContractPatterns();

        /// <summary>
        /// Gets the interface of a particular type if it is implemented by another type,
        /// otherwise returns null.
        /// </summary>
        /// <param name="implementationType">The implementation type</param>
        /// <param name="interfaceType">The interface type</param>
        /// <returns>The interface type or null if it is not implemented by the implementation type</returns>
        protected static Type GetInterface(Type implementationType, Type interfaceType)
        {
            return interfaceType.IsAssignableFrom(implementationType) ? interfaceType : null;
        }
    }
}
