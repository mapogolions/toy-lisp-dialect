using Cl.Errors;
using Cl.Extensions;
using Cl.SpecialForms;

namespace Cl.Types
{
    public class ClCell : ClObj
    {
        public static readonly ClCell Nil = new NIL();

        public ClCell(ClObj car, ClObj cdr)
        {
            Car = car;
            Cdr = cdr;
        }

        public virtual ClObj Car { get; }
        public virtual ClObj Cdr { get; }

        public override IContext Reduce(IContext ctx)
        {
            if (Car is ClSymbol tag)
            {
                return new TaggedSpecialForm(tag, Cdr).Reduce(ctx);
            }
            // Case: ((lambda (a b) (+ a b)) 1 2)
            var (value, env) = Car.Reduce(ctx);
            if (value is ClCallable callable)
            {
                return new ApplySpecialForm(ClSymbol.Anonymous, callable, Cdr).Reduce(new Context(env));
            }
            throw new SyntaxError("Invalid function call");
        }

        public override string ToString() => $"({Car} . {Cdr})";

        private class NIL : ClCell
        {
            public NIL() : base(null!, null!) { }

            public override ClObj Car => throw new NotSupportedException();
            public override ClObj Cdr => throw new NotSupportedException();
            public override string ToString() => "nil";
            public override IContext Reduce(IContext ctx) => ctx.FromValue(this);
        }
    }
}
