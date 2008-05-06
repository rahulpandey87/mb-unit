// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using MbUnit.Core.Framework;
using TestFu.Operations;

namespace MbUnit.Framework
{
    [Serializable]
    public enum CombinationType
    {
        AllPairs,
        Cartesian
    }

    /// <summary>
    /// Tag use to mark a mark a combinatorial unit test method.
    /// </summary>
    /// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='TestAttribute']"/>		
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CombinatorialTestAttribute : TestPatternAttribute
    {
        private CombinationType combinationType = CombinationType.AllPairs;

        private string tupleValidatorMethod = null;

	/// <summary>
	/// Specifies a combinatorial test of type <see cref="CombinationType.AllPairs" />.
	/// </summary>
        public CombinatorialTestAttribute()
        { }

	/// <summary>
	/// Specifies a combinatorial test of the specified type.
	/// </summary>
        public CombinatorialTestAttribute(CombinationType combinationType)
        {
            this.combinationType = combinationType;
        }

        public CombinationType CombinationType
        {
            get { return this.combinationType; }
        }

        public string TupleValidatorMethod
        {
            get
            {
                return tupleValidatorMethod;
            }

            set
            {
                tupleValidatorMethod = value;
            }
        }

        public ITupleEnumerable GetProduct(IDomainCollection domains)
        {
            switch (this.CombinationType)
            {
                case CombinationType.AllPairs:
                    return Products.PairWize(domains);
                case CombinationType.Cartesian:
                    return Products.Cartesian(domains);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
