using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using Cl.Contracts;
using Cl.SpecialForms;

namespace Cl
{
    public static class BuiltIn
    {
        public static IContext Eval(IEnumerable<ClObj> expressions) => expressions
            .Aggregate<ClObj, IContext>(new Context(Env), (ctx, expr) => expr.Reduce(ctx));

        public static ClObj Car(params ClObj[] obj) => obj.Unpack<ClCell>().Car;
        public static ClObj Cdr(params ClObj[] obj) => obj.Unpack<ClCell>().Cdr;

        public static ClObj Cadr(params ClObj[] obj) => Car(Cdr(obj));
        public static ClObj Cddr(params ClObj[] obj) => Cdr(Cdr(obj));
        public static ClObj Caddr(params ClObj[] obj) => Car(Cddr(obj));
        public static ClObj Cdddr(params ClObj[] obj) => Cdr(Cddr(obj));
        public static ClObj Cadddr(params ClObj[] obj) => Car(Cdddr(obj));
        public static ParamsFunc<ClObj, ClObj> Head = Car;
        public static ParamsFunc<ClObj, ClObj> Tail = Cdr;

        public static ParamsFunc<ClObj, ClObj> First = Car;
        public static ParamsFunc<ClObj, ClObj> Second = Cadr;
        public static ParamsFunc<ClObj, ClObj> Third = Caddr;
        public static ParamsFunc<ClObj, ClObj> Fourth = Cadddr;
        public static ClBool IsTrue(params ClObj[] obj)
        {
            var value = obj.Unpack<ClObj>();
            return ClBool.Of(value != ClCell.Nil && value != ClBool.False);
        }
        public static ClBool IsFalse(params ClObj[] obj) => Not(IsTrue(obj));
        public static ClBool Not(params ClObj[] obj) => ClBool.Of((!obj.Unpack<ClBool>().Value));

        public static ClCell ListOf(params ClObj[] items)
        {
            ClCell cell = ClCell.Nil;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                cell = new ClCell(items[i], cell);
            }
            return cell;
        }

        public static ClCell Cons(params ClObj[] obj)
        {
            var (head, tail) = obj.Unpack<ClObj, ClObj>();
            return new ClCell(head, tail);
        }

        public static ClCell ListOf(IEnumerable<ClObj> items) => ListOf(items.ToArray());

        public static IEnumerable<ClObj> Seq(ClObj obj)
        {
            var cell = obj.TypeOf<ClCell>();
            if (cell is null) throw new InvalidOperationException();
            if (cell == ClCell.Nil) yield break;
            yield return cell.Car;
            var tail = cell.Cdr;
            while (tail is ClCell pair)
            {
                if (tail == ClCell.Nil) yield break;
                yield return pair.Car;
                tail = pair.Cdr;
            }
            yield return tail;
        }

        public static ClObj Quote(ClObj obj) => new ClCell(ClSymbol.Quote, obj);

        // Predicates
        public static ClBool IsNull(params ClObj[] obj) => ClBool.Of(obj.Unpack<ClObj>() == ClCell.Nil);
        public static ClBool HasType<T>(params ClObj[] obj) where T : ClObj  =>
            ClBool.Of(obj.Unpack<ClObj>().TypeOf<T>() != null);

        public static ClBool IsString(params ClObj[] obj) => HasType<ClString>(obj);
        public static ClBool IsSymbol(params ClObj[] obj) => HasType<ClSymbol>(obj);
        public static ClBool IsInteger(params ClObj[] obj) => HasType<ClInt>(obj);
        public static ClBool IsDouble(params ClObj[] obj) => HasType<ClDouble>(obj);
        public static ClBool IsChar(params ClObj[] obj) => HasType<ClChar>(obj);
        public static ClBool IsPair(params ClObj[] obj) => HasType<ClCell>(obj);
        public static ClBool IsCallable(params ClObj[] obj) => HasType<ClCallable>(obj);

        // Converts
        public static ClInt IntOfString(params ClObj[] obj) => (ClInt) obj.Unpack<ClString>();
        public static ClDouble DoubleOfString(params ClObj[] obj) => (ClDouble) obj.Unpack<ClString>();
        public static ClString StringOfInt(params ClObj[] obj) => (ClString) obj.Unpack<ClInt>();
        public static ClString StringOfDouble(params ClObj[] obj) => (ClString) obj.Unpack<ClDouble>();
        public static ClInt IntOfChar(params ClObj[] obj) => (ClInt) obj.Unpack<ClChar>();
        public static ClChar CharOfInt(params ClObj[] obj) => (ClChar) obj.Unpack<ClInt>();

        // Arithmetics
        public static ClObj UnaryMinus(params ClObj[] obj) =>
            obj.Unpack<ClObj>() switch
            {
                ClInt fixnum => -fixnum,
                ClDouble floatingPoint => -floatingPoint,
                _ => throw new InvalidOperationException()
            };

        public static ClObj Sum(params ClObj[] obj) =>
            obj.ToList<ClObj>().Aggregate<ClObj, ClObj>(new ClInt(0), (acc, seed) => acc + seed);

