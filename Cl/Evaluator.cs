using System;
using Cl.Abs;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
    public class Evaluator
    {
        private readonly IEnv _env;

        public Evaluator(IEnv env)
        {
            _env = env;
        }

        public IClObj Eval(IClObj expr)
        {
            if (expr.IsSelfEvaluating()) return expr;
            if (expr.IsVariable()) return _env.Lookup(expr.Cast<ClSymbol>());
            if (expr.IsAssignment()) return EvalAssigment(expr);
            if (expr.IsDefinition()) return EvalDefinition(expr);
            if (expr.IsQuoted()) return BuiltIn.Tail(expr);
            if (expr.IsIfThenElse()) return EvalIfThenElse(expr);
            if (expr.IsAnd()) return EvalAnd(expr);
            if (expr.IsOr()) return EvalOr(expr);
            if (expr.IsBegin()) return EvalBegin(expr);
            if (expr.IsLambda()) return EvalLambda(expr);
            if (expr.IsCond()) return Eval(TransformCond(expr));
            throw new InvalidOperationException("Evaluation error");
        }

        private IClObj TransformCond(IClObj expr)
        {
            var clauses = BuiltIn.Tail(expr);
            if (clauses == Nil.Given) return ClBool.False;
            var clause = BuiltIn.First(clauses).Cast<ClCell>();
            if (clause.Car == ClSymbol.Else)
            {
                return BuiltIn.Tail(clauses) == Nil.Given
                    ? new ClCell(ClSymbol.Begin, clause.Cdr)
                    : throw new InvalidOperationException("Else clause is not last condition");
            }
            return BuiltIn.ListOf(ClSymbol.IfThenElse, clause.Car, new ClCell(ClSymbol.Begin, clause.Cdr),
                TransformCond(BuiltIn.Tail(clauses)));
        }

        public IClObj EvalBegin(IClObj expr)
        {
            var tail = BuiltIn.Tail(expr);
            IClObj result = Nil.Given;
            while (tail != Nil.Given)
            {
                result = Eval(BuiltIn.Head(tail));
                tail = BuiltIn.Tail(tail);
            }
            return result;
        }

        public IClObj EvalOr(IClObj expr)
        {
            // (or :expr-1 :expr-2 ... :expr-n) -> (and . (:expr-1 . (:expr-2 . (... (expr-n . nil))))
            var tail = BuiltIn.Tail(expr);
            while (tail != Nil.Given)
            {
                var result = Eval(BuiltIn.Head(tail));
                if (result != Nil.Given && result != ClBool.False) return ClBool.True;
                tail = BuiltIn.Tail(tail);
            }
            return ClBool.False;
        }

        public IClObj EvalAnd(IClObj expr)
        {
            // (and :expr-1 :expr-2 ... :expr-n) -> (and . (:expr-1 . (:expr-2 . (... (expr-n . nil))))
            var tail = BuiltIn.Tail(expr);
            while (tail != Nil.Given)
            {
                var result = Eval(BuiltIn.Head(tail));
                if (result == Nil.Given || result == ClBool.False) return ClBool.False;
                tail = BuiltIn.Tail(tail);
            }
            return ClBool.True;
        }

        // TODO: add tests
        public IClObj EvalLambda(IClObj expr)
        {
            // (lambda (a b c) :body-expr)
            var args = BuiltIn.Cadr(expr);
            var body = BuiltIn.Cddr(expr);
            return new ClProc(args.Cast<ClCell>(), body);
        }

        // TODO: test case - evaluation must be lazy
        // (if :expr :expr :expr) -> (if . (:expr . (:expr . (:expr . nil)))) else branch is provided
        // (if :expr :expr) -> (if . (:expr . (:expr . nil))) without else branch
        public IClObj EvalIfThenElse(IClObj expr)
        {
            var result = Eval(BuiltIn.Second(expr));
            if (result != Nil.Given && result != ClBool.False)  return Eval(BuiltIn.Third(expr));
            var elseBranch = BuiltIn.Cdddr(expr);
            return elseBranch ==  Nil.Given ? Nil.Given : Eval(BuiltIn.First(elseBranch));
        }

        public IClObj EvalDefinition(IClObj expr)
        {
            // (define a 10) -> (define . (a . (10 . nil)))
            // (define a b) -> (define . (a . (b . nil)))
            // (define a a) -> (define . (a . (a . nil))) throw exception
            // TODO: (define keyword 10) i.e. (define lambda 10) Need reject
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var obj = Eval(BuiltIn.Third(expr));
            _env.Bind(identifier, obj);
            return Nil.Given;
        }

        public IClObj EvalAssigment(IClObj expr)
        {
            // (set! a 10) -> (set! . (a . (10 . nil)))
            // (set! a b) -> (set! . (a . (b . nil)))
            // (set! a a) -> (set! . (a . (a . nil))) reassign
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var obj = Eval(BuiltIn.Third(expr));
            _env.Assign(identifier, obj);
            return Nil.Given;
        }
    }
}
