using Cl;
using Cl.Errors;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class CondSpecialForm : TaggedSpecialForm
    {
        public CondSpecialForm(ClObj cdr) : base(ClSymbol.Cond, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            static ClObj Transform(ClObj clauses)
            {
                if (clauses == ClCell.Nil) return ClBool.False;
                var clause = BuiltIn.First(clauses) as ClCell;
                if (clause is null)
                {
                    throw new SyntaxError("Clause must be a cell");
                }
                if (clause.Car.Equals(ClSymbol.Else))
                {
                    return BuiltIn.Tail(clauses) == ClCell.Nil
                        ? new ClCell(ClSymbol.Begin, clause.Cdr)
                        : throw new SyntaxError("Else clause must be last condition");
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
