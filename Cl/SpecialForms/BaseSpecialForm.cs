using System;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BaseSpecialForm : ClCell
    {
        internal BaseSpecialForm(ClSymbol tag, IClObj cdr) : base(tag, cdr) { }

        public ClSymbol Tag => Car as ClSymbol;

        public override IContext Reduce(IContext ctx)
        {
            if (Tag == ClSymbol.Quote) return ctx.FromResult(Cdr);
            if (Tag == ClSymbol.Define) return new DefineSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.Set) return new SetSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.And) return new AndSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.Or) return new OrSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.Begin) return new BeginSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.If) return new IfSpecialForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.Cond) return ConvertToBeginForm(Cdr).Reduce(ctx);
            if (Tag == ClSymbol.Lambda) return new LambdaSpecialForm(Cdr).Reduce(ctx);
            return new ApplySpecialForm(Tag, Cdr).Reduce(ctx);
        }

        private IClObj ConvertToBeginForm(IClObj clauses)
        {
            if (clauses == Nil.Given) return ClBool.False;
            var clause = BuiltIn.First(clauses).CastOrThrow<ClCell>(Errors.BuiltIn.ClauseMustBeCell);
            if (clause.Car == ClSymbol.Else)
            {
                return BuiltIn.Tail(clauses) == Nil.Given
                    ? new ClCell(ClSymbol.Begin, clause.Cdr)
                    : throw new InvalidOperationException(Errors.BuiltIn.ElseClauseMustBeLast);
            }
            return BuiltIn.ListOf(ClSymbol.If,
                clause.Car,
                new ClCell(ClSymbol.Begin, clause.Cdr),
                ConvertToBeginForm(BuiltIn.Tail(clauses)));
        }
    }
}
