using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms;

public class Let(ClObj cdr) : Tagged(ClSymbol.Let, cdr)
{
    public override IContext Reduce(IContext ctx)
    {
        var tail = BuiltIn.Tail(Cdr);
        if (tail == Nil || BuiltIn.Cdr(tail) != Nil)
        {
            throw new SyntaxError("Invalid body of the let special form");
        }
        var beginBody = VariableDefinitionExpressions().Append(BuiltIn.Car(tail)).ListOf();
        var begin = new ClCell(ClSymbol.Begin, beginBody);
        var lambda = BuiltIn.ListOf(ClSymbol.Lambda, Nil, begin);
        return BuiltIn.ListOf(lambda).Reduce(ctx);
    }

    private IEnumerable<ClObj> VariableDefinitionExpressions()
    {
        var expressions = BuiltIn.Head(Cdr).Cast<ClCell>();
        var bindings = BuiltIn.Seq(expressions)
            .Select(static expression => {
                if (expression is not ClCell variableDefinition)
                {
                    throw new SyntaxError("Invalid bindings format");
                }
                if (variableDefinition == Nil)
                {
                    throw new SyntaxError($"Variable definition expression cannot be {nameof(Nil)}");
                }
                if (BuiltIn.Cddr(variableDefinition) != Nil)
                {
                    throw new SyntaxError("Variable definition expression should have format (var val)");
                }
                return variableDefinition;
            });
        return bindings
            .Select(x => BuiltIn.ListOf(ClSymbol.Define, BuiltIn.First(x), BuiltIn.Second(x)));
    }
}
