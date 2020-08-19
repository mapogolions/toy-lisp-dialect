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
            var body = BuiltIn.Tail(Cdr);
            if (body == ClCell.Nil || BuiltIn.Cdr(body) != ClCell.Nil)
                throw new InvalidOperationException(Errors.Eval.InvalidLetBodyFormat);
            var pairs = BuiltIn.Head(Cdr).Cast<ClCell>();
            // Todo: refactoring
            var cells = BuiltIn.ListOf(BuiltIn.Seq(pairs).Select(obj => {
                var cell = obj.CastOrThrow<ClCell>(Errors.Eval.InvalidBindingsFormat);
                if (cell == ClCell.Nil)
                    throw new InvalidOperationException(Errors.Eval.InvalidBindingsFormat);
                if (BuiltIn.Cdr(cell) == ClCell.Nil || BuiltIn.Cddr(cell) != ClCell.Nil)
                    throw new InvalidOperationException(Errors.Eval.InvalidBindingsFormat);
                return cell;
            }));

            var parameters = BuiltIn.ListOf(BuiltIn.Seq(cells).Select(cell => BuiltIn.First(cell)));
            var args = BuiltIn.ListOf(BuiltIn.Seq(pairs).Select(cell => BuiltIn.Second(cell)));
            var lambda = new ClCell(ClSymbol.Lambda, new ClCell(parameters, body));
            return new ClCell(lambda, args).Reduce(ctx);
        }
    }
}
