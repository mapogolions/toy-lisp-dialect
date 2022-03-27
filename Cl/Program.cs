using Cl.Core;

namespace Cl
{
    internal static class Program
    {
        internal static void Main() =>
            new Repl(">>>", s => new Reader(s))
                .Start(new Context(BuiltIn.Env));
    }
}
