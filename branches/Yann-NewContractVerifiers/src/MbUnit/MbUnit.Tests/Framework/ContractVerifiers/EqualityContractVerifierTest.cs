using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using MbUnit.Framework.NewContractVerifiers;
using Gallio.Tests;
using Gallio.Model;

namespace MbUnit.Tests.Framework.NewContractVerifiers
{
    [TestFixture]
    [RunSample(typeof(FullContractOnEquatableSample))]
    [RunSample(typeof(PartialContractOnEquatableSample))]
    public class EqualityContractVerifierTest : AbstractContractVerifierTest
    {
        [Test]
        [Row(typeof(FullContractOnEquatableSample), "ObjectEquals", TestStatus.Passed)]
        [Row(typeof(FullContractOnEquatableSample), "ObjectGetHashCode", TestStatus.Passed)]
        [Row(typeof(FullContractOnEquatableSample), "EquatableEquals", TestStatus.Passed)]
        [Row(typeof(FullContractOnEquatableSample), "OperatorEquals", TestStatus.Passed)]
        [Row(typeof(FullContractOnEquatableSample), "OperatorNotEquals", TestStatus.Passed)]
        [Row(typeof(PartialContractOnEquatableSample), "ObjectEquals", TestStatus.Passed)]
        [Row(typeof(PartialContractOnEquatableSample), "ObjectGetHashCode", TestStatus.Passed)]
        [Row(typeof(PartialContractOnEquatableSample), "EquatableEquals", TestStatus.Passed)]
        [Row(typeof(PartialContractOnEquatableSample), "OperatorEquals", TestStatus.Inconclusive)]
        [Row(typeof(PartialContractOnEquatableSample), "OperatorNotEquals", TestStatus.Inconclusive)]
        public void VerifySampleEqualityContract(Type fixtureType, string testMethodName, TestStatus expectedTestStatus)
        {
            VerifySampleContract("EqualityTests", fixtureType, testMethodName, expectedTestStatus);
        }

        [Explicit]
        private class FullContractOnEquatableSample
        {
            [ContractVerifier]
            public readonly IContractVerifier EqualityTests = new EqualityContractVerifier<SampleEquatable>()
            {
                EquivalenceClasses = 
                    EquivalenceClassCollection<SampleEquatable>.FromDistinctInstances(
                        new SampleEquatable(123),
                        new SampleEquatable(456),
                        new SampleEquatable(789)),
                
                ImplementsOperatorOverloads = true
            };
        }

        [Explicit]
        private class PartialContractOnEquatableSample
        {
            [ContractVerifier]
            public readonly IContractVerifier EqualityTests = new EqualityContractVerifier<SampleEquatable>()
            {
                EquivalenceClasses =
                    EquivalenceClassCollection<SampleEquatable>.FromDistinctInstances(
                        new SampleEquatable(123),
                        new SampleEquatable(456),
                        new SampleEquatable(789)),

                ImplementsOperatorOverloads = false
            };
        }

        /// <summary>
        /// Sample equatable type.
        /// </summary>
        private class SampleEquatable : IEquatable<SampleEquatable>
        {
            private int value;

            public SampleEquatable(int value)
            {
                this.value = value;
            }

            public override int GetHashCode()
            {
                return value.GetHashCode();
            }

            public override bool Equals(object other)
            {
                return Equals(other as SampleEquatable);
            }

            public bool Equals(SampleEquatable other)
            {
                return (other != null) && (value == other.value);
            }

            public static bool operator ==(SampleEquatable left, SampleEquatable right)
            {
                return
                    (((object)left == null) && ((object)right == null)) ||
                    (((object)left != null) && left.Equals(right));
            }

            public static bool operator !=(SampleEquatable left, SampleEquatable right)
            {
                return !(left == right);
            }
        }

    }
}
