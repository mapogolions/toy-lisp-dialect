using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {

            var snippet = @"
                (defun plus-one (x)
                    (+ x 1))

                (defun map (number fn)
                    (fn number))

                (map 10 plus-one)
                 ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
