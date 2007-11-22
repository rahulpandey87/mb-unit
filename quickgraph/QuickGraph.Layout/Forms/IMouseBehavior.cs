using System;
using System.Windows.Forms;

namespace QuickGraph.Layout.Forms
{
	public interface IMouseBehavior
	{
		void Attach(QuickNetronPanel panel);
		void Detach(QuickNetronPanel panel);
	}
}
