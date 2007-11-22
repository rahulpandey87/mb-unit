using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace QuickWix
{
    public class WixSettings
    {
        private string fileName;
        private bool quitOnError = false;
        private bool skipUI = true;
        private bool skipVSI = true;
        private bool processUIOnly = false;
        private bool skipSequenceTables = false;
        private bool skipSummaryInfo = true;
        private bool unicode = false;
        private bool exportBinaries = false;
        private bool isMergeModule = false;
        private bool keepEmptyTables = false;

        [Category("Wix")]
        [Description("Chooses a .msi or .wix file")]
        [Editor(typeof(FileNameEditor),typeof(UITypeEditor))]
        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        [Category("Msi")]
        public bool SkipUI
        {
            get { return this.skipUI; }
            set { this.skipUI = value; }
        }
        [Category("Msi")]
        public bool QuitOnError
        {
            get { return this.quitOnError; }
            set { this.quitOnError = value; }
        }
        [Category("Msi")]
        public bool SkipVSI
        {
            get { return this.skipVSI; }
            set { this.skipVSI = value; }
        }
        [Category("Msi")]
        public bool ProcessUIOnly
        {
            get { return this.processUIOnly; }
            set { this.processUIOnly = value; }
        }
        [Category("Msi")]
        public bool SkipSequenceTables
        {
            get { return this.skipSequenceTables; }
            set { this.skipSequenceTables = value; }
        }
        [Category("Msi")]
        public bool SkipSummaryInfo
        {
            get { return this.skipSummaryInfo; }
            set { this.skipSummaryInfo = value; }
        }
        [Category("Msi")]
        public bool Unicode
        {
            get { return this.unicode; }
            set { this.unicode = value; }
        }
        [Category("Msi")]
        public bool ExportBinaries
        {
            get { return this.exportBinaries; }
            set { this.exportBinaries = value; }
        }
        [Category("Msi")]
        public bool IsMergeModule
        {
            get { return this.isMergeModule; }
            set { this.isMergeModule = value; }
        }
        [Category("Msi")]
        public bool KeepEmptyTables
        {
            get { return this.keepEmptyTables; }
            set { this.keepEmptyTables = value; }
        }
    }
}
