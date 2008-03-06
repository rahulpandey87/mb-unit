using System;
using System.ComponentModel;
using System.IO;

using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Attributes.Decorators
{
	[TestFixture]
	public class ExtractResourceTests
	{
        [Test, Category("Attributes.Decorators"), ExtractResource("MbUnit.Framework.Tests.Core.EmbeddedResourceSample.txt", "EmbeddedResourceSample.txt")]
        public void CanExtractResource()
        {
            Assert.IsTrue(File.Exists("EmbeddedResourceSample.txt"), "Could not find the extracted resource file");
        }

        [Test, Category("Attributes.Decorators"), ExtractResource("MbUnit.Framework.Tests.Core.EmbeddedResourceSample.txt")]
        public void CanExtractResourceToStream()
        {
            Stream stream = ExtractResourceAttribute.Stream;
            Assert.IsNotNull(stream, "The Stream is null");
            using (StreamReader reader = new StreamReader(stream))
            {
                Assert.AreEqual("Hello World! The ExtractResourceAttribute works!", reader.ReadToEnd(), "The contents of the resource is not what we expected.");
            }
        }
	}
}
