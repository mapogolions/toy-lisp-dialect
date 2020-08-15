using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                ;; custom function
                (define f
                    (lambda () (list 1 2 #f 4.12 #t)))

                ;; like HOF
                (define g
                    (lambda () tail))

                ;; skip the first two items
                (cdr
                    ;; comment here is valid
                    ((g) (f)))
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
