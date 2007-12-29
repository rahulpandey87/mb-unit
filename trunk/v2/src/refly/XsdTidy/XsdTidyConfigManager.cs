using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;

namespace XsdTidy
{
	/// <summary>
	/// XsdTidyConfigManager manages the serialization and deserialization of 
	/// XsdTidyConfig instances. It supports two types of save, current 
	/// settings and default settings. Both are saved in the executing assemblies'
	/// current folder as XML files named "SavedSettings" and "DefaultSettings" respectively.
	/// </summary>
	internal class XsdTidyConfigManager
	{
		private const string _SAVED ="SavedSettings.xml";
		private const string _DEFAULT ="DefaultSettings.xml";

		/// <summary>
		/// Save the config to disk as normal settings.
		/// </summary>
		/// <param name="config">Configuration settings to be saved.</param>
		internal static void Save(XsdTidyConfig config){
			Save(config, _SAVED);
		}

		/// <summary>
		/// Save the config to disk as default settings.
		/// </summary>
		/// <param name="config">Configuration settings to be saved.</param>
		internal static void SaveAsDefault(XsdTidyConfig config){
			Save(config, _DEFAULT);
		}

		/// <summary>
		/// Load the normal configuration settings from disk.
		/// </summary>
		/// <returns>Configuration setting load from disk.</returns>
		internal static XsdTidyConfig Load(){
			return Load(_SAVED);
		}

		/// <summary>
		/// Load the default configuration settings from disk.
		/// </summary>
		/// <returns>Configuration setting load from disk.</returns>
		internal static XsdTidyConfig LoadDefault(){
			return Load(_DEFAULT);
		}

		private static XsdTidyConfig Load(string fromFile){
			XmlSerializer serializer = new XmlSerializer (typeof (XsdTidyConfig));
			XsdTidyConfig config;
			FileStream stream = null; 
			try{
				stream = new FileStream (fromFile, FileMode.Open);
				config = serializer.Deserialize (stream) as XsdTidyConfig;
			}
			catch(FileNotFoundException ex){
				throw new ApplicationException("Configuration file '" + fromFile + "' was not found.", ex);
			}
			finally{
				if (stream != null )
					stream.Close ();
			}
			return config;
		}

		private static void Save(XsdTidyConfig config, string toFile){
			XmlSerializer serializer = new XmlSerializer (typeof (XsdTidyConfig));
			FileStream stream = null; 
			try{
				stream = new FileStream (toFile, FileMode.Create);
				serializer.Serialize (stream, config);
				stream.Flush ();
			}
			catch(IOException ex){
				throw new ApplicationException("There was an error while attempting to save the configuration file '" + toFile + "'.", ex);
			}
			finally{
				if (stream != null )
					stream.Close ();
			}
		}
	}
}
