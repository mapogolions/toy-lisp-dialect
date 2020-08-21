using System;
using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class LetSpecialForm : TaggedSpecialForm
    {
        internal LetSpecialForm(IClObj cdr) : base(ClSymbol.Let, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var tail = BuiltIn.Tail(Cdr);
            if (tail == ClCell.Nil || BuiltIn.Cdr(tail) != ClCell.Nil)
                throw new InvalidOperationException(Errors.Eval.InvalidLetBodyFormat);
            var pairs = BuiltIn.Head(Cdr).Cast<ClCell>();
            var bindings = BuiltIn.ListOf(BuiltIn.Seq(pairs).Select(obj => {
                var cell = obj.CastOrThrow<ClCell>(Errors.Eval.InvalidBindingsFormat);
                if (cell == ClCell.Nil)
                    throw new InvalidOperationException(Errors.Eval.InvalidBindingsFormat);
                if (BuiltIn.Cdr(cell) == ClCell.Nil || BuiltIn.Cddr(cell) != ClCell.Nil)
                    throw new InvalidOperationException(Errors.Eval.InvalidBindingsFormat);
                return cell;
            }));
            var definitions = BuiltIn.Seq(bindings)
                .Select(binding => BuiltIn.ListOf(ClSymbol.Define, BuiltIn.First(binding), BuiltIn.Second(binding)));
            var begin = new ClCell(ClSymbol.Begin, BuiltIn.ListOf(definitions.Append(BuiltIn.Car(tail))));
            var lambda = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, begin);
            return BuiltIn.ListOf(lambda).Reduce(ctx);
        }
    }
}
