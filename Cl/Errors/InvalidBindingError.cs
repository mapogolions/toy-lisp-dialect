using System;

namespace Cl.Errors
{
    public class InvalidBindingError : Exception
    {
        public InvalidBindingError(string foundType)
            : base($"{foundType} couldn't be left side of binding statement") { }
    }
}
