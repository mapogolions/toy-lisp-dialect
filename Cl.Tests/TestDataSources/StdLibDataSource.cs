using System.Collections;

namespace Cl.Tests.TestDataSources
{
    public class StdLibDataSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // read
            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (read (quote (define x 1) (define y 2)))
                            (+ x y))))
                ",
                "3"
            };

            // partial
            yield return new object[]
            {
                // `partial` has the same behaviour as `call` function if all args are passed
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b c d) (+ a b c d))
                            (partial add (list 1 2 3 4)))))
                ",
                "10"
            };
            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b c d) (+ a b c d))
                            (define f (partial add (list 1 2)))
                            (list
                                ((f 3) 4)
                                (f (list 3 4))
                                ((f 3) (list 4))))))
                ",
                "(10 . (10 . (10 . nil)))"
            };
            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b c d) (+ a b c d))
                            (define f (partial add 1))
                            (list
                                (((f 2) 3) 4)
                                (f (list 2 3 4))
                                ((f 2) (list 3 4))
                                (((f (list 2)) 3) (list 4))))))
                ",
                "(10 . (10 . (10 . (10 . nil))))"
            };
            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b) (+ a b))
                            (define succ (partial add (list 1)))
                            (define pred (partial add (list (- 1))))
                            (list
                                (succ 1) (pred 1)))))
                ",
                "(2 . (0 . nil))"
            };

            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b) (+ a b))
                            (define succ (partial add 1))
                            (define pred (partial add (- 1)))
                            (list
                                (succ 1) (pred 1)))))
                ",
                "(2 . (0 . nil))"
            };

            // call
            yield return new object[]
            {
                @"
                (invoke
                    (lambda ()
                        (begin
                            (defun add (a b) (+ a b))
                            (call add (list 1 2)))))
                ",
                "3"
            };

            // find-index
            yield return new object[]
            {
                @"(find-index (lambda (x) (eq x 0)) (list 1 0 2 0))",
                "1"
            };

            // matches
            yield return new object[]
            {
                @"(matches (lambda (x) (lte x 0)) nil)",
                "0"
            };
            yield return new object[]
            {
                @"(matches (lambda (x) (lte x 0)) (list 2 1 2))",
                "0"
            };
            yield return new object[]
            {
                @"(matches (lambda (x) (eq x 2)) (list 2 1 2))",
                "2"
            };

            // for-each
            yield return new object[]
            {
                @"
                (let ((n 0))
                    (begin
                        (for-each
                            (lambda (x)
                                (set! n (+ n x)))
                            (list 1 2 3 4))
                        n))
                ",
                "10"
            };

            // assoc
            yield return new object[]
            {
                @"(assoc nil)",
                "nil"
            };
            yield return new object[]
            {
                @"(assoc (list 'a' 'b'))",
                "((0 . (a . nil)) . ((1 . (b . nil)) . nil))"
            };

            // seed-n
            yield return new object[]
            {
                @"(seed-n 1 3)",
                "(1 . (1 . (1 . nil)))"
            };

            // flatten
            yield return new object[]
            {
                @"(flatten (list (list (list 1 2) 3) 4 (list 5 (list 6 (list 7)) 8)))",
                "(1 . (2 . (3 . (4 . (5 . (6 . (7 . (8 . nil))))))))"
            };
            yield return new object[]
            {
                @"(flatten (list 1 (list 2 (list 3))))",
                "(1 . (2 . (3 . nil)))"
            };
            yield return new object[]
            {
                @"(flatten (list 1 2 3))",
                "(1 . (2 . (3 . nil)))"
            };

            // flat-map
            yield return new object[]
            {
                @"(flat-map (lambda (x) (if (eq x 2) (list x x) x)) (list 1 2))",
                "(1 . (2 . (2 . nil)))"
            };
            yield return new object[]
            {
                @"(flat-map (lambda (x) (list x x)) (list 1 2))",
                "(1 . (1 . (2 . (2 . nil))))"
            };

            // group-n
            yield return new object[]
            {
                @"(group-n (- 1) (list 1 2 3))",
                "(1 . (2 . (3 . nil)))"
            };
            yield return new object[]
            {
                @"(group-n 0 (list 1 2 3))",
                "(1 . (2 . (3 . nil)))"
            };

            yield return new object[]
            {
                @"(group-n 4 (list 1 2 3))",
                "((1 . (2 . (3 . nil))) . nil)"
            };
            yield return new object[]
            {
                @"(group-n 2 (list 1 2 3))",
                "((1 . (2 . nil)) . ((3 . nil) . nil))"
            };

            // concat
            yield return new object[]
            {
                @"(concat nil (list 1 2))",
                "(1 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(concat (list 1 2) nil)",
                "(1 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(concat (list 1) (list 2))",
                "(1 . (2 . nil))"
            };

            // all
            yield return new object[]
            {
                @"(all (lambda (x) (gte x 0)) (list 1 0 2))",
                "#t"
            };
            yield return new object[]
            {
                @"(all (lambda (x) (lte x 0)) nil)",
                "#t"
            };
            yield return new object[]
            {
                @"(all (lambda (x) (lte x 0)) (list 1 2))",
                "#f"
            };

            // any
            yield return new object[]
            {
                @"(any (lambda (x) (eq x 0)) (list 1 0 2))",
                "#t"
            };
            yield return new object[]
            {
                @"(any (lambda (x) (lte x 0)) nil)",
                "#f"
            };
            yield return new object[]
            {
                @"(any (lambda (x) (lte x 0)) (list 1 2))",
                "#f"
            };

            // reverse
            yield return new object[]
            {
                @"(reverse (list 1 2))",
                "(2 . (1 . nil))"
            };
            yield return new object[]
            {
                @"(reverse nil)",
                "nil"
            };

            // skip
            yield return new object[]
            {
                @"(skip (- 1) (list 1 2))",
                "(1 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(skip 0 (list 1 2))",
                "(1 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(skip 10 (list 1 2))",
                "nil"
            };
            yield return new object[]
            {
                @"(skip 1 (list 1 2))",
                "(2 . nil)"
            };

            // take
            yield return new object[]
            {
                @"(take (- 1) (list 1))",
                "nil"
            };
            yield return new object[]
            {
                @"(take 0 (list 1))",
                "nil"
            };
            yield return new object[]
            {
                @"(take 2 (list 1))",
                "(1 . nil)"
            };
            yield return new object[]
            {
                @"(take 2 (list 1 2 3))",
                "(1 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(take 2 nil)",
                "nil"
            };

            // zip
            yield return new object[]
            {
                @"(zip (list 1 2) nil)",
                "nil"
            };
            yield return new object[]
            {
                @"(zip (list 1) (list 'a' 'b'))",
                "((1 . (a . nil)) . nil)"
            };

            yield return new object[]
            {
                @"(zip (list 1 2) (list 'a'))",
                "((1 . (a . nil)) . nil)"
            };
            yield return new object[]
            {
                @"(zip (list 1 2) (list 'a' 'b'))",
                "((1 . (a . nil)) . ((2 . (b . nil)) . nil))"
            };

            // append
            yield return new object[]
            {
                @"(append 3 (list 1))",
                "(1 . (3 . nil))"
            };
            yield return new object[]
            {
                @"(append 3 nil)",
                "(3 . nil)"
            };

            // fold-right
            yield return new object[]
            {
                @"(fold-right * 1 (range 1 5))",
                "24"
            };
            yield return new object[]
            {
                @"(fold-right + 0 (range 1 5))",
                "10"
            };

            // fold-left
            yield return new object[]
            {
                @"(fold-left * 1 (range 1 5))",
                "24"
            };
            yield return new object[]
            {
                @"(fold-left + 0 (range 1 5))",
                "10"
            };

            // filter
            yield return new object[]
            {
                @"(filter (lambda (x) (lt x 0)) (list (- 1) 2 (- 4)))",
                "(-1 . (-4 . nil))"
            };
            yield return new object[]
            {
                @"(filter (lambda (x) (lt x 0)) nil)",
                "nil"
            };

            // map
            yield return new object[]
            {
                @"(map (lambda (x) (+ 1 x)) nil)",
                "nil"
            };
            yield return new object[]
            {
                @"(map (lambda (x) (+ 1 x)) (list 0 1))",
                "(1 . (2 . nil))"
            };

            // at-index
            yield return new object[]
            {
                @"(at-index (- 1) (list 'a' 'b' 'c'))",
                "nil"
            };
            yield return new object[]
            {
                @"(at-index 100 (list 'a' 'b' 'c'))",
                "nil"
            };
            yield return new object[]
            {
                @"(at-index 0 (list 'a' 'b' 'c'))",
                "a"
            };

            // count
            yield return new object[]
            {
                @"(count (list 1 2 4))",
                "3"
            };
            yield return new object[]
            {
                @"(count (list 1))",
                "1"
            };
            yield return new object[]
            {
                @"(count nil)",
                "0"
            };

            // range
            yield return new object[]
            {
                @"(range 1 2)",
                "(1 . nil)"
            };
            yield return new object[]
            {
                @"(range 1 1)",
                "nil"
            };
            yield return new object[]
            {
                @"(range 2 1)",
                "(2 . nil)"
            };
            yield return new object[]
            {
                @"(range 3 1)",
                "(3 . (2 . nil))"
            };
            yield return new object[]
            {
                @"(range (- 3) 1)",
                "(-3 . (-2 . (-1 . (0 . nil))))"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
