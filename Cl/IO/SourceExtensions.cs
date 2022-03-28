using System;
using System.Collections.Generic;
using Cl.Extensions;
using static Cl.Helpers.FpUniverse;

namespace Cl.IO
{
    public static class SourceExtensions
    {
        public static bool RewindEol(this ISource source) => Rewind(source, Environment.NewLine);

        public static bool RewindLine(this ISource source)
        {
            bool loop(bool recur = false)
            {
                if (RewindEol(source)) return true;
                return source.Read() == -1 ? recur : loop(true);
            }
            return loop();
        }

        public static bool RewindSpaces(this ISource source)
        {
            bool loop(bool atLeastOne = false)
            {
                if (source.Eof()) return atLeastOne;
                var code = source.Read();
                if (char.IsWhiteSpace((char) code)) return loop(true);
                source.Buffer(code);
                return atLeastOne;
            }
            return loop();
        }

        public static bool Rewind(this ISource source, string pattern)
        {
            var codes = new LinkedList<int>();
            foreach (var ch in pattern)
            {
                var code = source.Peek();
                if (code == - 1 || ch != (char) code)
                {
                    codes.ForEach(x => source.Buffer(x));
                    return false;
                }
                codes.AddFirst(source.Read());
            }
            return true;
        }

        public static bool RewindSpacesAndComments(this ISource source,
            string startsWith = ";", bool atLeastOne = false)
        {
            if (RewindSpaces(source))
            {
                atLeastOne = true;
            }
            if (!Rewind(source, startsWith)) return atLeastOne;
            Ignore(RewindLine(source));
            return RewindSpacesAndComments(source, startsWith, atLeastOne: true);
        }
    }
}
