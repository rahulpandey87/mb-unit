using System;
using System.Collections;
using System.IO;
using System.Reflection;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Exceptions;

namespace MbUnit.Tests.Core.Invokers
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="ExpectedExceptionRunInvoker"/> class.
    /// </summary>
    [TestFixture]
    public class ExpectedExceptionRunInvokerTest
    {
        private string description="Description";
        #region Tests
        [Test]
        [ExpectedArgumentNullException]
        public void InvokerNull()
        {
            ExpectedExceptionRunInvoker einvoker =
                new ExpectedExceptionRunInvoker(null, typeof(Exception),description);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void ExceptionTypeNull()
        {
            ExpectedExceptionRunInvoker einvoker =
                new ExpectedExceptionRunInvoker(new ThrowExceptionRunInvoker(null)
                    , null, description);
        }

        [Test]
        public void Test()
        {
            ExecuteInvoker(typeof(ArgumentNullException), new ArgumentNullException("hello"));
        }

        [Test]
        public void DescriptionInExceptionTypeMistmachException()
        {
            try
            {
                ExecuteInvoker(typeof(ArgumentNullException), new ArgumentException("hello"));
            }
            catch (ExceptionTypeMistmachException ex)
            {
                StringAssert.StartsWith(ex.Message, description);
            }
        }

        [Test]
        public void DescriptionInExceptionNotThrownException()
        {
            try
            {
                ExecuteInvoker(typeof(ArgumentNullException), null);
            }
            catch (ExceptionNotThrownException ex)
            {
                StringAssert.StartsWith(ex.Message, description);
            }
        }

        [Test]
        [ExpectedException(typeof(ExceptionNotThrownException))]
        public void ExceptionNotThrown()
        {
            ExecuteInvoker(typeof(ArgumentNullException), null);
        }

        [Test]
        [ExpectedException(typeof(ExceptionTypeMistmachException))]
        public void ExceptionMistmatch()
        {
            ExecuteInvoker(typeof(InvalidOperationException), new ArgumentNullException("hello"));
        }

        protected ExpectedExceptionRunInvoker ExecuteInvoker(Type expectedType, Exception ex)
        {
            ThrowExceptionRunInvoker invoker = new ThrowExceptionRunInvoker(ex);            
            ExpectedExceptionRunInvoker einvoker =
                new ExpectedExceptionRunInvoker(invoker, expectedType,description);
            ArrayList list = new ArrayList();
            einvoker.Execute(null, list);
            Assert.IsTrue(invoker.Invoked);
            return einvoker;
        }
        #endregion

        #region ThrowExceptionRunInvoker
        public class ThrowExceptionRunInvoker : IRunInvoker
        {
            private Exception ex;
            private bool invoked=false;

            public ThrowExceptionRunInvoker(Exception ex)
            {
                this.ex=ex;
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
                if (ex!=null)
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

