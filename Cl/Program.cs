using System;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {

            var snippet = @"
                (double-of-string '12')
                 ";
            using var reader = new Reader(snippet);
            var (result, _) = BuiltIn.Eval(reader.Read());
            Console.WriteLine(result);
        }
    }
}
