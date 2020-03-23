using System;

namespace RESS.Shared.Exceptions
{
    public abstract class DomainException : Exception
    {
        public abstract string ErrorCode { get; }

        protected DomainException() { }

        protected DomainException(string message) : base(message)
        {
        }
    }
}