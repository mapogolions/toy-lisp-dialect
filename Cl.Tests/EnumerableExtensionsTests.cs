using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Cl.Core.Extensions;

namespace Cl.Tests
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void ShouldReturnSequenceOfPairs()
        {
            var seq = new List<int> { 1, 2 }.ZipIfBalanced(new List<char> { 'a', 'b' });

            Assert.That(seq.FirstOrDefault(), Is.EqualTo((1, 'a')));
            Assert.That(seq.LastOrDefault(), Is.EqualTo((2, 'b')));
        }

        [Test]
        public void ShouldThrowExceptionWhenSequencesAreOfDifferentLengths()
        {
            var seq = new List<int> { 1, 2, 3 }.ZipIfBalanced(Enumerable.Empty<int>());
            var errorMessage = "Enumerables are of different lengths";

            Assert.That(() => seq.ToList(),
                Throws.InvalidOperationException.With.Message.EqualTo(errorMessage));
        }
    }
}
