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

        public static class Eval
        {
            public const string InvalidFunctionCall = "Invalid function call";
            public const string InvalidLambdaBody = "Invalid function body format";
            public const string InvalidLambdaParameters = "Invalid function parameters format";
        }

        public static string UnboundVariable(ClSymbol identifier) => $"Unbound variable {identifier}";

        public static class BuiltIn
        {
            public const string ArgumentMustBeCell = "Argument must be a cell";
            public const string ClauseMustBeCell = "Clause must be a cell";
            public const string ElseClauseMustBeLast = "Else clause must be last condition";
            public const string UnsupportBinding = "Unsupport binding";
            public static string ArgumentIsNotOfType<T>() where T : IClObj => $"Argument is not of type {nameof(T)}";
        }
    }
}
