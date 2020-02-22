using System;
using Cl.Input;
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

            if (TryReadBool(out var atom)) return atom;
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

        public bool TryReadBool(out ClBool obj)
        {
            obj = null;
            if (_source.Eof()) return false;
            if (!_source.SkipMatched("#")) return false;
            if (_source.SkipMatched("t"))
            {
                obj = new ClBool(true);
                return true;
            }
            if (_source.SkipMatched("f"))
            {
                obj = new ClBool(false);
                return true;
            }
            throw new InvalidOperationException("Unknown boolean literal");
        }

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
