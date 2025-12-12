using Cl.Types;

namespace Cl.SpecialForms;

public class Defun(ClObj cdr) : Tagged(ClSymbol.Defun, cdr)
{
    public override IContext Reduce(IContext ctx)
    {
        var funcName = BuiltIn.Head(Cdr);
        var lambda = new ClCell(ClSymbol.Lambda, BuiltIn.Tail(Cdr));
        return BuiltIn.ListOf(ClSymbol.Define, funcName, lambda).Reduce(ctx);
    }
}
