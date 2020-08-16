using System;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class CondSpecialForm : TaggedSpecialForm
    {
        internal CondSpecialForm(IClObj cdr) : base(ClSymbol.Cond, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            static IClObj Transform(IClObj clauses)
            {
                if (clauses == Nil.Given) return ClBool.False;
                var clause = BuiltIn.First(clauses).CastOrThrow<ClCell>(Errors.BuiltIn.ClauseMustBeCell);
                if (clause.Car.Equals(ClSymbol.Else))
                {
                    return BuiltIn.Tail(clauses) == Nil.Given
                        ? new ClCell(ClSymbol.Begin, clause.Cdr)
                        : throw new InvalidOperationException(Errors.BuiltIn.ElseClauseMustBeLast);
                }
                return BuiltIn.ListOf(ClSymbol.If,
                    clause.Car,
                    new ClCell(ClSymbol.Begin, clause.Cdr),
                    Transform(BuiltIn.Tail(clauses)));
            }
            return Transform(Cdr).Reduce(ctx);
        }
    }
}
