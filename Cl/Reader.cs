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

            if (Boolean(out var boolean)) return boolean;
            if (String(out var str)) return str;
            if (Fixnum(out var fixnum)) return fixnum;
            throw new InvalidOperationException("Read illegal state");
        }

        public bool Pair(out ClPair cell)
        {
            cell = null;
            if (!_source.SkipMatched("(")) return false;
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(")"))
            {
                cell = Nil.Given;
                return true;
            }
            // mutually recursive
            return false;
        }

        public bool Fixnum(out ClFixnum atom)
        {
            atom = null;
            var sign = _source.SkipMatched("-") ? '-' : '+';
            string loop(string acc)
            {
                if (_source.Eof()) return acc;
                if (!char.IsDigit((char) _source.Peek())) return acc;
                return loop($"{acc}{(char) _source.Read()}");
            }
            if (int.TryParse(loop($"{sign}"), out var num))
            {
                atom = new ClFixnum(num);
                return true;
            }
            return false;
        }

        public bool String(out ClString atom)
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

        public bool Boolean(out ClBool atom)
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
