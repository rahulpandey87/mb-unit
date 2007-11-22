using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace MbUnit.Framework.Tests.Asserts
{
    [TestFixture]
    public class WebAssert_Test
    {
        #region Control assertions
        [Test]
        public void IsEnableViewState()
        {
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.EnableViewState = true;
            WebAssert.IsEnableViewState(txtBoxControl);
        }

        [Test]
        public void IsNotEnableViewState()
        {
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.EnableViewState = false;
            WebAssert.IsNotEnableViewState(txtBoxControl);
        }

        [Test]
        public void IsVisible()
        {
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.Visible = true;
            WebAssert.IsVisible(txtBoxControl);
        }

        [Test]
        public void IsNotVisible()
        {
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.Visible = false;
            WebAssert.IsNotVisible(txtBoxControl);
        }

        [Test]
        public void IsIDEqual()
        {
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.ID = "TxtBoxControl";
            WebAssert.IsIDEqual(txtBoxControl, "TxtBoxControl");
        }

        [Test]
        public void HasControls()
        {
            PlaceHolder place = new PlaceHolder();
            TextBox txtBoxControl = new TextBox();

            place.Controls.Add(txtBoxControl);

            WebAssert.HasControls(place);
        }

        [Test]
        public void HasNoControls()
        {
            TextBox txtBoxControl = new TextBox();
            WebAssert.HasNoControls(txtBoxControl);
        }

        [Test]
        public void AreTemplateSourceDirectoryEqual()
        {
            TextBox txtBoxControl = new TextBox();
            CheckBox chkBoxControl = new CheckBox();
            WebAssert.AreTemplateSourceDirectoryEqual(txtBoxControl, chkBoxControl);
        }

        [Test]
        [Ignore("FindControl() doesn't work")]
        public void IsChild()
        {
            PlaceHolder place = new PlaceHolder();
            TextBox txtBoxControl = new TextBox();
            txtBoxControl.ID = "TxtBoxControl";
            place.Controls.Add(txtBoxControl);

            WebAssert.IsChild(place, "TxtBoxControl");
        }

        [Test]
        public void IsNotChild()
        {
            PlaceHolder place = new PlaceHolder();
            TextBox txtBoxControl = new TextBox();

            WebAssert.IsNotChild(place, txtBoxControl);
        }

        #endregion

        #region Page Assertions

        [Test]
        public void AreErrorPageEqual()
        {
            Page pg = new Page();
            pg.ErrorPage = "404.html";

            WebAssert.AreErrorPageEqual("404.html", pg);
        }

        [Test]
        public void AreClientTargetEqual()
        {
            Page pg = new Page();
            pg.ClientTarget = "BrowserDetect.html";

            WebAssert.AreClientTargetEqual("BrowserDetect.html", pg);
        }

        [Test]
        [ExpectedException(typeof(MbUnit.Core.Exceptions.AssertionException))]
        public void IsPostBack()
        {
            Page pg = new Page();

            WebAssert.IsPostBack(pg);
        }

        [Test]
        public void IsNotPostBack()
        {
            Page pg = new Page();

            WebAssert.IsNotPostBack(pg);
        }

        [Test]
        public void IsValid()
        {
            Page pg = new Page();
            pg.Validate();
            WebAssert.IsValid(pg);
        }

        [Test]
        [ExpectedException(typeof(MbUnit.Core.Exceptions.AssertionException))]
        public void IsNotValid()
        {
            Page pg = new Page();
            pg.Validate();
            WebAssert.IsNotValid(pg);
        }

        [Test]
        [Ignore("SmartNavigation is always false")]
        public void IsSmartNavigation()
        {
            Page pg = new Page();
            pg.SmartNavigation = true;
            WebAssert.IsSmartNavigation(pg);
        }

        [Test]
        public void IsNotSmartNavigation()
        {
            Page pg = new Page();
            pg.SmartNavigation = false;
            WebAssert.IsNotSmartNavigation(pg);
        }

        #endregion
    }
}
