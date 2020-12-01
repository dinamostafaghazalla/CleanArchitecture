using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, Exception ex) : base("DomainException: ", ex)
        {
        }

        public DomainException() : base()
        {
        }

        public DomainException(string message) : base(message)
        {
        }
    }
}