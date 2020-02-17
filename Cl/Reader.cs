using System;

namespace Cl
{
    public class Reader : IDisposable
    {
        private readonly ISource _source;

        public Reader(ISource source)
        {
            _source = source;
        }

        public bool IsDelimiter(char ch) =>
            char.IsWhiteSpace(ch) || ch == '(' || ch == ')' || ch == '"' || ch == ';';

        public char PeekDelimiter()
        {
            EnsureSourceIsNotDrained();
            var ch = (char) _source.Peek();
            if (!IsDelimiter(ch))
                throw new InvalidOperationException("Character not followed by delimiter");
            return ch;
        }

        public bool Skip(string source)
        {
            foreach (var ch in source)
            {
                EnsureSourceIsNotDrained();
                var code = _source.Read();
                if ((char) code != ch)
                    throw new InvalidOperationException($"Unexpected character {ch}");
            }
            return true;
        }

        public void SkipWhiteSpaces()
        {
            if (_source.Eof()) return;
            var ch = (char) _source.Read();
            if (ch == ';')
            {
                SkipLine();
                SkipWhiteSpaces();
                return;
            }
            if (char.IsWhiteSpace(ch))
            {
                SkipWhiteSpaces();
                return;
            }
            _source.Buffer(ch);
        }

        public bool SkipLine()
        {
            if (_source.Eof()) return false;
            var code = _source.Read();
            if ((char) code != '\r' || (char) code != '\n')
                return SkipLine();
            _source.Buffer(code);
            if (SkipEol()) return true;
            return SkipLine();
        }

        public bool SkipEol()
        {
            if (_source.Eof()) return false;
            var code = _source.Read();
            if (OsxEol(code) || WinEol(code) || UnixEol(code))
                return true;
            _source.Buffer(code);
            return false;
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

        private void EnsureSourceIsNotDrained()
        {
            if (_source.Eof())
                throw new InvalidOperationException("Source is drained");
        }

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
