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

namespace MbUnit.Core.Reports.Serialization
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-counter")]
    [XmlRoot(ElementName="report-counter")]
    [Serializable]
    public sealed class ReportCounter 
    {        
        private int _successcount;
        private int _ignorecount;
        private int _failureCount;
        private int _runCount;
        private int _skipCount;
        private int _assertCount;
        private double _duration;

        public void Clear()
		{
			this._successcount=0;
			this._ignorecount=0;
			this._failureCount=0;
			this._runCount=0;
            this._skipCount = 0;
            this._assertCount = 0;
            this._duration = 0.00001;
        }

		public void AddCounts(ReportCounter right)
		{
			if (right==null)
				throw new ArgumentNullException("right");
			this.RunCount += right.RunCount;
			this.SuccessCount += right.SuccessCount;
			this.FailureCount += right.FailureCount;
			this.IgnoreCount += right.IgnoreCount;
            this.SkipCount += right.SkipCount;
            this.AssertCount += right.AssertCount;
            this.Duration += right.Duration;
        }


        [XmlAttribute("duration")]
        public double Duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="run-count")]
        public int RunCount {
            get {
                return this._runCount;
            }
            set {
                this._runCount = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="success-count")]
        public int SuccessCount 
        {
            get 
            {
                return this._successcount;
            }
            set 
            {
                this._successcount = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="failure-count")]
        public int FailureCount 
        {
            get 
            {
                return this._failureCount;
            }
            set 
            {
                this._failureCount = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="ignore-count")]
        public int IgnoreCount 
        {
            get {
                return this._ignorecount;
            }
            set {
                this._ignorecount = value;
            }
        }

        [XmlAttribute(AttributeName = "skip-count")]
        public int SkipCount
        {
            get
            {
                return this._skipCount;
            }
            set
            {
                this._skipCount = value;
            }
        }

        [XmlAttribute(AttributeName = "assert-count")]
        public int AssertCount
        {
            get
            {
                return this._assertCount;
            }
            set
            {
                this._assertCount = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}r/{1}s/{2}i/{3}f/{4}k/{5}a/{6:0.00}sec",
                this.RunCount,
                this.SuccessCount,
                this.IgnoreCount,
                this.FailureCount,
                this.SkipCount,
                this.AssertCount,
                this.Duration
                );
        }

    }
}
