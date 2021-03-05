namespace Cl.Errors
{
    using System;

    public class TypeError : Exception
    {
        public TypeError(string errorMessage) : base(errorMessage) { }
    }
}
