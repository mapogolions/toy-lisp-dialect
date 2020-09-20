using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {

            var snippet = @"
                (filter (list 1 2) (lambda (x) #f))
            ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
