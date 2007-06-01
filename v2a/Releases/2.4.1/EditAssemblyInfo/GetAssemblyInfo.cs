using System.Reflection;

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace MbUnit.EditAssemblyInfo
{
    public class GetAssemblyInfoTask : Task
    {
        public string _FileName;
        private string _assemblyVersion;

        public override bool Execute()
        {
            try
            {
                Assembly a = Assembly.LoadFrom(_FileName);
                AssemblyName b = a.GetName();
                _assemblyVersion = b.Version.ToString();

                return true;
            }
            catch
            {
                return false;
            }

        }

        [Required]
        public string FileName
        {           
            set { _FileName = value; }
        }
                
        [Output]
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }           
        }
	

    }
}
