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

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using Gallio.Icarus.Controllers;
using Gallio.Model;

namespace Gallio.Icarus.Controls
{
    public class TestTreeView : TreeViewAdv
    {
        private readonly NodeCheckBox nodeCheckBox;

        public bool EditEnabled
        {
            get { return nodeCheckBox.EditEnabled; }
            set { nodeCheckBox.EditEnabled = value; }
        }

        public TestTreeView()
        {
            nodeCheckBox = new NodeCheckBox();
            nodeCheckBox.DataPropertyName = "CheckState";
            nodeCheckBox.LeftMargin = 0;
            nodeCheckBox.ParentColumn = null;
            nodeCheckBox.ThreeState = true;
            NodeControls.Add(nodeCheckBox);

            NodeIcon nodeTypeIcon = new NodeIcon();
            nodeTypeIcon.DataPropertyName = "NodeTypeIcon";
            nodeTypeIcon.LeftMargin = 1;
            nodeTypeIcon.ParentColumn = null;
            NodeControls.Add(nodeTypeIcon);

            NodeIcon testStateIcon = new NodeIcon();
            testStateIcon.DataPropertyName = "TestStatusIcon";
            testStateIcon.LeftMargin = 1;
            testStateIcon.ParentColumn = null;
            NodeControls.Add(testStateIcon);

            NodeTextBox nodeTextBox = new NodeTextBox();
            nodeTextBox.DataPropertyName = "Text";
            nodeTextBox.IncrementalSearchEnabled = true;
            nodeTextBox.EditEnabled = false;
            nodeTextBox.LeftMargin = 3;
            nodeTextBox.ParentColumn = null;
            nodeTextBox.DrawText += nodeTextBox_DrawText;
            NodeControls.Add(nodeTextBox);
        }

        private static void nodeTextBox_DrawText(object sender, DrawEventArgs e)
        {
            TestTreeNode node = (TestTreeNode)e.Node.Tag;
            switch (node.TestStatus)
            {
                case TestStatus.Passed:
                    e.TextColor = OptionsController.Instance.PassedColor;
                    break;
                case TestStatus.Failed:
                    e.TextColor = OptionsController.Instance.FailedColor;
                    break;
                case TestStatus.Skipped:
                    e.TextColor = OptionsController.Instance.SkippedColor;
                    break;
                case TestStatus.Inconclusive:
                    e.TextColor = OptionsController.Instance.InconclusiveColor;
                    break;
            }
        }
    }
}
