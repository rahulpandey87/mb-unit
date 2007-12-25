// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

extern alias MbUnit2;
using Gallio.Reflection;
using Gallio.Tests;
using MbUnit2::MbUnit.Framework;

using System;
using Gallio.Model.Filters;
using Gallio.Model;
using Rhino.Mocks;
using ITestComponent=Gallio.Model.ITestComponent;

namespace Gallio.Tests.Model.Filters
{
    [TestFixture]
    [TestsOn(typeof(NamespaceFilter<ITestComponent>))]
    public class NamespaceFilterTest : BaseUnitTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgument()
        {
            new NamespaceFilter<ITestComponent>(null);
        }

        [RowTest]
        [Row(true, typeof(NamespaceFilterTest))]
        [Row(false, typeof(NamespaceFilter<ITestComponent>))]
        [Row(false, null)]
        public void IsMatchCombinations(bool expectedMatch, Type type)
        {
            ICodeElementInfo codeElement = type != null ? Reflector.Wrap(type) : null;

            ITestComponent component = Mocks.CreateMock<ITestComponent>();
            SetupResult.For(component.CodeElement).Return(codeElement);
            Mocks.ReplayAll();

            Assert.AreEqual(expectedMatch, new NamespaceFilter<ITestComponent>(
                new EqualityFilter<string>(typeof(NamespaceFilterTest).Namespace)).IsMatch(component));
        }

        [RowTest]
        [Row(typeof(NamespaceFilterTest))]
        [Row(typeof(NamespaceFilter<ITestComponent>))]
        //[Row(null)]
        public void ToStringTest(Type type)
        {
            string namespaceName = type.Namespace;
            NamespaceFilter<ITestComponent> filter = new NamespaceFilter<ITestComponent>(
                new EqualityFilter<string>(namespaceName));
            Assert.AreEqual("Namespace(Equality('" + namespaceName + "'))", filter.ToString());
        }
    }
}