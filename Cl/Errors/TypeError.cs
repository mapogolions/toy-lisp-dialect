namespace Cl.Errors
{

    public class TypeError : Exception
    {
        public TypeError(string errorMessage) : base(errorMessage) { }
    }
}
