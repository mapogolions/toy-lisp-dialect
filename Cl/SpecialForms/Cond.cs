using Cl.Errors;
using Cl.Types;

namespace Cl.SpecialForms;

public class Cond(ClObj cdr) : Tagged(ClSymbol.Cond, cdr)
{
    public override IContext Reduce(IContext ctx)
    {
        static ClObj Transform(ClObj clauses)
        {
            if (clauses == Nil) return ClBool.False;
            if (BuiltIn.First(clauses) is not ClCell clause)
            {
                throw new SyntaxError("Clause must be a cell");
            }
            if (clause.Car.Equals(ClSymbol.Else))
            {
                return BuiltIn.Tail(clauses) == Nil
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
