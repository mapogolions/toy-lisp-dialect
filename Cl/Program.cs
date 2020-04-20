using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexicalEnvironment = @"
                (define f
                    (lambda (x)
                        (lambda (x)
                            (lambda () x))))

                (define g (f 10))
                (define h (g 11))
                (h)
            ";
            var reader = new Reader(lexicalEnvironment);
            var evaluator = new Evaluator(new Env(BuiltIn.Env));
            var result = evaluator.Eval(reader.Read());
            Console.WriteLine(result);
            // new Repl().Start();
        }
    }
}
