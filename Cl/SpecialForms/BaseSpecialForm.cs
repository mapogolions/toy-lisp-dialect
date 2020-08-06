using System;
using Cl.Contracts;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BaseSpecialForm : ClCell
    {
        internal BaseSpecialForm(ClSymbol tag, IClObj cdr) : base(tag, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            if (Car == ClSymbol.Quote) return ctx.FromResult(Cdr);
            if (Car == ClSymbol.Define) return new DefineSpecialForm(Cdr).Reduce(ctx);
            if (Car == ClSymbol.Set) return new SetSpecialForm(Cdr).Reduce(ctx);
            if (Car == ClSymbol.And) return new AndSpecialForm(Cdr).Reduce(ctx);
            if (Car == ClSymbol.Or) return new OrSpecialForm(Cdr).Reduce(ctx);
            if (Car == ClSymbol.Begin) return new BeginSpecialForm(Cdr).Reduce(ctx);
            if (Car == ClSymbol.If) return new IfSpecialForm(Cdr).Reduce(ctx);
            throw new InvalidOperationException("Invalid special form");
        }
    }
}
