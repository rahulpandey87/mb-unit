using System;
using System.Windows.Forms;
using MbUnit.Framework;
using TestFu.Gestures;
using System.Threading;

namespace TestFu.Tests.Gestures
{
    [TestFixture]
    public class ClickGestureTest
    {
        private GestureTestForm form;
        private GestureFactory factory;

        [SetUp]
        public void SetUp()
        {
            form = new GestureTestForm();
            form.Show();
            factory = new GestureFactory(this.form);
        }

        [TearDown]
        public void TearDown()
        {
            if (form != null)
            {
                form.Dispose();
                form = null;
            }
        }

        [Test]
        public void ClickButtonOne()
        {
            IGesture gesture = factory.MouseClick(form.button1);
            RunGesture(gesture);
            Assert.AreEqual(1, form.button1ClickCount);
        }

        private void RunGesture(IGesture gesture)
        {
            ThreadRunner runner = new ThreadRunner(new ThreadStart(gesture.Start));
            runner.Run();
            if (runner.CatchedException != null)
                Assert.Fail(runner.CatchedException.ToString());
        }

        [Test]
        public void ClickButtonTwo()
        {
            IGesture gesture = factory.MouseClick(form.button2);
            RunGesture(gesture);
            Assert.AreEqual(1, form.button2ClickCount);
        }

        [Test]
        public void ClickButtonOneAndTwo()
        {
            IGesture gesture1 = factory.MouseClick(form.button1);
            IGesture gesture2 = factory.MouseClick(form.button2);
            IGesture gesture = factory.Sequence(gesture1, gesture2);
            RunGesture(gesture);

            Assert.AreEqual(1, form.button1ClickCount);
            Assert.AreEqual(1, form.button2ClickCount);
        }
    }
}
