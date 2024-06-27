using Cl.IO;

namespace Cl.Extensions
{
    public static class SourceExtensions
    {
        public static bool SkipEol(this ISource source) => source.Skip("\n") || source.Skip("\r\n");

        public static bool SkipLine(this ISource source)
        {
            return loop(source);

            static bool loop(ISource source, bool recur = false)
            {
                if (source.SkipEol()) return true;
                return source.Read() == -1 ? recur : loop(source, true);
            }
        }

        public static bool SkipSpaces(this ISource source)
        {
            return loop(source);

            static bool loop(ISource source, bool atLeastOne = false)
            {
                if (source.Eof()) return atLeastOne;
                var code = source.Read();
                if (char.IsWhiteSpace((char)code)) return loop(source, true);
                source.Buffer(code);
                return atLeastOne;
            }
        }

        public static bool Skip(this ISource source, string pattern)
        {
            var codes = new LinkedList<int>();
            foreach (var ch in pattern)
            {
                var code = source.Peek();
                if (code == -1 || ch != (char)code)
                {
                    codes.ForEach(source.Buffer);
                    return false;
                }
                codes.AddFirst(source.Read());
            }
            return true;
        }

        public static bool SkipComments(this ISource source, string startsWith = ";")
        {
            return loop(source, startsWith, false);

            static bool loop(ISource source, string startsWith, bool atLeastOne)
            {
                if (source.SkipSpaces())
                {
                    atLeastOne = true;
                }
                if (!source.Skip(startsWith)) return atLeastOne;
                source.SkipLine();
                return SkipComments(source, startsWith);
            }
        }
    }
}
