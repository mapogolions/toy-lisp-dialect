using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                (cond
                    (#f 10)
                    ((list) 11)
                    (else 0))
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
