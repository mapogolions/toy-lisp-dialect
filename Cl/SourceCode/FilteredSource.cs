using System;
namespace Cl.SourceCode
{
    public class FilteredSource : PassThroughFilteredSource
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
            foreach (var ch in pattern)
            {
                if (_source.Eof())
                    throw new InvalidOperationException($"Unexpected character {ch}");
                var code = _source.Read();
                if (ch != (char) code)
                {
                    _source.Buffer(code);
                    throw new InvalidOperationException($"Unexpected character {ch}");
                }
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
