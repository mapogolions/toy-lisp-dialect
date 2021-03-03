using Cl.Types;
using NUnit.Framework;
using Cl.Extensions;
using Cl.Exceptions;

namespace Cl.Tests
{
    [TestFixture]
    public class EnvTests
    {
        [Test]
        public void Env_InjectPredifinedEntities()
        {
            var env = new Env(BuiltIn.Env);
            var nativeFn = env.Lookup(new ClSymbol("head")).Cast<NativeFn>();
            var args = BuiltIn.ListOf(ClBool.True, ClBool.False);

            Assert.That(nativeFn.Apply(args), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Assign_ValueByKey_ThrowException_WhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env());

            Assert.That(() => env.Assign(Var.Foo, ClBool.False),
                Throws.Exception.TypeOf<UnboundVariableException>().With.Message.EqualTo("Unbound variable foo"));
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenAtLeastOneFrameHasBinding()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(Var.Bar, ClBool.False);

            Assert.That(inner.Assign(Var.Bar, ClBool.True), Is.True);
            Assert.That(inner.Lookup(Var.Bar), Is.EqualTo(ClBool.True));
            Assert.That(outer.Lookup(Var.Bar), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenInnerFrameHasBinding()
        {
            var env = new Env();
            env.Bind(Var.Foo, ClBool.False);

            Assert.That(env.Assign(Var.Foo, ClBool.True), Is.True);
            Assert.That(env.Lookup(Var.Foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_IdentifiersAndKeywrodCoexistIndependently()
        {
            var env = new Env((ClSymbol.If, ClBool.True));

            Assert.That(env.Lookup(ClSymbol.If), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ThrowExpception_WhenIdentifierIsKeyword()
        {
            var env = new Env();
            Assert.That(() => env.Lookup(ClSymbol.If),
                Throws.Exception.TypeOf<UnboundVariableException>().With.Message.EqualTo("Unbound variable if"));
        }

        [Test]
        public void Lookup_ByKey_ThrowException_WhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env(new Env()));

            Assert.That(() => env.Lookup(Var.Bar),
                Throws.Exception.TypeOf<UnboundVariableException>().With.Message.EqualTo("Unbound variable bar"));
        }

        [Test]
        public void Lookup_ByKey_ReturnFirstFoundValue()
        {
            var outer = new Env();
            var inner = new Env(outer);
            inner.Bind(Var.Foo, ClBool.False);
            outer.Bind(Var.Foo, ClBool.True);

            Assert.That(inner.Lookup(Var.Foo), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromOuterFrame_WhenInnerFrameDoesNotContainKey()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(Var.Foo, ClBool.True);

            Assert.That(inner.Lookup(Var.Foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromInnerFrame()
        {
            var inner = new Env(new Env());
            inner.Bind(Var.Foo, ClBool.True);

            Assert.That(inner.Lookup(Var.Foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ByKey_ReturnValue()
        {
            var env = new Env();
            env.Bind(Var.Bar,  ClBool.False);

            Assert.That(env.Lookup(Var.Bar), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void Bind_ExistingKey_ReassignValue()
        {
            var env = new Env();
            env.Bind(Var.Foo, ClBool.True);

            Assert.That(env.Bind(Var.Foo, ClBool.False), Is.True);
        }

        [Test]
        public void Bind_NotExistingKey_CreateNewValue()
        {
            var env = new Env();

            Assert.That(env.Bind(Var.Bar, ClBool.False), Is.True);
        }
    }
}
