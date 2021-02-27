using System;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class TaggedSpecialForm : ClCell
    {
        internal TaggedSpecialForm(ClSymbol tag, ClObj cdr) : base(tag, cdr) { }

        public ClSymbol Tag => Car as ClSymbol;

        public override IContext Reduce(IContext ctx)
        {
            if (Tag.Equals(ClSymbol.Quote)) return ctx.FromValue(Cdr);
            if (Tag.Equals(ClSymbol.Define)) return new DefineSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Set)) return new SetSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.And)) return new AndSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Or)) return new OrSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Begin)) return new BeginSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.If)) return new IfSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Cond)) return new CondSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Let)) return new LetSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Lambda)) return new LambdaSpecialForm(Cdr).Reduce(ctx);
            if (Tag.Equals(ClSymbol.Defun)) return new DefunSpecialForm(Cdr).Reduce(ctx);
            var obj = ctx.Env.Lookup(Tag);
            if (obj is ClCallable callable) return new ApplySpecialForm(callable, Cdr).Reduce(ctx);
            throw new InvalidOperationException(Errors.Eval.InvalidFunctionCall);
        }
    }
}
