using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {

            var snippet = @"
                (defun plus-n (n)
                    (lambda (number)
                        (+ number n)))

                (defun map (number fn)
                    (fn number))

                (map 10 (plus-n 1))
                 ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
