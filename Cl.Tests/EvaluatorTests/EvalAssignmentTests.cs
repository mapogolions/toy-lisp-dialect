using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using Cl.Constants;
using Cl.Input;
using Cl.Types;
using NUnit.Framework;
using static Cl.Extensions.FpUniverse;

namespace Cl.Tests.EvaluatorTests
{
    [TestFixture]
    public class EvalAssignmentTests
    {
        [Test]
        public void EvalAssign_ReturnTrue()
        {
            Assert.That(true, Is.True);
        }
    }
}
