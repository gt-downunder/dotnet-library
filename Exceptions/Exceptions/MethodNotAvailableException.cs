﻿using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class MethodNotAvailableException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Method not available";

        public override HttpStatusCode StatusCode => HttpStatusCode.NotImplemented;
    }
}
