using System;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class EnvTests
    {
        private IEnv _env;

        [SetUp]
        public void BeforeEach()
        {
            _env = new Env();
        }

        [Test]
        public void Bind_ExistingKey_ReassignValue()
        {
            var key = new ClSymbol("a");
            _env.Bind(key, new ClBool(true));
            Assert.That(_env.Bind(key, new ClChar('a')), Is.True);
        }

        [Test]
        public void Bind_NotExistingKey_CreateNewValue()
        {
            Assert.That(_env.Bind(new ClSymbol("a"), new ClBool(true)), Is.True);
        }
    }
}
