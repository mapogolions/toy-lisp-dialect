using System.Collections.Generic;
using System;
using Cl.Input;
using Cl.Types;
using static Cl.Extensions.FpUniverse;

namespace Cl
{
    public class Reader : IDisposable
    {
        private readonly IFilteredSource _source;

        public IDictionary<string, char> SpecialChars = new Dictionary<string, char>
        {
            ["newline"] = '\n',
            ["tab"] = '\t',
            ["space"] = ' '
        };

        public Reader(IFilteredSource source)
        {
            _source = source;
        }

        public IClObj Read()
        {
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(";")) Ignore(_source.SkipLine());
            if (ReadChar(out var ch)) return ch;
            if (ReadBool(out var boolean)) return boolean;
            if (ReadString(out var str)) return str;
            if (ReadFixnum(out var fixnum)) return fixnum;
            if (ReadPair(out var cell)) return cell;
            throw new InvalidOperationException("Read illegal state");
        }

        public bool ReadPair(out ClPair cell)
        {
            cell = default;
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

        public bool ReadFixnum(out ClFixnum atom)
        {
            atom = default;
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

        public bool ReadString(out ClString atom)
        {
            atom = default;
            if (!_source.SkipMatched("\"")) return false;
            string loop(string acc)
            {
                if (_source.Eof()) throw new InvalidOperationException("Unknown string literal");
                var ch = (char) _source.Read();
                if (ch == '"') return acc;
                return loop($"{acc}{ch}");
            }
            atom = new ClString(loop(string.Empty));
            return true;
        }

        public bool ReadBool(out ClBool atom)
        {
            atom = default;
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

        public bool ReadChar(out ClChar atom)
        {
            atom = default;
            if (!_source.SkipMatched("#")) return false;
            if (!_source.SkipMatched("\\")) return false;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!_source.SkipMatched(word)) continue;
                atom = new ClChar(ch);
                return true;
            }
            if (_source.Eof()) throw new InvalidOperationException("Unknown char literal");
            atom = new ClChar((char) _source.Read());
            return true;
        }

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
