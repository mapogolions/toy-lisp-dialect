using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"
                (define my-first-procedure
                    (lambda (x)
                        (if x 11 10)))

                (my-first-procedure #t)
            ";
            // Todo
            var closure = @"
                (define x 10)
                (define f
                    (lambda (x)
                        (lambda () x)))

                (define g (f 10))
                (g)
            ";
            var reader = new Reader(closure);
            var evaluator = new Evaluator(new Env(BuiltIn.Env));
            var result = evaluator.Eval(reader.Read());
            Console.WriteLine(result);
            // new Repl().Start();
        }
    }
}
