using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                (let ((x 10)
                      (y 11))
                    (list x y))
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
