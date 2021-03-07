using System.Linq;
using System;
using System.Collections.Generic;
using Cl.Extensions;
using Cl.Types;
using Cl.Contracts;
using Cl.SpecialForms;
using Cl.Helpers;
using Cl.Errors;

namespace Cl
{
    public static class BuiltIn
    {
        public static IContext Eval(IEnumerable<ClObj> expressions, IContext ctx) => expressions
            .Aggregate<ClObj, IContext>(ctx, (ctx, expr) => expr.Reduce(ctx));
        public static IContext Eval(IEnumerable<ClObj> expressions) => Eval(expressions, new Context(Env));

        public static ClObj Car(params ClObj[] args) => ArrayHelpers.Unpack<ClCell>(args).Car;
        public static ClObj Cdr(params ClObj[] args) => ArrayHelpers.Unpack<ClCell>(args).Cdr;

        public static ClObj Cadr(params ClObj[] args) => Car(Cdr(args));
        public static ClObj Cddr(params ClObj[] args) => Cdr(Cdr(args));
        public static ClObj Caddr(params ClObj[] args) => Car(Cddr(args));
        public static ClObj Cdddr(params ClObj[] args) => Cdr(Cddr(args));
        public static ClObj Cadddr(params ClObj[] args) => Car(Cdddr(args));
        public static VarargDelegate<ClObj, ClObj> Head = Car;
        public static VarargDelegate<ClObj, ClObj> Tail = Cdr;

        public static VarargDelegate<ClObj, ClObj> First = Car;
        public static VarargDelegate<ClObj, ClObj> Second = Cadr;
        public static VarargDelegate<ClObj, ClObj> Third = Caddr;
        public static VarargDelegate<ClObj, ClObj> Fourth = Cadddr;
        public static ClBool IsTrue(params ClObj[] args)
        {
            var value = ArrayHelpers.Unpack<ClObj>(args);
            return ClBool.Of(value != ClCell.Nil && value != ClBool.False);
        }
        public static ClBool IsFalse(params ClObj[] args) => Not(IsTrue(args));
        public static ClBool Not(params ClObj[] args) => ClBool.Of((!ArrayHelpers.Unpack<ClBool>(args).Value));

        public static ClCell ListOf(params ClObj[] items)
        {
            ClCell cell = ClCell.Nil;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                cell = new ClCell(items[i], cell);
            }
            return cell;
        }

        public static ClCell Cons(params ClObj[] args)
        {
            var (head, tail) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return new ClCell(head, tail);
        }

        public static ClCell ListOf(IEnumerable<ClObj> items) => ListOf(items.ToArray());

