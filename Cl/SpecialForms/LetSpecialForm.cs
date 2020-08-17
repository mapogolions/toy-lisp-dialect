using System;
using System.Linq;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    /*
        (let ((x 10)) x)
        ~>
        ((lambda (x y)
            (list x y)) 10 11)
    */
    internal class LetSpecialForm : TaggedSpecialForm
    {
        internal LetSpecialForm(IClObj cdr) : base(ClSymbol.Let, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            if (BuiltIn.Cddr(Cdr) != Nil.Given)
                throw new InvalidOperationException(Errors.Eval.InvalidLetBody);
            var pairs = BuiltIn.Head(Cdr).Cast<ClCell>();
            var parameters = BuiltIn.ListOf(BuiltIn.Seq(pairs).Select(pair => BuiltIn.First(pair)));
            var args = BuiltIn.ListOf(BuiltIn.Seq(pairs).Select(pair => BuiltIn.Second(pair)));
            var body = BuiltIn.Tail(Cdr);
            var lambda = new ClCell(ClSymbol.Lambda, new ClCell(parameters, body));
            return new ClCell(lambda, args).Reduce(ctx);
        }
    }
}
