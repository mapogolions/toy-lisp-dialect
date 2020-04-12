using System;
using Cl.Abs;

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
            var reader = new Reader(source);
            var evaluator = new Evaluator(new Env(BuiltIn.Env));
            var result = evaluator.Eval(reader.Read());
            Console.WriteLine(result);
            // new Repl().Start();
        }
    }
}
