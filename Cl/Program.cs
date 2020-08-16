using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                (define x
                    (cond
                        (#f 10)
                        (else 11)))
                x
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
