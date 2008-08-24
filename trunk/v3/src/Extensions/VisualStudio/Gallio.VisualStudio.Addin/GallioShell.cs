using System;
using System.Runtime.InteropServices;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Gallio.VisualStudio.Toolkit;
using Gallio.VisualStudio.Toolkit.Actions;

namespace Gallio.VisualStudio.Addin
{
    [ComVisible(true)]
	public class GallioShell : Shell
	{
        private static GallioShell instance;

        /// <summary>
        /// Gets the instance of the add-in.
        /// </summary>
        public static GallioShell Instance
        {
            get { return instance; }
        }

        public override void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            instance = this;

            base.OnConnection(application, connectMode, addInInst, ref custom);
        }

        public override void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            base.OnDisconnection(disconnectMode, ref custom);

            instance = null;
        }
	}
}