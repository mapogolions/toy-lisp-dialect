using System.Collections;
using System.Collections.Generic;

namespace Cl.Tests.TestDataSources
{
    public class CodeSnippetsDataSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                @"
                (defun count-down (n)
                    (if (eq n (- 1))
                        nil
                        (cons n
                            (count-down (+ n (- 1))))))

                (cons
                    (count-down 2)
                    (count-down 3))
                ",
                "((2 . (1 . (0 . nil))) . (3 . (2 . (1 . (0 . nil)))))"
            };

            yield return new object[]
            {
                @"
                (defun count-up (n)
                    (begin
                        (defun iter (current acc)
                            (if (eq current (- 1))
                                acc
                                (iter (+ current (- 1))
                                    (cons current acc))))
                        (iter n nil)))

                (count-up 3)
                ",
                "(0 . (1 . (2 . (3 . nil))))"
            };

            yield return new object[]
            {
                @"
                (defun inc (n)
                    (+ n 1))

                (defun dec (n)
                    (+ n (- 1)))

                (cons
                    (inc 11)
                    (dec 12))
                ",
                "(12 . 11)"
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