        public static ClObj Product(params ClObj[] obj) =>
            obj.ToList<ClObj>().Aggregate<ClObj, ClObj>(new ClInt(1), (acc, seed) => acc * seed);

        public static ClObj Divide(params ClObj[] obj)
        {
            (ClObj numerator, ClObj denominator) = obj.Unpack<ClObj, ClObj>();
            return numerator / denominator;
        }

        // String functions
        public static ClString Repeat(params ClObj[] obj)
        {
            (ClInt count, ClString source) = obj.Unpack<ClInt, ClString>();
            return new ClString(string.Concat(Enumerable.Repeat(source.Value, count.Value)));
        }

        public static ClString Upper(params ClObj[] obj)
        {
            var source = obj.Unpack<ClString>();
            return new ClString(source.Value.ToUpper());
        }

        public static ClString Lower(params ClObj[] obj)
        {
            var source = obj.Unpack<ClString>();
            return new ClString(source.Value.ToLower());
        }

        public static ClObj Map(params ClObj[] obj)
        {
            (ClCell coll, ClCallable callable) = obj.Unpack<ClCell, ClCallable>();
            var ctx = new Context(Env);
            return Seq(coll).Select(x => {
                    var (result, _) = new ApplySpecialForm(callable, ListOf(x)).Reduce(ctx);
                    return result;
                }).ListOf();
        }

        public static ClObj Filter(params ClObj[] obj)
        {
            (ClCell coll, ClCallable callable) = obj.Unpack<ClCell, ClCallable>();
            var ctx = new Context(Env);
            return Seq(coll).Where(x => {
                    var (result, _) = new ApplySpecialForm(callable, ListOf(x)).Reduce(ctx);
                    return result is ClBool flag && flag.Value is true;
                }).ListOf();
        }

        public static ClObj Println(params ClObj[] obj)
        {
            obj.ToList().ForEach(x => Console.WriteLine(x));
            return ClCell.Nil;
        }

        public static ClObj Print(params ClObj[] obj)
        {
            obj.ToList().ForEach(x => Console.Write(x));
            return ClCell.Nil;
        }

        // Pervasives
        public static IEnv Env = new Env(
            (new ClSymbol("null?"), new NativeFn(IsNull)),
            (new ClSymbol("string?"), new NativeFn(IsString)),
            (new ClSymbol("symbol?"), new NativeFn(IsSymbol)),
            (new ClSymbol("int?"), new NativeFn(IsInteger)),
            (new ClSymbol("double?"), new NativeFn(IsDouble)),
            (new ClSymbol("char?"), new NativeFn(IsChar)),
            (new ClSymbol("callable?"), new NativeFn(IsCallable)),
            (new ClSymbol("head"), new NativeFn(Head)),
            (new ClSymbol("tail"), new NativeFn(Tail)),
            (new ClSymbol("car"), new NativeFn(Car)),
            (new ClSymbol("cdr"), new NativeFn(Cdr)),
            (new ClSymbol("cadr"), new NativeFn(Cadr)),
            (new ClSymbol("cddr"), new NativeFn(Cddr)),
            (new ClSymbol("caddr"), new NativeFn(Caddr)),
            (new ClSymbol("cadddr"), new NativeFn(Cadddr)),
            (new ClSymbol("first"), new NativeFn(First)),
            (new ClSymbol("second"), new NativeFn(Second)),
            (new ClSymbol("third"), new NativeFn(Third)),
            (new ClSymbol("fourth"), new NativeFn(Fourth)),
            (new ClSymbol("true?"), new NativeFn(IsTrue)),
            (new ClSymbol("false?"), new NativeFn(IsFalse)),
            (new ClSymbol("not"), new NativeFn(Not)),
            (new ClSymbol("list"), new NativeFn(ListOf)),
            (new ClSymbol("cons"), new NativeFn(Cons)),
            (new ClSymbol("int-of-string"), new NativeFn(IntOfString)),
            (new ClSymbol("double-of-string"), new NativeFn(DoubleOfString)),
            (new ClSymbol("string-of-int"), new NativeFn(StringOfInt)),
            (new ClSymbol("string-of-double"), new NativeFn(StringOfDouble)),
            (new ClSymbol("int-of-char"), new NativeFn(IntOfChar)),
            (new ClSymbol("char-of-int"), new NativeFn(CharOfInt)),
            (new ClSymbol("-"), new NativeFn(UnaryMinus)),
            (new ClSymbol("+"), new NativeFn(Sum)),
            (new ClSymbol("*"), new NativeFn(Product)),
            (new ClSymbol("/"), new NativeFn(Divide)),
            (new ClSymbol("repeat"), new NativeFn(Repeat)),
            (new ClSymbol("upper"), new NativeFn(Upper)),
            (new ClSymbol("lower"), new NativeFn(Lower)),
            (new ClSymbol("map"), new NativeFn(Map)),
            (new ClSymbol("filter"), new NativeFn(Filter)),
            (new ClSymbol("println"), new NativeFn(Println)),
            (new ClSymbol("print"), new NativeFn(Print))
            // (new ClSymbol("mod"), new NativeFn(Mod)),
            // (new ClSymbol("rem"), new NativeFn(Rem))
        );
    }
}
