using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Collections;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Remoting
{
    /// <summary>
    /// A <see cref="TestFixture" /> for the <see cref="FixtureCategoryTestTreePopulator" />
    /// class
    /// </summary>
    [TestFixture]
    public class FixtureCategoryTestTreePopulator_Test
    {
        private FixtureCategoryTestTreePopulator target = null;
        private GuidTestTreeNodeDictionary dic;
        private TestTreeNode root;

        [Test]
        public void Test()
        {
            this.target = new FixtureCategoryTestTreePopulator();
            this.dic = new GuidTestTreeNodeDictionary();

            root = new TestTreeNode("root", TestNodeType.Category);

            AddPipe(typeof(MbUnit.Core.Remoting.DummyClass1));
            AddPipe(typeof(MbUnit.Core.Remoting.DummyClass2));

            Visit(root,"");

            Assert.AreEqual(1, root.Nodes.Count);

            TestTreeNode categories = root.Nodes[0];
            Assert.AreEqual("Categories", categories.Name);
            Assert.AreEqual(3, categories.Nodes.Count);

            foreach (TestTreeNode node in categories.Nodes)
            {
                Assert.AreEqual(1, node.Nodes.Count);
                switch (node.Name)
                {
                    case "Misc":
                        {
                            TestTreeNode dummyNode2 = node.Nodes[0];
                            Assert.AreEqual(1, dummyNode2.Nodes.Count);
                            Assert.AreEqual("DummyClass2", dummyNode2.Name);

                            break;
                        }

                    case "MyApp":
                        {
                            TestTreeNode businessLogic = node.Nodes[0];
                            Assert.AreEqual(1, businessLogic.Nodes.Count);
                            Assert.AreEqual("BusinessLogic", businessLogic.Name);

                            TestTreeNode dummyNode1 = businessLogic.Nodes[0];
                            Assert.AreEqual(1, dummyNode1.Nodes.Count);
                            Assert.AreEqual("DummyClass1", dummyNode1.Name);

                            break;
                        }

                    case "BusinessLogic":
                        {
                            TestTreeNode dummyNode1 = node.Nodes[0];
                            Assert.AreEqual(1, dummyNode1.Nodes.Count);
                            Assert.AreEqual("DummyClass1", dummyNode1.Name);

                            break;
                        }
                }
            }
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

namespace MbUnit.Core.Remoting
{
    [FixtureCategory("MyApp.BusinessLogic"), FixtureCategory("BusinessLogic")]
    internal class DummyClass1
    {
    }
}

namespace MbUnit.Core.Remoting
{
    internal class DummyClass2
    {
    }
}