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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

namespace MbUnit.Core.Reports.Serialization
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    
    
    /// <summary />
    /// <remarks />
    [XmlType(IncludeInSchema=true, TypeName="report-assembly-version")]
    [XmlRoot(ElementName="report-assembly-version")]
    [Serializable]
    public sealed class ReportAssemblyVersion {
        
        /// <summary />
        /// <remarks />
        private int _build;
        
        /// <summary />
        /// <remarks />
        private int _revision;
        
        /// <summary />
        /// <remarks />
        private int _minor;
        
        /// <summary />
        /// <remarks />
        private int _major;
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="major")]
        public int Major {
            get {
                return this._major;
            }
            set {
                this._major = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="minor")]
        public int Minor {
            get {
                return this._minor;
            }
            set {
                this._minor = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="build")]
        public int Build {
            get {
                return this._build;
            }
            set {
                this._build = value;
            }
        }
        
        /// <summary />
        /// <remarks />
        [XmlAttribute(AttributeName="revision")]
        public int Revision {
            get {
                return this._revision;
            }
            set {
                this._revision = value;
            }
        }
    }
}
