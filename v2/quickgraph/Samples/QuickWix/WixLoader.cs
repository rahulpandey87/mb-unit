using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Tools.WindowsInstallerXml;
using Microsoft.Tools.WindowsInstallerXml.Serialize;

namespace QuickWix
{
    public class WixLoader
    {
        private Wix wix = null;
        private Decompiler decompiler = new Decompiler();
        private XmlSerializer serializer = new XmlSerializer(typeof(Wix));

        public WixLoader()
        {
            this.decompiler.Message+=new MessageEventHandler(decompiler_Message);
        }

        public Wix Wix
        {
            get { return this.wix; }
        }

        public void Load(WixSettings settings)
        {
            if (settings== null)
                throw new ArgumentNullException("fileName");
            string ext = Path.GetExtension(settings.FileName);
            switch (ext)
            {
                case ".msi":
                    LoadMsi(settings);
                    return;
                case ".wix":
                    LoadWix(settings.FileName);
                    return;
                default:
                    throw new Exception("Unknown file extension");
            }
        }

        public void LoadMsi(WixSettings settings)
        {
            decompiler.ContinueOnError = !settings.QuitOnError;
            decompiler.SkipUI = settings.SkipUI;
            decompiler.SkipVSI = settings.SkipVSI;
            decompiler.ProcessUIOnly = settings.ProcessUIOnly;
            decompiler.SkipSequenceTables = settings.SkipSequenceTables;
            decompiler.SkipSummaryInfo = settings.SkipSummaryInfo;
            decompiler.Unicode = settings.Unicode;
            decompiler.ExportBinaries = settings.ExportBinaries;
            decompiler.IsMergeModule = settings.IsMergeModule;
            decompiler.KeepEmptyTables = settings.KeepEmptyTables;

            string temporary = Path.GetFileNameWithoutExtension(settings.FileName) + ".wix";
            decompiler.Decompile(settings.FileName, temporary);
            LoadWix(temporary);
        }

        public void LoadWix(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                this.wix = (Wix)this.serializer.Deserialize(reader);
            }
        }

        void decompiler_Message(object sender, MessageEventArgs mea)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Id: {0} Level:{1}", mea.Id, mea.Level));
            System.Diagnostics.Debug.WriteLine(mea);
        }
    }
}
