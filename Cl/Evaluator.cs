using System;
using System.Linq;
using Cl.Abs;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
    public class Evaluator
    {
        // NLGB:  nested -> local -> global -> builtin
        private IEnv _env;

        public Evaluator(IEnv env)
        {
            // new Evaluator(new Env(builinScope)) - inject builtin
            _env = env;
        }

        public IClObj Eval(IClObj expr)
        {
            if (expr.IsSelfEvaluating()) return expr;
            if (expr.IsVariable()) return _env.Lookup(expr.Cast<ClSymbol>());
            if (expr.IsQuoted()) return BuiltIn.Tail(expr);
            if (expr.IsCond()) return Eval(TransformCond(expr));
            if (TryEvalAssigment(expr, out var obj)) return obj;
            if (TryEvalDefinition(expr, out obj)) return obj;
            if (TryEvalIfThenElse(expr, out obj)) return obj;
            if (TryEvalAnd(expr, out obj)) return obj;
            if (TryEvalOr(expr, out obj)) return obj;
            if (TryEvalBegin(expr, out obj)) return obj;
            if (TryEvalLambda(expr, out obj)) return obj;
            if (TryEvalApplication(expr, out obj)) return obj;
            throw new InvalidOperationException(Errors.Eval.EvaluationError);
        }

        public bool TryEvalApplication(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            var cell = expr.TypeOf<ClCell>();
            if (cell is null) return false;
            /*
                ((lambda (x) x) 10) -> ({ClProc} 10)
                (identity 10) -> ({ClSymobl} 10)
                ((if true (lambda () "yes") (lambda () "no")) 10) -> ({ClProc} 10)
                ((lambda (x) 10)) -> return 10
             */
            var procedure = Eval(cell.Car).Cast<ClProc>(Errors.Eval.UnknownProcedureType);
            var args = cell.Cdr.Cast<ClCell>();
            // eager evaluation strategy
            var values = BuiltIn.Seq(args).Select(it => Eval(it));
            var parent = _env;
            _env = _env.Extend(procedure.Varargs, BuiltIn.ListOf(values));
            obj = Eval(procedure.Body);
            _env = parent;
            return true;
        }

        public bool TryEvalBegin(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            if (!expr.IsBegin()) return false;
            var tail = BuiltIn.Tail(expr);
            while (tail != Nil.Given)
            {
                obj = Eval(BuiltIn.Head(tail));
                tail = BuiltIn.Tail(tail);
            }
            return true;
        }

        public bool TryEvalOr(IClObj expr, out IClObj obj)
        {
            obj = ClBool.False;
            if (!expr.IsOr()) return false;
            var tail = BuiltIn.Tail(expr);
            var atLeastOne = BuiltIn.Seq(tail).Any(it => BuiltIn.IsTrue(Eval(it)).Value);
            if (atLeastOne) obj = ClBool.True;
            return true;
        }

        public bool TryEvalAnd(IClObj expr, out IClObj obj)
        {
            obj = ClBool.True;
            if (!expr.IsAnd()) return false;
            var tail = BuiltIn.Tail(expr);
            var atLeastOne = BuiltIn.Seq(tail).Any(it => BuiltIn.IsFalse(Eval(it)).Value);
            if (atLeastOne) obj = ClBool.False;
            return true;
        }

        public bool TryEvalLambda(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            if (!expr.IsLambda()) return false;
            if (BuiltIn.Cdddr(expr) != Nil.Given)
                throw new InvalidOperationException("Invalid body");
            var parameters = BuiltIn.Second(expr).Cast<ClCell>("Operands must be a cell");
            var hasUnsupportBinding = BuiltIn.Seq(parameters).Any(it => it.TypeOf<ClSymbol>() is null);
            if (hasUnsupportBinding)
                throw new InvalidOperationException(Errors.BuiltIn.UnsupportBinding);
            var body = BuiltIn.Third(expr);
            obj = new ClProc(parameters, body);
            return true;
        }

        public bool TryEvalIfThenElse(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            if (!expr.IsIfThenElse()) return false;
            var result = Eval(BuiltIn.Second(expr));
            if (result != Nil.Given && result != ClBool.False)
            {
                obj = Eval(BuiltIn.Third(expr));
                return true;
            }
            var elseBranch = BuiltIn.Cdddr(expr);
            obj = elseBranch ==  Nil.Given ? Nil.Given : Eval(BuiltIn.First(elseBranch));
            return true;
        }

        public bool TryEvalDefinition(IClObj expr, out IClObj obj)
        {
            // (define a 10) -> (define . (a . (10 . nil)))
            // (define a b) -> (define . (a . (b . nil)))
            // (define a a) -> (define . (a . (a . nil))) throw exception
            // TODO: (define keyword 10) i.e. (define lambda 10) Need reject
            obj = Nil.Given;
            if (!expr.IsDefinition()) return false;
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var result = Eval(BuiltIn.Third(expr));
            _env.Bind(identifier, result);
            return true;
        }

        public bool TryEvalAssigment(IClObj expr, out IClObj obj)
        {
            // (set! a 10) -> (set! . (a . (10 . nil)))
            // (set! a b) -> (set! . (a . (b . nil)))
            // (set! a a) -> (set! . (a . (a . nil))) reassign
            obj = Nil.Given;
            if (!expr.IsAssignment()) return false;
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var result = Eval(BuiltIn.Third(expr));
            _env.Assign(identifier, result);
            return true;
        }

        private IClObj TransformCond(IClObj expr)
        {
            var clauses = BuiltIn.Tail(expr);
            if (clauses == Nil.Given) return ClBool.False;
            var clause = BuiltIn.First(clauses).TypeOf<ClCell>();
            if (clause is null)
                throw new InvalidOperationException(Errors.BuiltIn.ClauseMustBeCell);
            if (clause.Car == ClSymbol.Else)
            {
                return BuiltIn.Tail(clauses) == Nil.Given
                    ? new ClCell(ClSymbol.Begin, clause.Cdr)
                    : throw new InvalidOperationException(Errors.BuiltIn.ElseClauseMustBeLast);
            }
            return BuiltIn.ListOf(ClSymbol.IfThenElse,
                clause.Car,
                new ClCell(ClSymbol.Begin, clause.Cdr),
                TransformCond(BuiltIn.Tail(clauses)));
        }
    }
}
