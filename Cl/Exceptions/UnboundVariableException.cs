using System;

namespace Cl.Exceptions
{
    public class UnboundVariableException : Exception
    {
        public UnboundVariableException(string identifier) : base($"Unbound variable {identifier}") { }
    }
}
