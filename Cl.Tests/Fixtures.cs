using Cl.Types;

namespace Cl.Tests;

internal static class Var
{
    internal static ClSymbol Foo = new("foo");
    internal static ClSymbol Bar = new("bar");
    internal static ClSymbol Fn = new("fn");
}

internal static class Value
{
    internal static ClString Foo = new("foo");
    internal static ClString Bar = new("bar");
    internal static ClInt One = new(1);
}
