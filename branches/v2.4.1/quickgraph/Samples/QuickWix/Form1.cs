
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace QuickWix
{
    partial class Form1 : Form
    {
        private WixSettings settings = new WixSettings();
        private WixLoader loader = new WixLoader();
        private WixRenderer renderer = new WixRenderer();

        public Form1()
        {
            InitializeComponent();
            this.propertyGrid1.SelectedObject = settings;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            this.loader.Load(this.settings);
            this.renderer.Wix = this.loader.Wix;

            string fileName = this.renderer.Render(this.settings.FileName);
            if (fileName == null)
                return;
            System.Diagnostics.Process.Start(fileName);
        }
    }
}