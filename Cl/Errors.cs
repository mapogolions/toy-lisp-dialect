using Cl.Types;

namespace Cl
{
    public static class Errors
    {
        public static class Reader
        {
            public const string ReadIllegalState = "Read illegal state";
            public static string UnknownLiteral(string type) => $"Unknown {type} literal";
        }

        public static string UnboundVariable(ClSymbol identifier) => $"Unbound variable {identifier}";

        public static class BuiltIn
        {
            public const string ArgumentMustBeCell = "Argument must be a cell";
        }
    }
}
