using System;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class EnvTests
    {
        private IEnv _env;
        private ClBool _theTrue;
        private ClBool _theFalse;
        private ClSymbol _foo;
        private ClSymbol _bar;

        [OneTimeSetUp]
        public void BeforeClass()
        {
            _theTrue = new ClBool(true);
            _theFalse = new ClBool(false);
            _foo = new ClSymbol("foo");
            _bar = new ClSymbol("bar");
        }

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
        }

        [Test]
        public void Lookup_ByKey_ThrowExceptionWhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env(new Env()));

            Assert.That(() => env.Lookup(_bar), Throws.InvalidOperationException);
        }

        [Test]
        public void Lookup_ByKey_ReturnFirstFoundValue()
        {
            var outer = new Env();
            var inner = new Env(outer);
            inner.Bind(_foo, _theFalse);
            outer.Bind(_foo, _theTrue);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(_theFalse));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromOuterFrame_WhenInnerFrameDoesNotContainKey()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(_foo, _theTrue);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(_theTrue));
        }

        [Test]
        public void Lookup_ByKey_ReturnValueFromInnerFrame()
        {
            var inner = new Env(new Env());
            inner.Bind(_foo, _theTrue);

            Assert.That(inner.Lookup(_foo), Is.EqualTo(_theTrue));
        }

        [Test]
        public void Lookup_ByKey_ReturnValue()
        {
            _env.Bind(_bar,  _theFalse);

            Assert.That(_env.Lookup(_bar), Is.EqualTo(_theFalse));
        }

        [Test]
        public void Bind_ExistingKey_ReassignValue()
        {
            _env.Bind(_foo, _theTrue);

            Assert.That(_env.Bind(_foo, _theFalse), Is.True);
        }

        [Test]
        public void Bind_NotExistingKey_CreateNewValue()
        {
            Assert.That(_env.Bind(new ClSymbol("a"), new ClBool(true)), Is.True);
        }
    }
}
