using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Gallio.Icarus.WindowManager;
using Gallio.UI.Menus;
using MbUnit.Framework;
using NHamcrest.Core;

namespace Gallio.Icarus.Tests.WindowManager
{
    public class MenuManagerTests
    {
        [Test]
        public void Queued_menu_commands_are_added_when_the_toolbar_is_set()
        {
            var menuManager = new MenuManager();
            const string menuId = "MenuId";
            menuManager.Add(menuId, () => new MenuCommand());
            var items = new List<ToolStripMenuItem>();

            menuManager.SetToolstrip(items);

            Assert.AreEqual(1, items.Count);
            var toolStripMenuItem = items.First();
			Assert.AreEqual(menuId, toolStripMenuItem.Text);
			Assert.AreEqual(1, toolStripMenuItem.DropDownItems.Count);
        }

        [Test]
        public void Menu_commands_added_after_toolbar_is_set_are_added_immediately()
        {
            var menuManager = new MenuManager();
            const string menuId = "MenuId";
            var items = new List<ToolStripMenuItem>();
            menuManager.SetToolstrip(items);

            menuManager.Add(menuId, () => new MenuCommand());

			Assert.AreEqual(1, items.Count);
			var toolStripMenuItem = items.First();
			Assert.AreEqual(menuId, toolStripMenuItem.Text);
			Assert.AreEqual(1, toolStripMenuItem.DropDownItems.Count);
        }
    }
}