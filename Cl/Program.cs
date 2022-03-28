using Cl;
using Cl.Readers;

namespace Cl
{
    internal static class Program
    {
        // internal static void Main() =>
        //     new Repl(">>>", s => new Reader(s))
        //         .Start(new Context(BuiltIn.Env));

        internal static void Main()
        {
            var reader = new Readers.Reader(new ClObjReader());
            new Repl(">>>", reader).Start(new Context(BuiltIn.Env));
        }
    }
}
