using System;
using System.Collections.Generic;
using Cl.Extensions;
using static Cl.Helpers.FpUniverse;

namespace Cl.IO
{
    public static class RewindableSourceExtensions
    {
        public static bool RewindEol(this ISource source) => RewindMatched(source, Environment.NewLine);

        public static bool RewindLine(this ISource source)
        {
            bool loop(bool recur = false)
            {
                if (RewindEol(source)) return true;
                return source.Read() == -1 ? recur : loop(true);
            }
            return loop();
        }

        public static bool RewindWhitespaces(this ISource source)
        {
            bool loop(bool recur = false)
            {
                if (source.Eof()) return recur;
                var code = source.Read();
                if (char.IsWhiteSpace((char) code)) return loop(true);
                source.Buffer(code);
                return recur;
            }
            return loop();
        }

        public static bool RewindMatched(this ISource source, string pattern)
        {
            var codes = new LinkedList<int>();
            foreach (var ch in pattern)
            {
                var code = source.Peek();
                if (code == -1 || ch != (char) code)
                {
                    codes.ForEach(x => source.Buffer(x));
                    return false;
                }
                codes.AddFirst(source.Read());
            }
            return true;
        }

        public static bool RewindWhitespacesAndComments(this ISource source,
            string startsWith = ";", bool atLeastOne = false)
        {
            if (RewindWhitespaces(source))
            {
                atLeastOne = true;
            }
            if (!RewindMatched(source, startsWith)) return atLeastOne;
            Ignore(RewindLine(source));
            return RewindWhitespacesAndComments(source, startsWith, atLeastOne: true);
        }
    }
}
