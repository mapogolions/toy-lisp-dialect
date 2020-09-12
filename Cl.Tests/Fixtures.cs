using Cl.Types;

namespace Cl.Tests
{
    internal static class Var
    {
        internal static ClSymbol Foo = new ClSymbol("foo");
        internal static ClSymbol Bar = new ClSymbol("bar");
        internal static ClSymbol Fn = new ClSymbol("fn");
    }

    internal static class Value
    {
        internal static ClString Foo = new ClString("foo");
        internal static ClString Bar = new ClString("bar");
        internal static ClInt One = new ClInt(1);
    }
}
