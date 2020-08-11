using System;
using System.Linq;
using Cl.Contracts;
using Cl.Types;

namespace Cl
{
    class Program
    {
        static void Main(string[] args)
        {
            var snippet = @"
                (define f
                    (lambda (x y) y))
                (f 10 11)
            ";
            using var reader = new Reader(snippet);
            var (result, _) = reader.Read()
                .Aggregate<IClObj, IContext>(new Context(BuiltIn.Env), (ctx, obj) => obj.Reduce(ctx));
            Console.WriteLine(result);
        }
    }
}
