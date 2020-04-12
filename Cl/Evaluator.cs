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
            // TODO: order of error
            obj = Nil.Given;
            var cell = expr.TypeOf<ClCell>();
            if (cell is null) return false;
            var procedure = Eval(cell.Car).Cast<ClProcedure>(Errors.Eval.UnknownProcedureType);
            var args = cell.Cdr.Cast<ClCell>();
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
            var conditions = BuiltIn.Tail(expr);
            var atLeastOneTruth = BuiltIn.Seq(conditions).Any(it => BuiltIn.IsTrue(Eval(it)).Value);
            if (atLeastOneTruth) obj = ClBool.True;
            return true;
        }

        public bool TryEvalAnd(IClObj expr, out IClObj obj)
        {
            obj = ClBool.True;
            if (!expr.IsAnd()) return false;
            var conditions = BuiltIn.Tail(expr);
            var atLeastOneFalse = BuiltIn.Seq(conditions).Any(it => BuiltIn.IsFalse(Eval(it)).Value);
            if (atLeastOneFalse) obj = ClBool.False;
            return true;
        }

        public bool TryEvalLambda(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            if (!expr.IsLambda()) return false;
            if (BuiltIn.Cdddr(expr) != Nil.Given)
                throw new InvalidOperationException(Errors.Eval.InvalidLambdaBody);
            var parameters = BuiltIn.Second(expr)
                .Cast<ClCell>(Errors.Eval.InvalidLambdaParameters);
            var hasUnsupportBinding = BuiltIn.Seq(parameters).Any(it => it.TypeOf<ClSymbol>() is null);
            if (hasUnsupportBinding)
                throw new InvalidOperationException(Errors.BuiltIn.UnsupportBinding);
            var body = BuiltIn.Third(expr);
            obj = new ClProcedure(parameters, body);
            return true;
        }

        public bool TryEvalIfThenElse(IClObj expr, out IClObj obj)
        {
            obj = Nil.Given;
            if (!expr.IsIfThenElse()) return false;
            var condition = Eval(BuiltIn.Second(expr));
            if (condition != Nil.Given && condition != ClBool.False)
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
            // NOTE: (define keyword 10) i.e. (define lambda 10) It's ok. Keywords and symbols table coexist independently
            obj = Nil.Given;
            if (!expr.IsDefinition()) return false;
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var result = Eval(BuiltIn.Third(expr));
            _env.Bind(identifier, result);
            return true;
        }

        public bool TryEvalAssigment(IClObj expr, out IClObj obj)
        {
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
