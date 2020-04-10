using Cl.Types;
using Cl.Abs;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class EnvTests
    {
        private ClSymbol _foo;
        private ClSymbol _bar;

        [OneTimeSetUp]
        public void BeforeClass()
        {
            _foo = new ClSymbol("foo");
            _bar = new ClSymbol("bar");
        }

        [Test]
        public void Extend_DoesNotAffectTheParentScope()
        {
            var outer = new Env();
            outer.Bind(new ClSymbol("x"), new ClString("foo"));
            var identifiers = BuiltIn.ListOf(new ClSymbol("x"));
            var values = BuiltIn.ListOf(new ClString("bar"));

            var inner = outer.Extend(identifiers, values);

            Assert.That(inner.Lookup(new ClSymbol("x")), Is.EqualTo(new ClString("bar")));
            Assert.That(outer.Lookup(new ClSymbol("x")), Is.EqualTo(new ClString("foo")));
        }

        [Test]
        public void Extend_CreateChainOfScopes()
        {
            var outer = new Env();

            var inner = outer.Extend(Nil.Given, Nil.Given);

            Assert.That(outer.AtTheTopLevel, Is.True);
            Assert.That(inner.AtTheTopLevel, Is.False);
        }

        [Test]
        public void Assign_ValueByKey_ThrowException_WhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env());

            Assert.That(() => env.Assign(_foo, ClBool.False), Throws.InvalidOperationException);
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenAtLeastOneFrameHasBinding()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(_bar, ClBool.False);

            Assert.That(inner.Assign(_bar, ClBool.True), Is.True);
            Assert.That(inner.Lookup(_bar), Is.EqualTo(ClBool.True));
            Assert.That(outer.Lookup(_bar), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenInnerFrameHasBinding()
        {
            var env = new Env();
            env.Bind(_foo, ClBool.False);

            Assert.That(env.Assign(_foo, ClBool.True), Is.True);
            Assert.That(env.Lookup(_foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ByKey_ThrowException_WhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env(new Env()));

            Assert.That(() => env.Lookup(_bar), Throws.InvalidOperationException);
        }

        [Test]
        public void Lookup_ByKey_ReturnFirstFoundValue()
        {
            var outer = new Env();
            var inner = new Env(outer);
            inner.Bind(_foo, ClBool.False);
            outer.Bind(_foo, ClBool.True);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromOuterFrame_WhenInnerFrameDoesNotContainKey()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(_foo, ClBool.True);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromInnerFrame()
        {
            var inner = new Env(new Env());
            inner.Bind(_foo, ClBool.True);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(ClBool.True));
        }

        [Test]
        public void Lookup_ByKey_ReturnValue()
        {
            var env = new Env();
            env.Bind(_bar,  ClBool.False);

            Assert.That(env.Lookup(_bar), Is.EqualTo(ClBool.False));
        }

        [Test]
        public void Bind_ExistingKey_ReassignValue()
        {
            var env = new Env();
            env.Bind(_foo, ClBool.True);

            Assert.That(env.Bind(_foo, ClBool.False), Is.True);
        }

        [Test]
        public void Bind_NotExistingKey_CreateNewValue()
        {
            var env = new Env();

            Assert.That(env.Bind(_bar, ClBool.False), Is.True);
        }
    }
}
