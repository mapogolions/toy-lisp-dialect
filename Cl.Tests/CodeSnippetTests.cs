using Cl.Extensions;
using Cl.Types;
using NUnit.Framework;

namespace Cl.Tests
{
    public class CodeSnippetTests
    {
        [Test]
        [TestCaseSource(nameof(CodeSnippets))]
        public void SnippetsTest(string snippet, ClObj expected)
        {
            using var reader = new Reader(snippet);
            var (actual, _) = BuiltIn.Eval(reader.Read());
            Assert.That(actual, Is.EqualTo(expected));
        }

        static object[] CodeSnippets =
            {
                new object[]
                {
                    @"
                        (defun counter (n)
                            (begin
                                (defun f ()
                                    (begin
                                        (set! n (+ 1 n))
                                        n))
                                f))

                        (define start-from-10 (counter 10))
                        (define start-from-20 (counter 20))

                        (start-from-10)
                    ",
                    new ClNumber<int>(11)
                }
            };
    }
}
