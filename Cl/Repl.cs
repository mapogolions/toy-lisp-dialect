using System;
using System.Linq;
using System.Text;
using Cl.Contracts;

namespace Cl
{
    public class Repl
    {
        private readonly string _sign;
        public Repl(string sing) => _sign = sing;

        public string Indent => $"{string.Concat(Enumerable.Repeat(" ", _sign.Length))}";
        public string MultiLineInput => $"{Indent}|";
        public string ResultLine => $"{Indent}|> ";

        public void Start()
        {
            IContext ctx = new Context(BuiltIn.Env);
            var snippet = new StringBuilder();
            Console.Write($"{_sign} ");
            while (true)
            {
                try
                {
                    var line = Console.ReadLine();
                    snippet.Append(line);
                    if (string.IsNullOrEmpty(line))
                    {
                        using var reader = new Reader(snippet.ToString());
                        snippet.Clear();
                        ctx = BuiltIn.Eval(reader.Read(), ctx);
                        Console.Write($"{ResultLine}{ctx.Value}{Environment.NewLine}{_sign} ");
                        continue;
                    }
                    Console.Write(MultiLineInput);
                }
                catch (Exception ex)
                {
                    snippet.Clear();
                    Console.Write($"{ex.Message}{Environment.NewLine}{_sign} ");
                }
            }
        }
    }
}
