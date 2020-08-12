using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                (define f
                    (lambda (x x) x))
                (f 10 11)
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
