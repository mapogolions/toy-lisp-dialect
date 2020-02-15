using Cl.Types;
using Cl.Abs;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class EnvTests
    {
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

        [Test]
        public void Assign_ValueByKey_ThrowException_WhenChainOfFramesDoesNotContainKey()
        {
            var env = new Env(new Env());

            Assert.That(() => env.Assign(_foo, _theFalse), Throws.InvalidOperationException);
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenAtLeastOneFrameHasBinding()
        {
            var outer = new Env();
            var inner = new Env(outer);
            outer.Bind(_bar, _theFalse);

            Assert.That(inner.Assign(_bar, _theTrue), Is.True);
            Assert.That(inner.Lookup(_bar), Is.EqualTo(_theTrue));
            Assert.That(outer.Lookup(_bar), Is.EqualTo(_theTrue));
        }

        [Test]
        public void Assign_ValueByKey_ReturnTrue_WhenInnerFrameHasBinding()
        {
            var env = new Env();
            env.Bind(_foo, _theFalse);

            Assert.That(env.Assign(_foo, _theTrue), Is.True);
            Assert.That(env.Lookup(_foo), Is.EqualTo(_theTrue));
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
            var env = new Env();
            env.Bind(_bar,  _theFalse);

            Assert.That(env.Lookup(_bar), Is.EqualTo(_theFalse));
        }

        [Test]
        public void Bind_ExistingKey_ReassignValue()
        {
            var env = new Env();
            env.Bind(_foo, _theTrue);

            Assert.That(env.Bind(_foo, _theFalse), Is.True);
        }

        [Test]
        public void Bind_NotExistingKey_CreateNewValue()
        {
            var env = new Env();

            Assert.That(env.Bind(_bar, _theFalse), Is.True);
        }
    }
}
