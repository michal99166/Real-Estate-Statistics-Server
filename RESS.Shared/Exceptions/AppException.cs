using System;

namespace RESS.Shared.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string ErrorCode { get; }

        protected AppException(string message) : base(message) { }
    }
}