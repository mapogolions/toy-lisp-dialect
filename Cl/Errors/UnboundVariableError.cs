using System;

namespace Cl.Errors
{
    public class UnboundVariableError : Exception
    {
        public UnboundVariableError(string identifier) : base($"Unbound variable {identifier}") { }
    }
}
