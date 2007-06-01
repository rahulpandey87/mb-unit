using System;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.IO;

namespace XsdTidy
{
	using Refly.Xsd;

	/// <summary>
	/// Summary description for XsdTidyConfig.
	/// </summary>
	[XmlRoot("xsdtidy-config")]
	public class XsdTidyConfig
	{
		private string xsdFile = string.Empty;
		private string outputPath = string.Empty;
		private string outputNamespace = "Schemas";
		private bool createNamespaceFolders = false;
        private string xsdExePath = @"C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin";
		private XsdWrapperConfig tidier = new XsdWrapperConfig();
		private bool autoCamelize = true;
		private bool autoCapitalize=true;

		[Category("Data")]
		[Description("XSD file to convert")]
		[XmlElement("xsd-file")]
		[EditorAttribute(typeof(FileNameEditor),typeof(UITypeEditor))]
		public string XsdFile
		{
			get 
			{
				return this.xsdFile;
			}
			set
			{
				this.xsdFile=value;
			}
		}

		[Category("Data")]
		[Description("Root namespace of generated classes")]
		[XmlElement("output-namespace")]
		public string OutputNamespace
		{
			get 
			{
				return this.outputNamespace;
			}
			set
			{
				this.outputNamespace=value;
			}
		}

		[Category("Data")]
		[Description("Indicates whether folders will be created for each element in namespace")]
		[XmlElement("create-folders")]
		public bool CreateNamespaceFolders 
		{
			get 
			{
				return this.createNamespaceFolders;
			}
			set 
			{
				this.createNamespaceFolders=value;
			}
		}

		[Category("Data")]
		[Description("Path to xsd.exe")]
		[XmlElement("xsdexe-path")]
		[EditorAttribute(typeof(FolderNameEditor),typeof(UITypeEditor))]
		public string XsdExePath 
		{
			get 
			{
				return this.xsdExePath;
			}
			set 
			{
				this.xsdExePath=value;
			}
		}

		[Category("Data")]
		[Description("Output path where generated class files will be saved.")]
		[XmlElement("output-path")]
		[EditorAttribute(typeof(FolderNameEditor),typeof(UITypeEditor))]
		public string OutputPath 
		{
			get 
			{
				return this.outputPath;
			}
			set 
			{
				this.outputPath=value;
			}
		}

		[Category("Data")]
		[Description("Automatically camelize words.")]
		[XmlAttribute("auto-camelize")]
		public bool AutoCamelize
		{
			get
			{
				return this.autoCamelize;
			}
			set
			{
				this.autoCamelize=value;
			}
		}

		[Category("Data")]
		[Description("Automatically capitalize words.")]
		[XmlAttribute("auto-capitalize")]
		public bool AutoCapitalize
		{
			get
			{
				return this.autoCapitalize;
			}
			set
			{
				this.autoCapitalize=value;
			}
		}

		/*
		[Category("Data")]
		[Description("")]
		[EditorAttribute(typeof(XsdWrapperConfig),typeof(UITypeEditor))]
		public XsdWrapperConfig Tidier
		{
			get
			{
				return this.tidier;
			}
		}
*/

		public void CheckData()
		{
			// check xsd file
			if (this.xsdFile==null || this.xsdFile.Length==0)
				throw new ApplicationException("XsdFile not set");

			// check xsd file
			if (!File.Exists(this.xsdFile))
				throw new ApplicationException("XsdFile not found");

			// check xsd.exe file
			if (this.xsdExePath == null || this.xsdExePath.Length ==  0 || !Directory.Exists(this.xsdExePath))
				throw new ApplicationException("xsd.exe file not found");

			// check output path file
			if (this.outputPath == null || this.outputPath.Length ==  0 || !Directory.Exists(this.outputPath))
				throw new ApplicationException("Output path does not exist.");
		}
	}
}
