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

        public Reader(IFilteredSource source)
        {
            _source = source;
        }

        public Reader(string source) : this(new FilteredSource(source))
        {
        }

        public IClObj Read()
        {
            var ast = ReadMutuallyRec();
            _source.SkipWhitespacesAndComments();
            if (_source.Eof()) return ast;
            throw new InvalidOperationException(Errors.ReadIllegalState);
        }

        private IClObj ReadMutuallyRec()
        {
            _source.SkipWhitespacesAndComments();
            if (ReadLiteral(ReadChar, out var ast)) return ast;
            if (ReadLiteral(ReadBool, out ast)) return ast;
            if (ReadLiteral(ReadString, out ast)) return ast;
            if (ReadLiteral(ReadFloat, out ast)) return ast;
            if (ReadLiteral(ReadFixnum, out ast)) return ast;
            if (ReadLiteral(ReadPair, out ast)) return ast;
            throw new InvalidOperationException(Errors.ReadIllegalState);
        }

        public bool ReadLiteral(Func<IClObj> fn, out IClObj ast)
        {
            ast = fn();
            return ast != null;
        }

        public ClSymbol ReadSymbol()
        {
            if (_source.Eof()) return default;
            var ch = (char) _source.Peek();
            if (!char.IsLetter(ch)) return default;
            string loop(string acc)
            {
                if (_source.Eof()) return acc;
                if (!char.IsLetterOrDigit((char) _source.Peek())) return acc;
                return loop($"{acc}{(char) _source.Read()}");
            }
            return new ClSymbol(loop($"{(char) _source.Read()}"));
        }

        public ClPair ReadPair()
        {
            if (!_source.SkipMatched("(")) return default;
            return ReadListOf();
        }

        private ClPair ReadListOf()
        {
            Ignore(_source.SkipWhitespacesAndComments());
            if (_source.SkipMatched(")")) return Nil.Given;
            var car = ReadMutuallyRec();
            var hasDelimiter = _source.SkipWhitespacesAndComments();
            if (_source.SkipMatched(")")) return new ClPair(car, Nil.Given);
            if (!hasDelimiter)
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClPair)));
            return new ClPair(car, ReadListOf());
        }

        public ClFloat ReadFloat()
        {
            if (!TryReadNumbersInRow(out var significand)) return default;
            if (!_source.SkipMatched("."))
            {
                significand.Reverse().ForEach(ch => _source.Buffer(ch));
                return default;
            }
            if (!TryReadNumbersInRow(out var mantissa))
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClFloat)));
            var number = double.Parse($"{significand}.{mantissa}", NumberStyles.Float, CultureInfo.InvariantCulture);
            return new ClFloat(number);
        }

        public ClFixnum ReadFixnum()
        {
            if (!TryReadNumbersInRow(out var nums)) return default;
            if (!int.TryParse(nums, out var integer)) return default;
            return new ClFixnum(integer);
        }

        private bool TryReadNumbersInRow(out string nums)
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
