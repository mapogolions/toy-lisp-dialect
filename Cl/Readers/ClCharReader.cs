using Cl.IO;
using Cl.Types;
using Cl.Errors;
using Cl.Extensions;

namespace Cl.Readers
{
    public class ClCharReader : IReader<ClChar>
    {
        public ClChar? Read(ISource source)
        {
            if (!source.Skip(@"#\")) return null;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!source.Skip(word)) continue;
                return new ClChar(ch);
            }
            if (source.Eof())
                 throw new SyntaxError($"Invalid format of the {nameof(ClChar)} literal");
            return new ClChar((char) source.Read());
        }

        private static IDictionary<string, char> SpecialChars = new Dictionary<string, char>
            {
                ["newline"] = '\n',
                ["tab"] = '\t',
                ["space"] = ' '
            };
    }
}
