// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Windows.Forms;

namespace Gallio.Icarus
{
    public partial class ReloadDialog : Form
    {
        public bool AlwaysReloadTests
        {
            get { return alwaysReload.Checked; }
        }

        public ReloadDialog(string assembly)
        {
            InitializeComponent();
                                                 
            assemblyLabel.Text = string.Format("The assembly {0} has been modified.", assembly);
        }
    }
}
