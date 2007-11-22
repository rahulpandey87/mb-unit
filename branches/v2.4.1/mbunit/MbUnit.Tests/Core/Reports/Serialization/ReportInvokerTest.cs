#region Includes
using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
#endregion

using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Reports.Serialization
{
	/// <summary>
	/// <see cref="TestFixture"/> for the <see cref="ReportInvoker"/> class.
	/// </summary>
	[TestFixture]
	public class ReportInvokerTest
	{
		#region Tests
        [Test]
        public void Serialize()
        {
			SerialAssert.IsXmlSerializable(typeof(ReportInvoker));
		}
		#endregion
	}
}

