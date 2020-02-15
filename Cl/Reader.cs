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

        public void SkipEol()
        {
            if (_source.Eof()) return;
            var code = _source.Read();
            if (OsxEol(code) || WinEol(code) || UnixEol(code)) return;
            _source.Buffer(code);
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

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
