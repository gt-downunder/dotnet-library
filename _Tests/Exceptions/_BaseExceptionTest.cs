using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNet.Library.Tests.Exceptions
{
    [TestClass]
    [TestCategory("Extensions")]
    public class BaseExceptionTest : BaseTest
    {
        protected static void ThrowException(Exception ex) => throw ex;
    }
}
