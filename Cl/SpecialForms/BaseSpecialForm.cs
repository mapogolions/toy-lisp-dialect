using System;
using Cl.Contracts;
using Cl.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    internal class BaseSpecialForm : ClCell
    {
        internal BaseSpecialForm(ClSymbol tag, IClObj cdr) : base(tag, cdr) { }

        public override IContext Reduce(IContext context)
        {
            if (Car == ClSymbol.Quote) return context.FromResult(Cdr);
            if (Car == ClSymbol.Define) return new DefineSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.Set) return new SetSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.And) return new AndSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.Or) return new OrSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.Begin) return new BeginSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.If) return new IfSpecialForm(Cdr).Reduce(context);
            if (Car == ClSymbol.Cond) return ConvertToBeginForm(Cdr).Reduce(context);
            if (Car == ClSymbol.Lambda) return new LambdaSpecialForm(Cdr).Reduce(context);
            // var fnName = Car.CastOrThrow<ClSymbol>("Invalid function call");
            // var fn = context.Env.Lookup(fnName).CastOrThrow<ClFn>("Invlid function call");
            throw new InvalidOperationException("Invalid special form");
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
