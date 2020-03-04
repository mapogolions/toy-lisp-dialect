using System.Linq;
using System.Collections.Generic;
using System;
using Cl.Input;
using Cl.Types;
using Cl.Extensions;
using static Cl.Extensions.FpUniverse;
using Cl.Constants;
using System.Globalization;

namespace Cl
{
    public class Reader : IDisposable
    {
        private readonly IFilteredSource _source;
        private readonly IDictionary<string, IClObj> _symbols;

        public Reader(IFilteredSource source, IDictionary<string, IClObj> symbols)
        {
            _source = source;
            _symbols = symbols;
        }

        public Reader(IFilteredSource source) : this(source, new Dictionary<string, IClObj>())
        {
        }

        public IClObj Read()
        {
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(";")) Ignore(_source.SkipLine());
            if (ReadChar(out var ch)) return ch;
            if (ReadBool(out var boolean)) return boolean;
            if (ReadString(out var str)) return str;
            if (ReadFloatingPoint(out var natural)) return natural;
            if (ReadFixnum(out var integer)) return integer;
            if (ReadPair(out var cell)) return cell;
            throw new InvalidOperationException(Errors.ReadIllegalState);
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
            var car = Read();
            var cdr = Read();
            Ignore(_source.SkipWhitespaces());
            if (!_source.SkipMatched(")"))
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClPair)));
            cell = new ClPair(car, cdr);
            return true;
        }

        public bool ReadFloatingPoint(out ClFloatingPoint atom)
        {
            atom = null;
            if (!TryReadNumbersInRow(out var significand)) return false;
            if (!_source.SkipMatched("."))
            {
                significand.Reverse().ForEach(ch => _source.Buffer(ch));
                return false;
            }
            if (!TryReadNumbersInRow(out var floating))
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClFloatingPoint)));
            var number = double.Parse($"{significand}.{floating}", NumberStyles.Float, CultureInfo.InvariantCulture);
            atom = new ClFloatingPoint(number);
            return true;
        }

        public bool ReadFixnum(out ClFixnum atom)
        {
            atom = default;
            if (!TryReadNumbersInRow(out var nums)) return false;
            if (!int.TryParse(nums, out var integer)) return false;
            atom = new ClFixnum(integer);
            return true;
        }

        public bool TryReadNumbersInRow(out string nums)
        {
            string loop(string acc = "")
            {
                if (_source.Eof()) return acc;
                if (!char.IsDigit((char) _source.Peek())) return acc;
                return loop($"{acc}{(char) _source.Read()}");
            }
            nums = loop();
            return !string.IsNullOrEmpty(nums);
        }

        public bool ReadString(out ClString atom)
        {
            atom = default;
            if (!_source.SkipMatched("\"")) return false;
            string loop(string acc)
            {
                if (_source.Eof())
                    throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClString)));
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
                atom = ClBool.True;
                return true;
            }
            if (_source.SkipMatched("f"))
            {
                atom = ClBool.False;
                return true;
            }
            throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClBool)));
        }

        public bool ReadChar(out ClChar atom)
        {
            atom = default;
            if (!_source.SkipMatched("#\\")) return false;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!_source.SkipMatched(word)) continue;
                atom = new ClChar(ch);
                return true;
            }
            if (_source.Eof()) throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClChar)));
            atom = new ClChar((char) _source.Read());
            return true;
        }

        public IDictionary<string, char> SpecialChars = new Dictionary<string, char>
            {
                ["newline"] = '\n',
                ["tab"] = '\t',
                ["space"] = ' '
            };

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}
