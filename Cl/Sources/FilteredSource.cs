using System;
using System.Collections.Generic;
using Cl.Extensions;

namespace Cl.Sources
{
    public class FilteredSource : BySource
    {
        public FilteredSource(ISource source) : base(source) { }

        public FilteredSource(string source) : this(new Source(source)) { }

        public override bool SkipEol() => SkipMatched(Environment.NewLine);

        public override bool SkipLine()
        {
            bool loop(bool recur = false)
            {
                if (SkipEol()) return true;
                return _source.Read() == -1 ? recur : loop(true);
            }
            return loop();
        }

        public override bool SkipWhitespaces()
        {
            bool loop(bool recur = false)
            {
                if (_source.Eof()) return recur;
                var code = _source.Read();
                if (char.IsWhiteSpace((char) code)) return loop(true);
                _source.Buffer(code);
                return recur;
            }
            return loop();
        }

        public override bool SkipMatched(string pattern)
        {
            var codes = new LinkedList<int>();
            foreach (var ch in pattern)
            {
                var code = _source.Peek();
                if (code == -1 || ch != (char) code)
                {
                    codes.ForEach(it => _source.Buffer(it));
                    return false;
                }
                codes.AddFirst(_source.Read());
            }
            return true;
        }
    }
}
