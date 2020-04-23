using System;
using System.Collections.Generic;
using System.Linq;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
    public class Evaluator
    {
        private IEnv _env;

        public Evaluator(IEnv env) => _env = env;

        public IClObj Eval(IList<IClObj> expressions)
        {
            IClObj result = null;
            foreach (var expression in expressions)
            {
                result = Eval(expression);
            }
            return result;
        }

        public IClObj Eval(IClObj expr)
        {
            if (expr.IsSelfEvaluating()) return expr;
            if (expr.IsVariable()) return _env.Lookup(expr.Cast<ClSymbol>());
            //TODO: maybe need evaluate
            if (expr.IsQuoted()) return BuiltIn.Tail(expr);
            if (expr.IsCond()) return Eval(TransformCond(expr));
            if (TryEvalExpression(EvalAssigment, expr, out var obj)) return obj;
            if (TryEvalExpression(EvalDefinition, expr, out obj)) return obj;
            if (TryEvalExpression(EvalIf, expr, out obj)) return obj;
            if (TryEvalExpression(EvalAnd, expr, out obj)) return obj;
            if (TryEvalExpression(EvalOr, expr, out obj)) return obj;
            if (TryEvalExpression(EvalBegin, expr, out obj)) return obj;
            if (TryEvalExpression(EvalLambda, expr, out obj)) return obj;
            if (TryEvalExpression(EvalApplication, expr, out obj)) return obj;
            throw new InvalidOperationException(Errors.Eval.EvaluationError);

        }
        public bool TryEvalExpression(Func<IClObj, IClObj> fn, IClObj expr, out IClObj result)
        {
            result = fn(expr);
            return result != null;
        }

        // TODO: For test purposes. Support currying out of the box
        public IClObj EvalApplication(IClObj expr)
        {
            var cell = expr.TypeOf<ClCell>();
            if (cell is null) return null;
            var procedure = Eval(cell.Car);
            var args = cell.Cdr.Cast<ClCell>();
            var values = BuiltIn.Seq(args).Select(it => Eval(it));
            if (!(procedure is ClProcedure proc))
                throw new InvalidOperationException("Error");
            var fn = BuiltIn.Curry(proc, BuiltIn.ListOf(values));
            if (fn.Varargs != Nil.Given) return fn;
            var parentEnv = _env;
            _env = fn.LexicalEnv;
            var result = Eval(fn.Body);
            _env = parentEnv;
            return result;
        }

        // public IClObj EvalApplication(IClObj expr)
        // {
        //     var cell = expr.TypeOf<ClCell>();
        //     if (cell is null) return null;
        //     var procedure = Eval(cell.Car);
        //     var args = cell.Cdr.Cast<ClCell>();
        //     var values = BuiltIn.Seq(args).Select(it => Eval(it));
        //     switch (procedure)
        //     {
        //         case PrimitiveProcedure proc:
        //             return proc.Apply(BuiltIn.ListOf(values));
        //         case ClProcedure proc:
        //             var parentEnv = _env;
        //             // _env = proc.LexicalEnv.Extend(proc.Varargs, BuiltIn.ListOf(values));
        //             _env = proc.LexicalEnv.Populate(proc.Varargs, BuiltIn.ListOf(values));
        //             var result = Eval(proc.Body);
        //             _env = parentEnv;
        //             return result;
        //         default:
        //             throw new InvalidOperationException(Errors.Eval.UnknownProcedureType);
        //     }
        // }

        public IClObj EvalBegin(IClObj expr)
        {
            if (!expr.IsBegin()) return null;
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
            if (!expr.IsOr()) return null;
            var conditions = BuiltIn.Tail(expr);
            return BuiltIn.Seq(conditions)
                .Select(it => Eval(it))
                .FirstOrLast(it => BuiltIn.IsTrue(it).Value) ?? ClBool.False;
        }

        public IClObj EvalAnd(IClObj expr)
        {
            if (!expr.IsAnd()) return null;
            var conditions = BuiltIn.Tail(expr);
            return BuiltIn.Seq(conditions)
                .Select(it => Eval(it))
                .FirstOrLast(it => BuiltIn.IsFalse(it).Value) ?? ClBool.True;
        }

        public IClObj EvalLambda(IClObj expr)
        {
            if (!expr.IsLambda()) return null;
            if (BuiltIn.Cdddr(expr) != Nil.Given)
                throw new InvalidOperationException(Errors.Eval.InvalidLambdaBody);
            var parameters = BuiltIn.Second(expr)
                .Cast<ClCell>(Errors.Eval.InvalidLambdaParameters);
            var hasUnsupportBinding = BuiltIn.Seq(parameters).Any(it => it.TypeOf<ClSymbol>() is null);
            if (hasUnsupportBinding)
                throw new InvalidOperationException(Errors.BuiltIn.UnsupportBinding);
            var body = BuiltIn.Third(expr);
            return new ClProcedure(parameters, body, new Env(_env));
            // return new ClProcedure(parameters, body, _env);
        }

        public IClObj EvalIf(IClObj expr)
        {
            if (!expr.IsIf()) return null;
            var condition = Eval(BuiltIn.Second(expr));
            if (condition != Nil.Given && condition != ClBool.False)
                return Eval(BuiltIn.Third(expr));
            var elseBranch = BuiltIn.Cdddr(expr);
            return elseBranch ==  Nil.Given ? Nil.Given : Eval(BuiltIn.First(elseBranch));
        }

        public IClObj EvalDefinition(IClObj expr)
        {
            if (!expr.IsDefinition()) return null;
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var result = Eval(BuiltIn.Third(expr));
            _env.Bind(identifier, result);
            return Nil.Given;
        }

        public IClObj EvalAssigment(IClObj expr)
        {
            if (!expr.IsAssignment()) return null;
            var identifier = BuiltIn.Second(expr).Cast<ClSymbol>();
            var result = Eval(BuiltIn.Third(expr));
            _env.Assign(identifier, result);
            return Nil.Given;
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
            return BuiltIn.ListOf(ClSymbol.If,
                clause.Car,
                new ClCell(ClSymbol.Begin, clause.Cdr),
                TransformCond(BuiltIn.Tail(clauses)));
        }
    }
}
