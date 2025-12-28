using System;

namespace CollegeGradingSys.Utilities.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
