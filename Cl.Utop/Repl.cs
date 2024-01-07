using System.Text;
using Cl.Readers;
using Cl.IO;
using Cl.Types;

namespace Cl.Utop
{
    public sealed class Repl
    {
        private readonly string _prompt;
        private readonly string _indent;
        private readonly string _multilineInput;
        private readonly string _resultLine;
        private readonly Reader _reader = new();

        public Repl(string prompt)
        {
            _prompt = prompt;
            _indent = $"{string.Concat(Enumerable.Repeat(" ", _prompt.Length))}";
            _multilineInput = $"{_indent}|";
            _resultLine = $"{_indent}|> ";
        }

        public void Start(params string[] args)
        {
            var ctx = BuiltIn.StdLib(args.Length > 0 ? args[0] : null);
            var snippet = new StringBuilder();
            Prompt();
            while (true)
            {
                try
                {
                    var line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        snippet.AppendLine(line);
                        MultilineInput();
                        continue;
                    }
                    using var source = new Source(snippet.ToString());
                    var obj = _reader.Read(source);
                    ctx = obj.Reduce(ctx);
                    PrintResult(ctx.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                snippet.Clear();
                Prompt();
            }
        }

        private void Prompt() => Console.Write($"{_prompt} ");
        private void MultilineInput() => Console.Write(_multilineInput);
        private void PrintResult(ClObj result) => Console.WriteLine($"{_resultLine}{result}");
    }
}
