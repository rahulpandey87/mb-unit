using System;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.IO;
using System.IO.IsolatedStorage;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Framework.Security;
using System.Windows.Forms;
using System.Reflection;
using System.Data.SqlClient;

namespace MbUnit.Demo
{
    [TestFixture(ApartmentState = System.Threading.ApartmentState.STA)]
    [ExpectedException(typeof(SecurityException))]
    public class SecurityTest
    {
        [Test]
        [DenyFileIO]
        public void SecureMethod()
        {
            using (StreamWriter writer = new StreamWriter("test.txt"))
            {
                writer.Write("we should not be here");
            }
        }

        [Test]
        [DenyFileDialog]
        public void OpenDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
        }

        [Test]
        [DenyFileDialog]
        public void SaveDialog()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();
        }

        [Test]
        [DenyIsolatedFileStorage]
        public void IsolatedStorage()
        {
            // Get a new isolated store for this user, domain, and assembly.  
            // Put the store into an IsolatedStorageFile object.

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

            // This code creates a few different directories.

            isoStore.CreateDirectory("TopLevelDirectory");
            isoStore.CreateDirectory("TopLevelDirectory/SecondLevel");

            // This code creates two new directories, one inside the other.
            isoStore.CreateDirectory("AnotherTopLevelDirectory/InsideDirectory");


            // This file is placed in the root.

            IsolatedStorageFileStream isoStream1 = new IsolatedStorageFileStream("InTheRoot.txt", FileMode.Create, isoStore);
            Console.WriteLine("Created a new file in the root.");
            isoStream1.Close();

            // This file is placed in the InsideDirectory.

            IsolatedStorageFileStream isoStream2 = new IsolatedStorageFileStream("AnotherTopLevelDirectory/InsideDirectory/HereIAm.txt", FileMode.Create, isoStore);
            isoStream2.Close();

            Console.WriteLine("Created a new file in the InsideDirectory.");

        }

        [Test]
        [DenyUI]
        public void ShowMessageBox()
        {
            MessageBox.Show("You should not see this");
        }

        [Test]
        [DenyUI]
        public void UseClipboard()
        {
            IDataObject dao = Clipboard.GetDataObject();
        }
/*
        [Test]
        [DenyReflection]
        public void ReflectPrivateType()
        {
            FieldInfo fi =
                typeof(MbUnit.Core.Fixture).GetField("type",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(fi);
            Console.WriteLine(fi);
        }
        */

        [Test]
        [DenySqlClient]
        public void ConnectSqlClient()
        {
            using (SqlConnection conn = new SqlConnection("server=localhost"))
            {
                conn.Open();
            }
        }
    }
}
