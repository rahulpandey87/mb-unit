using System;
using System.Reflection;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing.Design;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.ComponentModel
{
    [ComponentFixture]
    [ToolboxBitmap(typeof(ComponentFixtureBase),"ComponentFixture.png")]
    [ToolboxItem(false)]
    public class ComponentFixtureBase : Component
    {
        public ComponentFixtureBase()
        { }
        public ComponentFixtureBase(IContainer container)
        {
            container.Add(this);
        }

        [Category("Test")]
        [Description("Invoked once to initialize the fixture")]
        public event EventHandler FixtureSetUp;
        protected virtual void OnFixtureSetUp(EventArgs args)
        {
            if (this.FixtureSetUp != null)
                this.FixtureSetUp(this, args);
        }

        [Category("Test")]
        [Description("Invoked before each test case")]
        public event EventHandler SetUp;
        protected virtual void OnSetUp(EventArgs args)
        {
            if (this.SetUp != null)
                this.SetUp(this, args);
        }

        [Category("Test")]
        [Description("Invoked once after each test case")]
        public event EventHandler TearDown;
        protected virtual void OnTearDown(EventArgs args)
        {
            if (this.TearDown != null)
                this.TearDown(this, args);
        }

        [Category("Test")]
        [Description("Invoked once to clean up the fixture")]
        public event EventHandler FixtureTearDown;
        protected virtual void OnFixtureTearDown(EventArgs args)
        {
            if (this.FixtureTearDown != null)
                this.FixtureTearDown(this,args);
        }

        #region SetUp and TearDown
        [TestFixtureSetUp]
        public void InvokeFixtureSetUp()
        {
            this.OnFixtureSetUp(EventArgs.Empty);
        }
        [SetUp]
        public void InvokeSetUp()
        {
            this.OnSetUp(EventArgs.Empty);
        }
        [TearDown]
        public void InvokeTearDown()
        {
            this.OnTearDown(EventArgs.Empty);
        }
        [TestFixtureTearDown]
        public void InvokeFixtureTearDown()
        {
            this.OnFixtureTearDown(EventArgs.Empty);
        }
        #endregion

        private void InitializeComponent()
        {

        }
    }
}
