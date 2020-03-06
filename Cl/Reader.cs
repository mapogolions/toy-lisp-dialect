using System.Linq;
using System.Collections.Generic;
using System;
using Cl.Input;
using Cl.Types;
using Cl.Extensions;
using static Cl.Extensions.FpUniverse;
using Cl.Constants;
using System.Globalization;
using Cl.Abs;

namespace Cl
{
    public class Reader : IDisposable
    {
        private readonly IFilteredSource _source;
        private readonly ISymbolsTable _symbolsTable;

        public Reader(IFilteredSource source, ISymbolsTable symbolsTable)
        {
            _source = source;
            _symbolsTable = symbolsTable;
        }

        public Reader(IFilteredSource source) : this(source, new DefaultSymbolsTable())
        {
        }

        public IClObj Read()
        {
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(";")) Ignore(_source.SkipLine());
            var readers = new List<Func<IClObj>> { ReadChar, ReadBool, ReadString, ReadFloat, ReadFixnum, ReadPair };
            foreach (var reader in readers)
            {
                var obj = reader.Invoke();
                if (obj != null) return obj;
            }
            throw new InvalidOperationException(Errors.ReadIllegalState);
        }

        public ClPair ReadPair()
        {
            if (!_source.SkipMatched("(")) return default;
            Ignore(_source.SkipWhitespaces());
            if (_source.SkipMatched(")")) return Nil.Given;
            var car = Read();
            if (!_source.SkipWhitespaces())
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClPair)));
            var cdr = Read();
            if (!_source.SkipMatched(")"))
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClPair)));
            return new ClPair(car, cdr);
        }

        public ClFloat ReadFloat()
        {
            if (!TryReadNumbersInRow(out var significand)) return default;
            if (!_source.SkipMatched("."))
            {
                significand.Reverse().ForEach(ch => _source.Buffer(ch));
                return default;
            }
            if (!TryReadNumbersInRow(out var floating))
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClFloat)));
            var number = double.Parse($"{significand}.{floating}", NumberStyles.Float, CultureInfo.InvariantCulture);
            return new ClFloat(number);
        }

        public ClFixnum ReadFixnum()
        {
            if (!TryReadNumbersInRow(out var nums)) return default;
            if (!int.TryParse(nums, out var integer)) return default;
            return new ClFixnum(integer);
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

        public ClString ReadString()
        {
            if (!_source.SkipMatched("\"")) return default;
            string loop(string acc)
            {
                if (_source.Eof())
                    throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClString)));
                var ch = (char) _source.Read();
                if (ch == '"') return acc;
                return loop($"{acc}{ch}");
            }
            return new ClString(loop(string.Empty));
        }

        public ClBool ReadBool()
        {
            if (!_source.SkipMatched("#")) return default;
            if (_source.SkipMatched("t")) return ClBool.True;
            if (_source.SkipMatched("f")) return ClBool.False;
            throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClBool)));
        }

        public ClChar ReadChar()
        {
            if (!_source.SkipMatched("#\\")) return default;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!_source.SkipMatched(word)) continue;
                return new ClChar(ch);
            }
            if (_source.Eof()) throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClChar)));
            return new ClChar((char) _source.Read());
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
