/*
 * Created by SharpDevelop.
 * User: dehalleux
 * Date: 17/06/2004
 * Time: 13:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;
using Reflector.CodeModel;
using MbUnit.Forms;

namespace Reflector.Graph.Faulty
{	
	
	public class MbUnitPackage : BasePackage
	{
		[ReflectorWindow("MbUnit")]
		[ReflectorCommandBar(CommandBarTarget.Assembly)]
		private MbUnitWindow mbUnit=new MbUnitWindow();				
	}
	
	public class MbUnitWindow : ReflectorTreeView
	{
		private ReflectorServices services =null;
		
		public MbUnitWindow()
		{
			this.Dock =DockStyle.Fill;
		}
		
		public ReflectorServices Services
		{
			get
			{
				return this.services;
			}
			set
			{
				if (this.services!=null)
				{
					this.services.AssemblyBrowser.ActiveItemChanged-=new EventHandler(this.activeItem_Changed);										
				}
				this.services=value;
				if (this.services!=null)
				{
					this.services.AssemblyBrowser.ActiveItemChanged+=new EventHandler(this.activeItem_Changed);					
				}
			}
		}
		
		private void activeItem_Changed(Object sender, EventArgs args)
		{
			this.RemoveAssemblies();
			IAssembly assembly = this.Services.ActiveAssembly;
			if (assembly!=null)
				this.Translate();
		}
				
		public void Translate()
		{
			IAssembly assembly =this.Services.ActiveAssembly;
			if (assembly==null)
				return;
			
			this.RemoveAssemblies();
			this.AddAssembly(assembly.Location);
			this.ThreadedPopulateTree();
		}
	}
}
