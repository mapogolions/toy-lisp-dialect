using Cl.Readers;

namespace Cl
{
    internal static class Program
    {
        internal static void Main() =>
            new Repl(">", new Reader()).Start(new Context(BuiltIn.Env));
    }
}
