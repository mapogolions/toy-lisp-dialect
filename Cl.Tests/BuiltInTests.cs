using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests
{
    [TestFixture]
    public class BuiltInTests
    {
        [Test]
        public void Car_ThrowException_WhenArgumentIsNotCell()
        {
            Assert.That(() => BuiltIn.Car(new ClString("foo")),
                Throws.InvalidOperationException.With.Message.EqualTo("Argument is not a cell"));
        }
    }
}