        public static IEnumerable<ClObj> Seq(ClObj obj)
        {
            var cell = obj.Cast<ClCell>();
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
        public static ClBool IsNull(params ClObj[] args) =>
            ClBool.Of(ArrayHelpers.Unpack<ClObj>(args) == ClCell.Nil);
        public static ClBool HasType<T>(params ClObj[] args) where T : ClObj  =>
            ClBool.Of(ArrayHelpers.Unpack<ClObj>(args) as T is not null);
        public static ClBool IsString(params ClObj[] args) => HasType<ClString>(args);
        public static ClBool IsSymbol(params ClObj[] args) => HasType<ClSymbol>(args);
        public static ClBool IsInteger(params ClObj[] args) => HasType<ClInt>(args);
        public static ClBool IsDouble(params ClObj[] args) => HasType<ClDouble>(args);
        public static ClBool IsChar(params ClObj[] args) => HasType<ClChar>(args);
        public static ClBool IsPair(params ClObj[] args) => HasType<ClCell>(args);
        public static ClBool IsCallable(params ClObj[] args) => HasType<ClCallable>(args);
        public static ClInt IntOfString(params ClObj[] args) => ArrayHelpers.Unpack<ClString>(args).Cast<ClInt>();
        public static ClDouble DoubleOfString(params ClObj[] args) => ArrayHelpers.Unpack<ClString>(args).Cast<ClDouble>();
        public static ClString StringOfInt(params ClObj[] args) => ArrayHelpers.Unpack<ClInt>(args).Cast<ClString>();
        public static ClString StringOfDouble(params ClObj[] args) => ArrayHelpers.Unpack<ClDouble>(args).Cast<ClString>();
        public static ClInt IntOfChar(params ClObj[] args) => ArrayHelpers.Unpack<ClChar>(args).Cast<ClInt>();
        public static ClChar CharOfInt(params ClObj[] args) => ArrayHelpers.Unpack<ClInt>(args).Cast<ClChar>();

        public static ClBool Eq(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return ClBool.Of(a.Equals(b));
        }

        public static ClBool Lt(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return ClBool.Of(a.CompareTo(b) <= -1);
        }

        public static ClBool Lte(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return ClBool.Of(a.CompareTo(b) <= 0);
        }

        public static ClBool Gt(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return ClBool.Of(a.CompareTo(b) >= 1);
        }

        public static ClBool Gte(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return ClBool.Of(a.CompareTo(b) >= 0);
        }

        public static ClObj UnaryMinus(params ClObj[] args)
        {
            var _ = ArrayHelpers.Unpack<ClObj>(args);
            return _ switch
            {
                ClInt fixnum => -fixnum,
                ClDouble floatingPoint => -floatingPoint,
                _ => throw new TypeError($"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {_.GetType().Name}")
            };
        }

        public static ClObj Sum(params ClObj[] args) =>
            args.ToList<ClObj>().Aggregate<ClObj, ClObj>(new ClInt(0), (acc, seed) => acc + seed);

        public static ClObj Product(params ClObj[] args) =>
            args.Aggregate<ClObj, ClObj>(new ClInt(1), (acc, seed) => acc * seed);

        public static ClObj Divide(params ClObj[] args)
        {
            var (a, b) = ArrayHelpers.Unpack<ClObj, ClObj>(args);
            return a / b;
        }

        public static ClString Repeat(params ClObj[] args)
        {
            var (times, source) = ArrayHelpers.Unpack<ClInt, ClString>(args);
            return new ClString(string.Concat(Enumerable.Repeat(source.Value, times.Value)));
        }

        public static ClString Upper(params ClObj[] args)
        {
            var source = ArrayHelpers.Unpack<ClString>(args);
            return new ClString(source.Value.ToUpper());
        }

        public static ClString Lower(params ClObj[] args)
        {
            var source = ArrayHelpers.Unpack<ClString>(args);
            return new ClString(source.Value.ToLower());
        }

        public static ClObj Map(params ClObj[] args)
        {
            (ClCell coll, ClCallable callable) = ArrayHelpers.Unpack<ClCell, ClCallable>(args);
            var ctx = new Context(Env);
            return Seq(coll).Select(x => {
                    var resultCtx = new ApplySpecialForm(callable, ListOf(x)).Reduce(ctx);
                    return resultCtx.Value;
                }).ListOf();
        }

        public static ClObj Filter(params ClObj[] args)
        {
            (ClCell coll, ClCallable callable) = ArrayHelpers.Unpack<ClCell, ClCallable>(args);
            var ctx = new Context(Env);
            return Seq(coll).Where(x => {
                    var resultCtx = new ApplySpecialForm(callable, ListOf(x)).Reduce(ctx);
                    return resultCtx.Value is ClBool flag && flag.Value is true;
                }).ListOf();
        }

        public static ClObj Println(params ClObj[] args)
        {
            args.ForEach(x => Console.WriteLine(x));
            return ClCell.Nil;
        }

        public static ClObj Print(params ClObj[] args)
        {
            args.ForEach(x => Console.Write(x));
            return ClCell.Nil;
        }

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
            (new ClSymbol("print"), new NativeFn(Print)),
            (new ClSymbol("eq"), new NativeFn(Eq)),
            (new ClSymbol("lt"), new NativeFn(Lt)),
            (new ClSymbol("gt"), new NativeFn(Gt)),
            (new ClSymbol("lte"), new NativeFn(Lte)),
            (new ClSymbol("gte"), new NativeFn(Gte))
        );
    }
}
