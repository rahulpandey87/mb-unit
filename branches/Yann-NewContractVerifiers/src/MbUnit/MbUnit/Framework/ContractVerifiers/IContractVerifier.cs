using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework.Pattern;
using MbUnit.Framework.ContractVerifiers.Patterns;

namespace MbUnit.Framework.ContractVerifiers
{
    /// <summary>
    /// Field-level contract verifier.
    /// </summary>
    public interface IContractVerifier
    {
        /// <summary>
        /// Provides builders of pattern tests for the contract verifier.
        /// </summary>
        /// <returns>An enumeration of pattern test builders.</returns>
        IEnumerable<ContractVerifierPattern> GetContractPatterns();
    }
}
