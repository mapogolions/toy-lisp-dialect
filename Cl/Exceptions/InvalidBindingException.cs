using System;

namespace Cl.Exceptions
{
    public class InvalidBindingException : Exception
    {
        public InvalidBindingException(string foundType)
            : base($"{foundType} couldn't be left side of binding statement") { }
    }
}
