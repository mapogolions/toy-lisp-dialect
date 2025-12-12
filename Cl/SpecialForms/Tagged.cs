using Cl.Errors;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms;

public class Tagged(ClSymbol tag, ClObj cdr) : ClCell(tag, cdr)
{
    public ClSymbol Tag { get; } = tag;

    public override IContext Reduce(IContext ctx)
    {
        if (Tag.Equals(ClSymbol.Quote)) return ctx.FromValue(Cdr);
        if (Tag.Equals(ClSymbol.Define)) return new Define(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Set)) return new SetSpecialForm(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.And)) return new And(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Or)) return new Or(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Begin)) return new Begin(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.If)) return new If(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Cond)) return new Cond(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Let)) return new Let(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Lambda)) return new Lambda(Cdr).Reduce(ctx);
        if (Tag.Equals(ClSymbol.Defun)) return new Defun(Cdr).Reduce(ctx);
        var obj = ctx.Env.Lookup(Tag);
        if (obj is ClCallable callable) return new Apply(Tag, callable, Cdr).Reduce(ctx);
        throw new SyntaxError($"{obj.GetType().Name} is neither callable nor special from");
    }
}
