using System;
using System.IO;

using MbUnit.Core.Config;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Core.Config
{
    [TestFixture]
    public class MbUnitProjectTest
    {
        private string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MbUnit\Test.mbunit");
        private MbUnitProject proj;

        [SetUp]
        public void SetUp()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MbUnit");
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }

        [Test]
        public void SaveAndLoadTest()
        {
            // Save test project
            proj = new MbUnitProject();
            proj.Assemblies.Add("string1");
            proj.Assemblies.Add("string2");
            proj.Assemblies.Add("string3");
            proj.Save(filename);

            // Re-load project
            proj = MbUnitProject.Load(filename);
            Assert.AreEqual(3, proj.Assemblies.Count);
            CollectionAssert.Contains(proj.Assemblies, "string1");
            CollectionAssert.Contains(proj.Assemblies, "string2");
            CollectionAssert.Contains(proj.Assemblies, "string3");
        }

        [Test, ExpectedException(typeof(ArgumentException), "fileName is null.")]
        public void LoadNullTest()
        {
            proj = MbUnitProject.Load(null);
        }

        [Test, ExpectedException(typeof(ArgumentException), "fileName is null.")]
        public void SaveNullTest()
        {
            proj = new MbUnitProject();
            proj.Save(null);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }
    }
}
