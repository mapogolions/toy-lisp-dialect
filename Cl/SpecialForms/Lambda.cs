using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms;

public class Lambda(ClObj cdr) : Tagged(ClSymbol.Lambda, cdr)
{
    public override IContext Reduce(IContext context)
    {
        if (BuiltIn.Cddr(Cdr) != Nil)
        {
            throw new SyntaxError("Invalid function body format");
        }
        if (BuiltIn.First(Cdr) is not ClCell parameters)
        {
            throw new SyntaxError("Invalid function parameters format");
        }
        if (BuiltIn.Seq(parameters).Any(x => x is not ClSymbol))
        {
            throw new SyntaxError($"Binding statement should have {nameof(ClSymbol)} on the left-hand-side");
        }
        var body = BuiltIn.Second(Cdr);
        return context.FromValue(new ClFn(parameters, body, new Env(context.Env)));
    }
}
