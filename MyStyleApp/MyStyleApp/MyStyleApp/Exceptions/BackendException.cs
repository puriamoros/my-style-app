using MyStyleApp.Models;
using System;

namespace MyStyleApp.Exceptions
{
    public class BackendException: Exception
    {
        public BackendError BackendError { get; private set; }

        public BackendException(BackendError backendError)
            : base(backendError.Message)
        {
            BackendError = backendError;
        }
    }
}
