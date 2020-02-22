using System.Text;
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

            if (TryReadBool(out var boolAtom)) return boolAtom;
            if (TryReadString(out var strAtom)) return strAtom;
            throw new InvalidOperationException("Read illegal state");
        }

        public bool TryReadString(out ClString atom)
        {
            atom = null;
            if (!_source.SkipMatched("\"")) return false;
            string loop(string acc)
            {
                if (_source.Eof())
                    throw new InvalidOperationException("Unknown string literal");
                var ch = (char) _source.Read();
                if (ch == '"') return acc;
                return loop($"{acc}{ch}");
            }
            atom = new ClString(loop(string.Empty));
            return true;
        }

        public IClObj ReadFixnum()
        {
            return null;
        }

        public IClObj ReadPair()
        {
            return null;
        }

        public bool TryReadBool(out ClBool atom)
        {
            atom = null;
            if (!_source.SkipMatched("#")) return false;
            if (_source.SkipMatched("t"))
            {
                atom = new ClBool(true);
                return true;
            }
            if (_source.SkipMatched("f"))
            {
                atom = new ClBool(false);
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
