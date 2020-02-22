using System;
using Cl.SourceCode;
using Cl.Types;
using static Cl.Extensions.FpUniverse;

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
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(";"))
                Ignore(_source.SkipLine());
            if (_source.Eof())
                throw new InvalidOperationException("Read illegal state");
            throw new InvalidOperationException("Read illegal state");
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

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
