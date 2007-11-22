using System;
using System.Collections;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Exceptions;

namespace MbUnit.Framework
{
    public sealed class ExpectedExceptionTestCase : TestCaseDecoratorBase
    {
        private Type exceptionType;
        public ExpectedExceptionTestCase(ITestCase testCase,Type exceptionType)
            :base(testCase)
        {
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            this.exceptionType = exceptionType;
        }

        public Type ExceptionType
        {
            get
            {
                return this.exceptionType;
            }
        }

        public override Object Invoke(Object o, IList args)
        {
            try
            {
                Object result = base.Invoke(o, args);
            }
            catch (Exception ex)
            {
                Exception catchedException = ex;
                if (catchedException is TargetInvocationException)
                    catchedException = ex.InnerException;

                Exception current = catchedException;

                while (!this.ExceptionType.IsInstanceOfType(current))
                {
                    current = current.InnerException;
                    if (current == null)
                        throw new ExceptionTypeMistmachException(
                            this.ExceptionType,
                            this.TestCase.Description,
                            catchedException
                            );
                }

                return null;
            }
            // if we are here it did not throw
            throw new ExceptionNotThrownException(ExceptionType,this.Description);
        }
    }
}
