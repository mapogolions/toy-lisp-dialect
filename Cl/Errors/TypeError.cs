namespace Cl.Errors;


public class TypeError(string errorMessage) : Exception(errorMessage)
{
}
