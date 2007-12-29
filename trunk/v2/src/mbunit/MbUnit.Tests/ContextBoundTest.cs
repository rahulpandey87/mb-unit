using System;

namespace MbUnit.Tests
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	[TestFixture, AppDomain("FriendlyTest", PrivateBinPath="bin", ShadowCopyFiles=true)]
	public class TextContext : ContextBoundObject
	{
		[Test]
		public void TestMyFriendlyName()
		{
			Assert.AreEqual("FriendlyTest", AppDomain.CurrentDomain.FriendlyName);
		}
	}
}
