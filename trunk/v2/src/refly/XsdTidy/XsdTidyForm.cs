using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Threading;

namespace XsdTidy
{
	using Refly.Xsd;

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class XsdTidyForm : System.Windows.Forms.Form 
	{
		private XsdTidyConfig config = null;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem setAsDefaultmenuItem;
		private System.Windows.Forms.MenuItem restoreDefaultMenuItem;
		private System.Windows.Forms.MenuItem generateMenuItem;
		private PropertyGrid grid;

		public XsdTidyForm() 
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.config = new XsdTidyConfig();
			grid = new PropertyGrid();
			grid.CommandsVisibleIfAvailable = true;
			grid.Dock = DockStyle.Fill;
			grid.SelectedObject = config;
			
			this.panel1.Controls.Add(grid);

			this.statusBar1.Text = string.Empty;

			InitSettings();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) 
		{
			if( disposing ) 
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.generateMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.setAsDefaultmenuItem = new System.Windows.Forms.MenuItem();
			this.restoreDefaultMenuItem = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Generate...";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 336);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(424, 22);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "statusBar1";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(0, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(424, 296);
			this.panel1.TabIndex = 0;
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(8, 8);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(96, 23);
			this.btnGenerate.TabIndex = 1;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem7,
																					  this.menuItem5});
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 0;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.generateMenuItem});
			this.menuItem7.Text = "Tools";
			// 
			// generateMenuItem
			// 
			this.generateMenuItem.Index = 0;
			this.generateMenuItem.Text = "Generate";
			this.generateMenuItem.Click += new System.EventHandler(this.generateMenuItem_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem6,
																					  this.setAsDefaultmenuItem,
																					  this.restoreDefaultMenuItem});
			this.menuItem5.Text = "Settings";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Save As...";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.Text = "-";
			// 
			// setAsDefaultmenuItem
			// 
			this.setAsDefaultmenuItem.Index = 2;
			this.setAsDefaultmenuItem.Text = "Save As Default";
			this.setAsDefaultmenuItem.Click += new System.EventHandler(this.setAsDefaultmenuItem_Click);
			// 
			// restoreDefaultMenuItem
			// 
			this.restoreDefaultMenuItem.Index = 3;
			this.restoreDefaultMenuItem.Text = "Default";
			this.restoreDefaultMenuItem.Click += new System.EventHandler(this.restoreDefaultMenuItem_Click);
			// 
			// XsdTidyForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(424, 358);
			this.ContextMenu = this.contextMenu1;
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.mainMenu1;
			this.Name = "XsdTidyForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "XsdTidy, beautifies XSD mapping";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new XsdTidyForm());
		}

		private void menuItem1_Click(object sender, System.EventArgs e) 
		{
//			Thread thread = new Thread(new ThreadStart(this.Generate));
            try
            {
                this.Generate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetStatusBar(string format, params object[] args)
        {
            string message = String.Format(format, args);
			this.Invoke(
				new SetControlTextDelegate(SetControlText),
				new object[] { this.statusBar1, message });
        }
        public delegate void SetControlTextDelegate(Control control, string message);
        private static void SetControlText(Control control, string message)
        {
            control.Text = message;
        }

		private	void Generate() 
		{
			// check data...
			SetStatusBar("Checking data");
            Application.DoEvents();
            this.config.CheckData();

			// Build Xsd path & file name and take care of instances where
			// '\' is missing from path and where path is empty.
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append (this.config.XsdExePath.TrimEnd('\\'));
			if (sb.Length > 0) sb.Append(@"\");
			sb.Append("xsd.exe");

			// call xsd.exe
			ProcessStartInfo info = new ProcessStartInfo(sb.ToString());
            Application.DoEvents();
            info.UseShellExecute = false;
            info.Arguments = String.Format(
				"\"{0}\" /c /n:{1}",
				this.config.XsdFile,
				this.config.OutputNamespace
				);

			SetStatusBar("Launching Xsd.exe");
            Application.DoEvents();
            Process process = Process.Start(info);
            process.WaitForExit();

			if (process.ExitCode != 0)
				throw new Exception("Xsd.exe failed");

			// load and compile generated file
			string[] namePieces = this.config.XsdFile.Split('.','/','\\');
			string fileName = (namePieces[namePieces.Length-2] + ".cs").ToLower();

			if (!File.Exists(fileName))
				throw new Exception("File " +fileName + " does not exist");

			// compile
			CSharpCodeProvider provider = new CSharpCodeProvider();

			// Obtain an ICodeGenerator from a CodeDomProvider class.
			ICodeCompiler comp = provider.CreateCompiler();
			CompilerParameters options = new CompilerParameters();
			options.GenerateInMemory = true;
			options.IncludeDebugInformation = true;
			options.ReferencedAssemblies.Add("System.dll");
			options.ReferencedAssemblies.Add("System.Xml.dll");

			SetStatusBar("Compiling result");
			CompilerResults result = comp.CompileAssemblyFromFile(options,fileName);
			if (result.Errors.HasErrors) 
			{
				StringWriter sw =new StringWriter();
				foreach(CompilerError error in result.Errors) 
				{
					sw.WriteLine(error.ToString());
				}
				throw new Exception(sw.ToString());
			}

			SetStatusBar("Recfactoring output");
			XsdWrapperGenerator xsg = new XsdWrapperGenerator(this.config.OutputNamespace);
			xsg.Conformer.Camelize = this.config.AutoCamelize;
			xsg.Conformer.Capitalize = this.config.AutoCapitalize;
			foreach(Type t in result.CompiledAssembly.GetExportedTypes()) 
			{
				xsg.Add(t);
			}
			xsg.Generate();

			// getting generator
			Refly.CodeDom.CodeGenerator gen = new Refly.CodeDom.CodeGenerator();

			SetStatusBar("Generating refactored classes");
			gen.CreateFolders=this.config.CreateNamespaceFolders;
			gen.GenerateCode(this.config.OutputPath,xsg.Ns);	
			SetStatusBar("Generation finished successfully");
		}

		private void btnGenerate_Click(object sender, System.EventArgs e) 
		{
			Thread thread = new Thread(new ThreadStart(this.Generate));
			thread.Start();
		}

		private void InitSettings()
		{
			// Try to load saved settings
			if (!LoadSettings(true)) 
			{
				// If fail, then try to load Default Settings
				LoadDefaultSettings(true);
			}
		}

		private void SaveSettings()
		{
			SaveSettings(false);
		}

		private void SaveSettings(bool silent)
		{
			try
			{
				XsdTidyConfigManager.Save(this.config);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SaveDefaultSettings()
		{
			SaveDefaultSettings(false);
		}

		private void SaveDefaultSettings(bool silent)
		{
			try
			{
				XsdTidyConfigManager.SaveAsDefault(this.config);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadSettings()
		{
			LoadSettings(false);
		}

		private bool LoadSettings(bool silent)
		{
			bool success = false;
			try
			{
				grid.SelectedObject = this.config = XsdTidyConfigManager.Load();
				success = true;
			}
			catch(Exception ex)
			{
				if (!silent) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return success;
		}

		private void LoadDefaultSettings()
		{
			LoadDefaultSettings(false);
		}


		private bool LoadDefaultSettings(bool silent)
		{
			bool success = false;
			try
			{
				grid.SelectedObject = this.config = XsdTidyConfigManager.LoadDefault();
				success = true;
			}
			catch(Exception ex)
			{
				if (!silent) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return success;
		}

		#region Menu handlers

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.SaveSettings();
		}

		private void setAsDefaultmenuItem_Click(object sender, System.EventArgs e)
		{
			this.SaveDefaultSettings();
		}

		private void restoreDefaultMenuItem_Click(object sender, System.EventArgs e)
		{
			this.LoadDefaultSettings();			
		}

		private void generateMenuItem_Click(object sender, System.EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(this.Generate));
			thread.Start();
		}		
		#endregion
	}
}
