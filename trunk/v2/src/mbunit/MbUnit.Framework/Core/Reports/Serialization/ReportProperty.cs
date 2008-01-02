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

using System;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;

namespace MbUnit.Core.Reports.Serialization
{
    [XmlRoot("property")]
    [Serializable]
    public sealed class ReportProperty
    {
        private string name = null;
        private string value = null;

        public ReportProperty()
        {}

        public ReportProperty(DictionaryEntry de)
        {
            this.name = String.Format("{0}", de.Key);
            try
            {
                if (de.Value == null)
                    this.value = "null";
                else
                    this.value = String.Format("{0}", de.Value);
            }
            catch (Exception ex)
            {
                this.value = String.Format("Error while getting value ({0})",ex.Message);
            }
        }

        public ReportProperty(Object instance, PropertyInfo property)
        {
            this.name = property.Name;
            try
            {
                object val = property.GetValue(instance, null);
                if (val == null)
                    this.value = "null";
                else
                    this.value = String.Format("{0}", val);
            }
            catch (Exception ex)
            {
                this.value = String.Format("Error while getting property value ({0})",ex.Message);
            }
        }

        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [XmlAttribute("value")]
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.name, this.value);
        }
    }
}
