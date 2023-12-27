using System.Text;
using Cl.Readers;
using Cl.IO;

namespace Cl.Utop
{
    public sealed class Repl
    {
        private readonly string _prompt;
        private readonly Reader _reader = new();

        public Repl(string prompt)
        {
            _prompt = prompt;
        }

        public string Indent => $"{string.Concat(Enumerable.Repeat(" ", _prompt.Length))}";
        public string MultiLineInput => $"{Indent}|";
        public string ResultLine => $"{Indent}|> ";

        public void Start(params string[] args)
        {
            var ctx = BuiltIn.StdLib(args.Length > 0 ? args[0] : null);
            var snippet = new StringBuilder();
            Console.Write($"{_prompt} ");
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
                        Console.Write($"{ResultLine}{ctx.Value}{Environment.NewLine}{_prompt} ");
                        continue;
                    }
                    Console.Write(MultiLineInput);
                }
                catch (Exception ex)
                {
                    snippet.Clear();
                    Console.Write($"{ex.Message}{Environment.NewLine}{_prompt} ");
                }
            }
        }
    }
}
