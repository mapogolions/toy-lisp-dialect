using System.Linq;
using Cl.Extensions;

namespace Cl.Input
{
    public class FilteredSource : PassThroughSource
    {
        public FilteredSource(ISource source) : base(source)
        {
        }

        public FilteredSource(string source) : this(new Source(source))
        {
        }

        public override bool SkipEol()
        {
            if (_source.Eof()) return false;
            var code = _source.Read();
            if (OsxEol(code) || WinEol(code) || UnixEol(code))
                return true;
            _source.Buffer(code);
            return false;
        }

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
            var codes = Enumerable.Empty<int>();
            foreach (var ch in pattern)
            {
                var code = _source.Peek();
                if (code == -1 || ch != (char) code)
                {
                    codes.ForEach(it => _source.Buffer(it));
                    return false;
                }
                codes.Prepend(_source.Read());
            }
            return true;
        }

        private bool UnixEol(int code) => (char) code == '\n';
        private bool OsxEol(int code) => Eol(code, '\n', '\r');
        private bool WinEol(int code) => Eol(code, '\r', '\n');

        private bool Eol(int code, char first, char second)
        {
            if ((char) code != first) return false;
            var next = _source.Read();
            if (next != -1 && (char) next == second) return true;
            if (next != -1)
                _source.Buffer(next);
            return false;
        }
    }
}
