using System;

using MbUnit.Framework;

namespace MbUnit.Demo.FixtureDependency
{
        [TestFixture]
        public class Parent
        {
            [Test]
            public void Success()
            {
            }
        }

        [TestFixture]
        public class SickParent
        {
            [Test]
            public void Failure()
            {
                throw new Exception("boom");
            }
        }

        [TestFixture]
        [DependsOn(typeof(Parent))]
        [DependsOn(typeof(SickParent))]
        public class Child
        {
            [Test]
            public void Success()
            {
            }
        }

        [TestFixture]
        [CurrentFixture]
        [DependsOn(typeof(Child))]
        public class GrandChild
        {
            [Test]
            public void Success()
            {
            }
        }
}
