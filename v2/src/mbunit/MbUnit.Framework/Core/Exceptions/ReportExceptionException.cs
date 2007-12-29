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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;
using System.IO;
using System.Runtime.Serialization;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Exceptions
{
	[Serializable]
	public class ReportExceptionException : ApplicationException, ISerializable
	{
		private ReportException exception;

		public ReportExceptionException(string message,ReportException exception)
			:base(message)
		{
			if (exception==null)
				throw new ArgumentNullException("exception");
			this.exception=exception;
		}

		protected ReportExceptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
		{
			this.exception = (ReportException)info.GetValue("Exception",typeof(ReportException));
		}

		#region ISerializable Members

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info,context);
			info.AddValue("Exception",this.exception);
		}

        public override string ToString()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine(this.Message);
            sw.WriteLine(this.exception.ToString());
            return sw.ToString();
        }
		#endregion
	}
}
