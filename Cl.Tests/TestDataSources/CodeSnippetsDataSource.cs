using System.Collections;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Tests.TestDataSources
{
    public class CodeSnippetsDataSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                @"
                (defun increment (n)
                    (+ n 1))

                (increment 10)
                ",
                "11"
            };

            yield return new object[]
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

                (list
                    (start-from-10)
                    (start-from-10))
                ",
                "(11 . (12 . nil))"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
