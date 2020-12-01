using System;

namespace CleanArchitecture.Domain.Exceptions
{
    public class DuplicateException : DomainException
    {
        public DuplicateException(string message) : base(message)
        {
        }

        public DuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}