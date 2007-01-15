using System;
using System.Windows.Forms;

namespace QuickGraph.Layout.Forms
{
	/// <summary>
	/// Summary description for ZoomWheelMouseBehavior.
	/// </summary>
	public class ZoomWheelMouseBehavior : IMouseBehavior
	{
		#region IMouseBehavior Members

		public void Attach(QuickNetronPanel panel)
		{
			panel.AutoScroll = false;
			panel.MouseWheel+=new System.Windows.Forms.MouseEventHandler(panel_MouseWheel);
		}

		public void Detach(QuickNetronPanel panel)
		{
			panel.AutoScroll = true;
			panel.MouseWheel-=new System.Windows.Forms.MouseEventHandler(this.panel_MouseWheel);
		}

		#endregion

		private void panel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			QuickNetronPanel panel = (QuickNetronPanel)sender;
			if (e.Delta!=0)
			{
				float zoom = panel.Zoom
					+ e.Delta*SystemInformation.MouseWheelScrollLines/120.0f/100;

				panel.Zoom = Math.Abs(zoom); 
				panel.Invalidate(true);
			}			
		}
	}
}
