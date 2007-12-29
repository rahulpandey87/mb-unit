using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Exceptions;
using System.Reflection;

namespace MbUnit.Tests.Core.Invokers
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="ExpectedExceptionRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class ConditionalExceptionRunInvokerTest
    {
        #region Tests
        [Test]
        [ExpectedArgumentNullException]
        public void InvokerNull()
        {
            ConditionalExceptionRunInvoker einvoker =
                new ConditionalExceptionRunInvoker(null, typeof(Exception),"Description","ShouldThrow");
        }

        [Test]
        [ExpectedArgumentNullException]
        public void ExceptionNull()
        {
            ConditionalExceptionRunInvoker einvoker =
                new ConditionalExceptionRunInvoker(new ThrowExceptionRunInvoker(null)
                    , null
                    , "Description"
                    ,"ShouldThrow");
        }

        [Test]
        public void Test()
        {
            ExecuteInvoker(typeof(ArgumentNullException), new ArgumentNullException("hello"),true);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptionThrownAndNotExpected()
        {
            ExecuteInvoker(typeof(ArgumentNullException), new ArgumentNullException("hello"), false);
        }

        [Test]
        [ExpectedException(typeof(ExceptionNotThrownException))]
        public void ExceptionNotThrown()
        {
            ExecuteInvoker(typeof(ArgumentNullException), null,true);
        }

        [Test]
        [ExpectedException(typeof(ExceptionTypeMistmachException))]
        public void ExceptionMistmatch()
        {
            ExecuteInvoker(typeof(InvalidOperationException), new ArgumentNullException("hello"),true);
        }

        protected void ExecuteInvoker(Type expectedType, Exception ex, bool shouldThrow)
        {
            ThrowExceptionRunInvoker invoker = new ThrowExceptionRunInvoker(ex);
            ConditionalExceptionRunInvoker einvoker =
                new ConditionalExceptionRunInvoker(invoker, expectedType,"Description", "ShouldThrow");
            ArrayList list = new ArrayList();
            list.Add(shouldThrow);
            ThrowOracle oracle = new ThrowOracle();
            einvoker.Execute(oracle, list);
            Assert.IsTrue(invoker.Invoked);
        }

        #endregion

        #region ThrowOracle
        public class ThrowOracle
        {
            public bool ShouldThrow(bool shouldThrow)
            {
                return shouldThrow;
            }
        }
        #endregion

        #region Invoker
        public class ThrowExceptionRunInvoker : IRunInvoker
        {
            private Exception ex;
            private bool invoked = false;

            public ThrowExceptionRunInvoker(Exception ex)
            {
                this.ex = ex;
            }
            String IRunInvoker.Name
            {
                get
                {
                    return "ThrowExceptionRunInvoker";
                }
            }

            MbUnit.Core.Runs.IRun IRunInvoker.Generator
            {
                get
                {
                    return null;
                }
            }

            public bool Invoked
            {
                get
                {
                    return this.invoked;
                }
            }

            Object IRunInvoker.Execute(Object o, IList args)
            {
                this.invoked = true;
                if (ex != null)
                    throw ex;
                return null;
            }

            public bool ContainsMemberInfo(MemberInfo m)
            {
                return false;
            }
        }
        #endregion
    }
}

