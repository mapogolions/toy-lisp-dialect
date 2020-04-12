using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using Cl.Abs;

namespace Cl
{
    public static class BuiltIn
    {
        public static IClObj Car(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Car,
                _ => throw new InvalidOperationException(Errors.BuiltIn.ArgumentMustBeCell)
            };

        public static IClObj Cdr(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Cdr,
                _ => throw new InvalidOperationException(Errors.BuiltIn.ArgumentMustBeCell)
            };

        public static IClObj Cadr(IClObj expr) => Car(Cdr(expr));
        public static IClObj Cddr(IClObj expr) => Cdr(Cdr(expr));
        public static IClObj Caddr(IClObj expr) => Car(Cddr(expr));
        public static IClObj Cdddr(IClObj expr) => Cdr(Cddr(expr));
        public static IClObj Cadddr(IClObj expr) => Car(Cdddr(expr));

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

        public static ClBool IsTrue(IClObj obj) => ClBool.Of(obj != Nil.Given && obj != ClBool.False);
        public static ClBool IsFalse(IClObj obj) => Negate(IsTrue(obj));
        public static ClBool Negate(ClBool flag) => ClBool.Of(!flag.Value);

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

        public static Func<IClObj, IClObj> Head = Car;
        public static Func<IClObj, IClObj> Tail = Cdr;

        public static Func<IClObj, IClObj> First = Car;
        public static Func<IClObj, IClObj> Second = Cadr;
        public static Func<IClObj, IClObj> Third = Caddr;
        public static Func<IClObj, IClObj> Fourth = Cadddr;

        // Predicates
        public static ClBool IsNull(IClObj obj) => ClBool.Of(obj == Nil.Given);
        public static ClBool HasType<T>(IClObj obj) where T : IClObj => ClBool.Of(obj.TypeOf<T>() != null);
        public static ClBool IsString(IClObj obj) => HasType<ClString>(obj);
        public static ClBool IsSymbol(IClObj obj) => HasType<ClSymbol>(obj);
        public static ClBool IsInteger(IClObj obj) => HasType<ClFixnum>(obj);
        public static ClBool IsFloat(IClObj obj) => HasType<ClFloat>(obj);
        public static ClBool IsChar(IClObj obj) => HasType<ClChar>(obj);
        public static ClBool IsPair(IClObj obj) => HasType<ClCell>(obj);
        public static ClBool IsProcedure(IClObj obj) => HasType<ClProcedure>(obj);

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
            (new ClSymbol("null?"), new PrimitiveProcedure(IsNull)),
            (new ClSymbol("string?"), new PrimitiveProcedure(IsString)),
            (new ClSymbol("symbol?"), new PrimitiveProcedure(IsSymbol)),
            (new ClSymbol("integer?"), new PrimitiveProcedure(IsInteger)),
            (new ClSymbol("float?"), new PrimitiveProcedure(IsFloat)),
            (new ClSymbol("char?"), new PrimitiveProcedure(IsChar)),
            (new ClSymbol("prodecure?"), new PrimitiveProcedure(IsProcedure)),

            (new ClSymbol("head"), new PrimitiveProcedure(Head)),
            (new ClSymbol("tail"), new PrimitiveProcedure(Tail)),
            (new ClSymbol("car"), new PrimitiveProcedure(Car)),
            (new ClSymbol("cdr"), new PrimitiveProcedure(Cdr)),
            (new ClSymbol("cadr"), new PrimitiveProcedure(Cadr)),
            (new ClSymbol("cddr"), new PrimitiveProcedure(Cddr)),
            (new ClSymbol("caddr"), new PrimitiveProcedure(Caddr)),
            (new ClSymbol("cadddr"), new PrimitiveProcedure(Cadddr)),
            (new ClSymbol("first"), new PrimitiveProcedure(First)),
            (new ClSymbol("second"), new PrimitiveProcedure(Second)),
            (new ClSymbol("third"), new PrimitiveProcedure(Third)),
            (new ClSymbol("fourth"), new PrimitiveProcedure(Fourth)),
            (new ClSymbol("true?"), new PrimitiveProcedure(IsTrue)),
            (new ClSymbol("false?"), new PrimitiveProcedure(IsFalse))
        );
    }
}
