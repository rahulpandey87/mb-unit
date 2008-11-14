using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Gallio.Framework.Assertions;
using Gallio.Framework.Data;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Gallio.Reflection;
using MbUnit.Framework.NewContractVerifiers.Patterns;
using MbUnit.Framework.NewContractVerifiers.Patterns.HasAttribute;
using MbUnit.Framework.NewContractVerifiers.Patterns.HasConstructor;

namespace MbUnit.Framework.NewContractVerifiers
{
    /// <summary>
    /// Field-based contract verifier for the implementation of custom exception.
    /// </summary>
    /// <typeparam name="TException">The target custom exception type.</typeparam>
    public class ExceptionContractVerifier<TException> : AbstractContractVerifier
        where TException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExceptionContractVerifier()
        {
            this.ImplementsSerialization = true;
            this.ImplementsStandardConstructors = true;
        }

        /// <summary>
        /// <para>
        /// Determines whether the verifier will check for the serialization support. 
        /// The default value is 'true'.
        /// </para>
        /// <para>
        /// Built-in verifications:
        /// <list type="bullet">
        /// <item>The exception implements the <see cref="ISerializable" /> interface.</item>
        /// <item>The exception has the <see cref="SerializableAttribute" /> attribute.</item>
        /// <item>The exception type has a protected serialization constructor similar to
        /// <see cref="Exception(SerializationInfo, StreamingContext)" />.</item>
        /// </list>
        /// </para>
        /// </summary>
        public bool ImplementsSerialization
        {
            get;
            set;
        }

        /// <summary>
        /// <para>
        /// Determines whether the verifier will check for the presence of
        /// the recommended standard constructors. The default value is 'true'.
        /// </para>
        /// <para>
        /// Built-in verifications:
        /// <list type="bullet">
        /// <item>The exception has a default argument-less constructor.</item>
        /// <item>The exception has a single argument constructor for the message.</item>
        /// <item>The exception two arguments constructor for the message and an inner exception.</item>
        /// </list>
        /// </para>
        /// </summary>
        public bool ImplementsStandardConstructors
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override IEnumerable<ContractVerifierPattern> GetContractPatterns()
        {
            if (ImplementsSerialization)
            {
                // Has Serializable attribute?
                yield return new HasAttributePatternBuilder<TException, SerializableAttribute>()
                    .ToPattern();

                // Has non-public serialization constructor?
                yield return new HasConstructorPatternBuilder<TException>()
                    .SetName("Serialization")
                    .SetParameterTypes(typeof(SerializationInfo), typeof(StreamingContext))
                    .SetAccessibility(HasConstructorAccessibility.NonPublic)
                    .ToPattern();
            }

            if (ImplementsStandardConstructors)
            {
                // Is public default constructor well defined?
                //yield return new StandardExceptionConstructorPatternBuilder()
                //    .SetTargetExceptionType(TargetType)
                //    .SetFriendlyName("Default")
                //    .SetCheckForSerializationSupport(ImplementsSerialization)
                //    .SetConstructorSpecifications(new ExceptionConstructorSpec())
                //    .ToPattern();

                // Is public single parameter constructor (message) well defined?
                //yield return new StandardExceptionConstructorPatternBuilder()
                //    .SetTargetExceptionType(TargetType)
                //    .SetFriendlyName("Message")
                //    .SetCheckForSerializationSupport(ImplementsSerialization)
                //    .SetParameterTypes(typeof(string))
                //    .SetConstructorSpecifications(
                //        new ExceptionConstructorSpec(null),
                //        new ExceptionConstructorSpec(String.Empty),
                //        new ExceptionConstructorSpec("A message"))
                //    .ToPattern();

                // Is public two parameters constructor (message and inner exception) well defined?
                //yield return new StandardExceptionConstructorPatternBuilder()
                //    .SetTargetExceptionType(TargetType)
                //    .SetFriendlyName("MessageAndInnerException")
                //    .SetCheckForSerializationSupport(ImplementsSerialization)
                //    .SetParameterTypes(typeof(string), typeof(Exception))
                //    .SetConstructorSpecifications(
                //        new ExceptionConstructorSpec(null, null),
                //        new ExceptionConstructorSpec(String.Empty, null),
                //        new ExceptionConstructorSpec("A message", null),
                //        new ExceptionConstructorSpec(null, new Exception()),
                //        new ExceptionConstructorSpec(String.Empty, new Exception()),
                //        new ExceptionConstructorSpec("A message", new Exception()))
                //    .ToPattern();
            }
        }
    }
}
