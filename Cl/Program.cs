using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var partial = @"
                (define f
                    (lambda (x y) y))

                (define partially-applied (f 10))
                (partially-applied 11)
            ";
            using var reader = new Reader(partial);
            var evaluator = new Evaluator(new Env(BuiltIn.Env));
            var result = evaluator.Eval(reader.Read());
            Console.WriteLine(result);
            // new Repl().Start();
        }
    }
}
