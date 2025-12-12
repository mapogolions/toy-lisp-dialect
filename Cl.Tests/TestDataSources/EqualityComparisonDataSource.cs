using System.Collections;

namespace Cl.Tests.TestDataSources;

public class EqualityComparisonDataSource : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            "(list (lte 2 2) (gte (- 1) (- 1)))",
            "(#t . (#t . nil))"
        };

        yield return new object[]
        {
            "(gt 0 (- 10))",
            "#t"
        };

        yield return new object[]
        {
            "(lt 1 3)",
            "#t"
        };

        yield return new object[]
        {
            @"
                (defun f (x) nil)
                (defun g (x) nil)
                (not (eq f g))
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (defun f (x)
                    (list x (+ x 1)))

                (define g f)

                (eq f g)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define a #f)
                (define b #f)
                (eq a b)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define a #t)
                (define b #t)
                (eq a b)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define a 'foo')
                (define b 'foo')
                (eq a b)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define a (list 1))
                (define b a)
                (eq a b)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define a (list 1))
                (define b (list 1))
                (eq a b)
                ",
            "#f"
        };

        yield return new object[]
        {
            @"
                (define a nil)
                (define b (list))
                (eq a b)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define x 10)
                (define y x)
                (eq x y)
                ",
            "#t"
        };

        yield return new object[]
        {
            @"
                (define x 11)
                (define y 11)
                (eq x y)
                ",
            "#t"
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
