using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;

namespace Cl
{
    public static class BuiltIn
    {
        public static IClObj Car(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Car,
                _ => throw new InvalidOperationException("Argument is not a cell")
            };

        public static IClObj Cdr(IClObj expr) =>
            expr switch
            {
                ClCell cell => cell.Cdr,
                _ => throw new InvalidOperationException("Argument is not a cell")
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

        public static ClBool IsTrue(IClObj obj) => (obj != Nil.Given && obj != ClBool.False) ? ClBool.True : ClBool.False;

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
    }
}
