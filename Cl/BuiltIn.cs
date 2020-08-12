using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using Cl.Contracts;

namespace Cl
{
    public static class BuiltIn
    {
        public static IContext Eval(IEnumerable<IClObj> expressions) => expressions
            .Aggregate<IClObj, IContext>(new Context(Env), (ctx, expr) => expr.Reduce(ctx));

        public static IClObj Car(params IClObj[] obj)
        {
            if (obj != null && obj.Length != 1)
                throw new ArgumentException("Arity exception");
            if (obj[0] is ClCell cell) return cell.Car;
            throw new InvalidOperationException(Errors.BuiltIn.ArgumentMustBeCell);
        }

        public static IClObj Cdr(params IClObj[] obj)
        {
            if (obj != null && obj.Length != 1)
                throw new ArgumentException("Arity exception");
            if (obj[0] is ClCell cell) return cell.Cdr;
            throw new InvalidOperationException(Errors.BuiltIn.ArgumentMustBeCell);
        }

        public static IClObj Cadr(params IClObj[] obj) => Car(Cdr(obj));
        public static IClObj Cddr(params IClObj[] obj) => Cdr(Cdr(obj));
        public static IClObj Caddr(params IClObj[] obj) => Car(Cddr(obj));
        public static IClObj Cdddr(params IClObj[] obj) => Cdr(Cddr(obj));
        public static IClObj Cadddr(params IClObj[] obj) => Car(Cdddr(obj));
        public static ParamsFunc<IClObj, IClObj> Head = Car;
        public static ParamsFunc<IClObj, IClObj> Tail = Cdr;

        public static ParamsFunc<IClObj, IClObj> First = Car;
        public static ParamsFunc<IClObj, IClObj> Second = Cadr;
        public static ParamsFunc<IClObj, IClObj> Third = Caddr;
        public static ParamsFunc<IClObj, IClObj> Fourth = Cadddr;
        public static ClBool IsTrue(params IClObj[] obj)
        {
             if (obj != null && obj.Length != 1)
                throw new ArgumentException("Arity exception");
            return ClBool.Of(obj[0] != Nil.Given && obj[0] != ClBool.False);
        }
        public static ClBool IsFalse(params IClObj[] obj) => Negate(IsTrue(obj));
        public static ClBool Negate(params IClObj[] obj)
        {
            if (obj != null && obj.Length != 1)
                throw new ArgumentException("Arity exception");
            if (obj[0] is ClBool flag) return ClBool.Of(!flag.Value);
            throw new ArgumentException("Argument must have bool type");
        }


        public static ClCell ListOf(params IClObj[] items)
        {
            ClCell cell = Nil.Given;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                cell = new ClCell(items[i], cell);
            }
            return cell;
        }

        public static ClCell ListOf(IEnumerable<IClObj> items) => ListOf(items.ToArray());

        public static IEnumerable<IClObj> Seq(IClObj obj)
        {
            var cell = obj.TypeOf<ClCell>();
            if (cell is null) throw new InvalidOperationException();
            if (cell == Nil.Given) yield break;
            yield return cell.Car;
            var tail = cell.Cdr;
            while (tail is ClCell pair)
            {
                if (tail == Nil.Given) yield break;
                yield return pair.Car;
                tail = pair.Cdr;
            }
            yield return tail;
        }

        public static ClCell Quote(ClCell cell) => new ClCell(ClSymbol.Quote, cell);

        // Predicates
        public static ClBool IsNull(IClObj obj) => ClBool.Of(obj == Nil.Given);
        public static ClBool HasType<T>(IClObj obj) where T : IClObj => ClBool.Of(obj.TypeOf<T>() != null);
        public static ClBool IsString(IClObj obj) => HasType<ClString>(obj);
        public static ClBool IsSymbol(IClObj obj) => HasType<ClSymbol>(obj);
        public static ClBool IsInteger(IClObj obj) => HasType<ClFixnum>(obj);
        public static ClBool IsFloat(IClObj obj) => HasType<ClFloat>(obj);
        public static ClBool IsChar(IClObj obj) => HasType<ClChar>(obj);
        public static ClBool IsPair(IClObj obj) => HasType<ClCell>(obj);
        public static ClBool IsProcedure(IClObj obj) => HasType<ClFn>(obj);

        // Converts
        public static ClFixnum IntegerOfChar(IClObj obj) =>
            obj switch
            {
                ClChar ch => new ClFixnum((int) ch.Value),
                _ => throw new ArgumentException(Errors.BuiltIn.ArgumentIsNotOfType<ClChar>())
            };

        public static ClChar CharOfInteger(IClObj obj) =>
            obj switch
            {
                ClCell { Car: ClFixnum number } cell when cell.Cdr == Nil.Given => new ClChar((char) number.Value),
                // ClCell { Car: ClFixnum _ } => throw ArgumentException();
                _ => throw new ArgumentException(Errors.BuiltIn.ArgumentIsNotOfType<ClFixnum>())
            };



        public static ClString StringOfNumber(IClObj obj) =>
            obj switch
            {
                ClFixnum number => new ClString(number.ToString()),
                ClFloat number => new ClString(number.ToString()),
                _ => throw new ArgumentException()
            };

        // Pervasives
        public static IEnv Env = new Env(
            // (new ClSymbol("null?"), new NativeFn(IsNull)),
            // (new ClSymbol("string?"), new NativeFn(IsString)),
            // (new ClSymbol("symbol?"), new NativeFn(IsSymbol)),
            // (new ClSymbol("integer?"), new NativeFn(IsInteger)),
            // (new ClSymbol("float?"), new NativeFn(IsFloat)),
            // (new ClSymbol("char?"), new NativeFn(IsChar)),
            // (new ClSymbol("prodecure?"), new NativeFn(IsProcedure)),

            // (new ClSymbol("head"), new NativeFn(Head)),
            // (new ClSymbol("tail"), new NativeFn(Tail)),
            // (new ClSymbol("car"), new NativeFn(Car)),
            // (new ClSymbol("cdr"), new NativeFn(Cdr)),
            // (new ClSymbol("cadr"), new NativeFn(Cadr)),
            // (new ClSymbol("cddr"), new NativeFn(Cddr)),
            // (new ClSymbol("caddr"), new NativeFn(Caddr)),
            // (new ClSymbol("cadddr"), new NativeFn(Cadddr)),
            // (new ClSymbol("first"), new NativeFn(First)),
            // (new ClSymbol("second"), new NativeFn(Second)),
            // (new ClSymbol("third"), new NativeFn(Third)),
            // (new ClSymbol("fourth"), new NativeFn(Fourth)),
            // (new ClSymbol("true?"), new NativeFn(IsTrue)),
            // (new ClSymbol("false?"), new NativeFn(IsFalse))
        );
    }
}
