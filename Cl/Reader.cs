using System.Linq;
using System.Collections.Generic;
using System;
using Cl.Input;
using Cl.Types;
using Cl.Extensions;
using static Cl.Extensions.FpUniverse;
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

        public IEnumerable<IClObj> Read()
        {
            var items = new List<IClObj>();
            while (!_source.Eof())
            {
                items.Add(ReadExpression());
                _source.SkipWhitespacesAndComments();
            }
            return items;
        }

        public IClObj ReadExpression()
        {
            _source.SkipWhitespacesAndComments();
            if (TryReadExpression(ReadChar, out var ast)) return ast;
            if (TryReadExpression(ReadBool, out ast)) return ast;
            if (TryReadExpression(ReadString, out ast)) return ast;
            if (TryReadExpression(ReadFloat, out ast)) return ast;
            if (TryReadExpression(ReadFixnum, out ast)) return ast;
            if (TryReadExpression(ReadCell, out ast)) return ast;
            if (TryReadExpression(ReadSymbol, out ast)) return ast;
            throw new InvalidOperationException(Errors.Reader.ReadIllegalState);
        }

        public bool TryReadExpression(Func<IClObj> fn, out IClObj ast)
        {
            ast = fn();
            return ast != null;
        }

        public ClSymbol ReadSymbol()
        {
            if (_source.Eof()) return null;
            var ch = (char) _source.Peek();
            if (!char.IsLetter(ch)) return null;
            string loop(string acc)
            {
                if (_source.Eof()) return acc;
                var ch = (char) _source.Peek();
                if (!char.IsLetterOrDigit(ch) && ch != '-' && ch != '?') return acc;
                return loop($"{acc}{(char) _source.Read()}");
            }
            return new ClSymbol(loop($"{(char) _source.Read()}"));
        }

        public ClCell ReadCell()
        {
            if(TryReadNilOrNull(out var cell)) return cell;
            var car = ReadExpression();
            var wasDelimiter = _source.SkipWhitespacesAndComments();
            if (!_source.SkipMatched(".")) return new ClCell(car, ReadList(wasDelimiter));
            var cdr = ReadExpression();
            Ignore(_source.SkipWhitespacesAndComments());
            if (!_source.SkipMatched(")"))
                throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClCell)));
            return new ClCell(car, cdr);
        }

        private bool TryReadNilOrNull(out ClCell cell)
        {
            cell = default;
            if (!_source.SkipMatched("(")) return true;
            Ignore(_source.SkipWhitespacesAndComments());
            if (_source.SkipMatched(")"))
            {
                cell = Nil.Given;
                return true;
            }
            return false;
        }

        private ClCell ReadList(bool wasDelimiter)
        {
            if (_source.SkipMatched(")")) return Nil.Given;
            if (!wasDelimiter)
                throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClCell)));
            var car = ReadExpression();
            wasDelimiter = _source.SkipWhitespacesAndComments();
            if (_source.SkipMatched(")")) return new ClCell(car, Nil.Given);
            return new ClCell(car, ReadList(wasDelimiter));
        }

        public ClFloat ReadFloat()
        {
            if (!TryReadNumbersInRow(out var significand)) return null;
            if (!_source.SkipMatched("."))
            {
                significand.Reverse().ForEach(ch => _source.Buffer(ch));
                return null;
            }
            if (!TryReadNumbersInRow(out var mantissa))
                throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClFloat)));
            var number = double.Parse($"{significand}.{mantissa}", NumberStyles.Float,
                CultureInfo.InvariantCulture);
            return new ClFloat(number);
        }

        public ClFixnum ReadFixnum()
        {
            if (!TryReadNumbersInRow(out var nums)) return null;
            if (!int.TryParse(nums, out var integer)) return null;
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
            if (!_source.SkipMatched("\"")) return null;
            string loop(string acc)
            {
                if (_source.Eof())
                    throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClString)));
                var ch = (char) _source.Read();
                if (ch == '"') return acc;
                return loop($"{acc}{ch}");
            }
            return new ClString(loop(string.Empty));
        }

        public ClBool ReadBool()
        {
            if (!_source.SkipMatched("#")) return null;
            if (_source.SkipMatched("t")) return ClBool.True;
            if (_source.SkipMatched("f")) return ClBool.False;
            throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClBool)));
        }

        public ClChar ReadChar()
        {
            if (!_source.SkipMatched("#\\")) return null;
            foreach (var (word, ch) in SpecialChars)
            {
                if (!_source.SkipMatched(word)) continue;
                return new ClChar(ch);
            }
            if (_source.Eof())
                throw new InvalidOperationException(Errors.Reader.UnknownLiteral(nameof(ClChar)));
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
