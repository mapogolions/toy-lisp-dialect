using System.Text;
using Cl.Readers;
using Cl.IO;
using Cl.Types;
using System.Linq;
using System;

namespace Cl.Utop
{
    public class Repl
    {
        private readonly string _sign;
        private readonly IReader<ClObj> _reader;

        public Repl(string sing, IReader<ClObj> reader)
        {
            _sign = sing;
            _reader = reader;
        }

        public string Indent => $"{string.Concat(Enumerable.Repeat(" ", _sign.Length))}";
        public string MultiLineInput => $"{Indent}|";
        public string ResultLine => $"{Indent}|> ";

        public void Start(IContext ctx)
        {
            var snippet = new StringBuilder();
            Console.Write($"{_sign} ");
            while (true)
            {
                try
                {
                    var line = Console.ReadLine();
                    snippet.AppendLine(line);
                    if (string.IsNullOrEmpty(line))
                    {
                        using var source = new Source(snippet.ToString());
                        snippet.Clear();
                        var obj = _reader.Read(source);
                        ctx = obj.Reduce(ctx);
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
