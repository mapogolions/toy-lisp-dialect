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

        public IClObj Read()
        {
            var ast = ReadMutualRec();
            _source.SkipWhitespacesAndComments();
            if (_source.Eof()) return ast;
            throw new InvalidOperationException(Errors.ReadIllegalState);
        }

        private IClObj ReadMutualRec()
        {
            _source.SkipWhitespacesAndComments();
            if (ReadLiteral(ReadChar, out var obj)) return obj;
            if (ReadLiteral(ReadBool, out obj)) return obj;
            if (ReadLiteral(ReadString, out obj)) return obj;
            if (ReadLiteral(ReadFloat, out obj)) return obj;
            if (ReadLiteral(ReadFixnum, out obj)) return obj;
            if (ReadLiteral(ReadPair, out obj)) return obj;
            throw new InvalidOperationException(Errors.ReadIllegalState);
        }

        public bool ReadLiteral(Func<IClObj> fn, out IClObj obj)
        {
            obj = fn();
            return obj != null;
        }

        public ClPair ReadPair()
        {
            if (!_source.SkipMatched("(")) return default;
            Ignore(_source.SkipWhitespacesAndComments());
            if (_source.SkipMatched(")")) return Nil.Given;
            var car = ReadMutualRec();
            if (!_source.SkipWhitespacesAndComments())
                throw new InvalidOperationException(Errors.UnknownLiteral(nameof(ClPair)));
            var cdr = ReadMutualRec();
            Ignore(_source.SkipWhitespacesAndComments());
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
