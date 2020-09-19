using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {

            var snippet = @"
                (list
                    (+ 1 2 3 4 (- 2.5))
                    (* 1 2 3 4)
                    (+)
                    (*))
                 ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
