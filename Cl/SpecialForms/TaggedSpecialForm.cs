using System;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class TaggedSpecialForm : ClCell
    {
        internal TaggedSpecialForm(ClSymbol tag, IClObj cdr) : base(tag, cdr) { }

        public ClSymbol Tag => Car as ClSymbol;

        public override IContext Reduce(IContext ctx)
        {
            if (Tag.Equals(ClSymbol.Quote)) return ctx.FromResult(Cdr);
            if (Tag.Equals(ClSymbol.Define)) return new DefineSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Set)) return new SetSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.And)) return new AndSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Or)) return new OrSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Begin)) return new BeginSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.If)) return new IfSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Cond)) return ConvertToBeginForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Lambda)) return new LambdaSpecialForm(Cdr).Reduce(ctx);
            var fn = ctx.Env.Lookup(Tag).CastOrThrow<ClFn>($"Function with {Tag} name doesn't exist");
            return new ApplySpecialForm(fn, Cdr).Reduce(ctx);
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
