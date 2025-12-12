
namespace Cl.Errors;

public class UnboundVariableError(string identifier) : Exception($"Unbound variable {identifier}")
{
}
