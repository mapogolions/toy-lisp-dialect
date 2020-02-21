using System;
using Cl.SourceCode;
using Cl.Types;

namespace Cl
{
    public class Reader : IDisposable
    {
        private readonly IFilteredSource _source;

        public Reader(IFilteredSource source)
        {
            _source = source;
        }

        public IClObj Read()
        {
            // _source.SkipWhitespaces();
            if (_source.Eof())
                throw new InvalidOperationException("Read illegal state");
            var code = _source.Peek();
            var ch = (char) code;
            if (ch == '#') return ReadBoolOrChar();
            if (ch == '(') return ReadPair();
            if (ch == '\\') return null;
            if (char.IsDigit(ch)) return ReadFixnum();
            return null;
        }

        public IClObj ReadFixnum()
        {
            return null;
        }

        public IClObj ReadPair()
        {
            return null;
        }

        public IClObj ReadBoolOrChar()
        {
            return null;
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
