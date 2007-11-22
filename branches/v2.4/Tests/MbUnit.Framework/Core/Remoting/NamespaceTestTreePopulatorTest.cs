using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Collections;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Remoting
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="NamespaceTestTreePopulator"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class NamespaceTestTreePopulatorTest
    {
        private NamespaceTestTreePopulator target = null;
        private GuidTestTreeNodeDictionary dic;
        private TestTreeNode root;

        [Test]
        public void Test()
        {
            this.target = new NamespaceTestTreePopulator();
            this.dic = new GuidTestTreeNodeDictionary();

            root = new TestTreeNode("root", TestNodeType.Category);

            AddPipe(typeof(MbUnit.Core.Remoting.Child1.DummyClass));
            AddPipe(typeof(MbUnit.Core.Remoting.Child2.DummyClass));

            Visit(root,"");

            Assert.AreEqual(1, root.Nodes.Count);

            TestTreeNode child = root.Nodes[0];
            Assert.AreEqual("Namespaces", child.Name);
            Assert.AreEqual(1, child.Nodes.Count);

            child = child.Nodes[0];
            Assert.AreEqual(1, child.Nodes.Count);
            Assert.AreEqual("MbUnit", child.Name);

            child = child.Nodes[0];
            Assert.AreEqual(1, child.Nodes.Count);
            Assert.AreEqual("Core", child.Name);

            child = child.Nodes[0];
            Assert.AreEqual("Remoting", child.Name);

            Assert.AreEqual(2, child.Nodes.Count);
            Assert.IsTrue((child.Nodes[0].Name == "Child1" && child.Nodes[1].Name == "Child2")
                || (child.Nodes[0].Name == "Child2" && child.Nodes[1].Name == "Child1")
                );

            Assert.AreEqual(1,child.Nodes[0].Nodes.Count);
            Assert.AreEqual(1,child.Nodes[1].Nodes.Count);
            Assert.AreEqual("DummyClass", child.Nodes[0].Nodes[0].Name);
            Assert.AreEqual("DummyClass", child.Nodes[1].Nodes[0].Name);
        }

        private void Visit(TestTreeNode node, string tab)
        {
            Console.WriteLine("{0}{1}",tab,node.Name);
            string tabchild = tab + "  ";
            foreach (TestTreeNode child in node.Nodes)
                Visit(child, tabchild);
        }

        private void AddPipe(Type t)
        {
            Fixture fixture1 = new Fixture(
                t,
                new MockRun(),
                null,
                null, 
                false
                );
            RunPipeStarterCollection pipes = new RunPipeStarterCollection(fixture1);
            pipes.Add(new RunPipeStarter(new RunPipe(fixture1)));

            this.target.Populate(dic, root, pipes);
        }

        internal class MockRun : Run
        {
            public MockRun()
                :base()
            { }

            public override void Reflect(MbUnit.Core.Invokers.RunInvokerTree tree, MbUnit.Core.Invokers.RunInvokerVertex parent, Type t)
            {
                throw new NotImplementedException();
            }
        }
    }
}

namespace MbUnit.Core.Remoting.Child1
{
    internal class DummyClass
    {
    }
}

namespace MbUnit.Core.Remoting.Child2
{
    internal class DummyClass
    {
    }
}