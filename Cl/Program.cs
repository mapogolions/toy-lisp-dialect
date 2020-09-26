using Cl.DefaultContracts;

namespace Cl
{
    class Program
    {
        static void Main(string[] args) =>
            new Repl(">>>", snippet => new Reader(snippet))
                .Start(new Context(BuiltIn.Env));
    }
}
