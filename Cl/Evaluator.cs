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
            if (expr.IsQuoted()) return BuiltIn.Second(expr);
            if (expr.IsIf()) return EvalIf(expr);
            if (expr.IsLambda()) return EvalLambda(expr);
            throw new InvalidOperationException("Evaluation error");
        }

        // TODO: add tests
        public IClObj EvalLambda(IClObj expr)
        {
            // (lambda (a b c) :body-expr)
            var args = BuiltIn.Cadr(expr);
            var body = BuiltIn.Cddr(expr);
            return new ClProc(args.Cast<ClCell>(), body);
        }

        // TODO: add tests
        // (if :expr :expr :expr) -> (if . (:expr . (:expr . (:expr . nil)))) else branch is provided
        // (if :expr :expr) -> (if . (:expr . (:expr . nil))) without else branch
        public IClObj EvalIf(IClObj expr)
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
