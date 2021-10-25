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
                    (list
                        'foo'
                        ;; comment )
                    )
                ",
                "(foo . nil)"
            };

            yield return new object[]
            {
                @"
                    ( ;; some
                    )
                ",
                "nil"
            };

            yield return new object[]
            {
                @"
                    ;; first

                    ;; second
                ",
                "nil"
            };

            yield return new object[]
            {
                @"
                    ;; comment
                    101
                ",
                "101"
            };

            yield return new object[]
            {
                @";; comment",
                "nil"
            };

            yield return new object[]
            {
                @"
                (defun count (coll)
                    (if (null? coll)
                        0
                        (+ 1 (count (tail coll)))))

                (defun at-index (i coll)
                    (if (or (null? coll) (lt i 0))
                        nil
                        (if (eq i 0)
                            (first coll)
                            (at-index (+ i (- 1)) (tail coll)))))

                (defun koa-compose (middleware)
                    ;; https://github.com/koajs/compose for more details
                    (lambda (context)
                        (begin
                            (define index (- 1))
                            (defun dispatch (i)
                                (if (gte index i)
                                    (println 'Next must be called only')
                                    (begin
                                        (set! index i)
                                        (if (eq i (count middleware))
                                            context
                                            (begin
                                                (define f (at-index i middleware))
                                                (f context (lambda ()
                                                    (dispatch (+ i 1)))))))))
                            (dispatch 0))))

                (defun increment (context next)
                    (+ 1 context))

                (defun multiply-by-3 (context next)
                    (* (next) 3))

                (define fn
                    (koa-compose
                        (list multiply-by-3 increment)))

                (list
                    (fn 0)
                    (fn 11))
                ",
                "(3 . (36 . nil))"
            };

            yield return new object[]
            {
                @"
                (defun at-index (i coll)
                    (if (or (null? coll) (lt i 0))
                        nil
                        (if (eq i 0)
                            (first coll)
                            (at-index (+ i (- 1)) (tail coll)))))

                (list
                    (at-index 2 (list 0 1 2))
                    (at-index (- 1) (list 0))
                    (at-index 0 (list #t #f #t)))
                ",
                "(2 . (nil . (#t . nil)))"
            };

            yield return new object[]
            {
                @"
                (defun prepend (x coll)
                    (cons x coll))

                (defun append (x coll)
                    (begin
                        (defun iter (coll)
                            (if (null? coll)
                                (cons x nil)
                                (cons
                                    (first coll)
                                    (iter (tail coll)))))
                        (iter coll)))
                (join ''
                    (append ']' (prepend '[' nil)))
                ",
                "[]"
            };

            yield return new object[]
            {
                @"
                (defun append (x coll)
                    (begin
                        (defun iter (coll)
                            (if (null? coll)
                                (cons x nil)
                                (cons
                                    (first coll)
                                    (iter (tail coll)))))
                        (iter coll)))

                (append 2 (list 0 1))
                ",
                "(0 . (1 . (2 . nil)))"
            };

            yield return new object[]
            {
                @"
                (defun prepend (x coll)
                    (cons x coll))

                (prepend 0 (list 1 2))
                ",
                "(0 . (1 . (2 . nil)))"
            };

            yield return new object[]
            {
                @"
                (defun count (coll)
                    (if (null? coll)
                        0
                        (+ 1 (count (tail coll)))))

                (list
                    (count nil)
                    (count (list 1))
                    (count (list 1 2 3))
                )
                ",
                "(0 . (1 . (3 . nil)))"
            };

            yield return new object[]
            {
                @"
                (defun where (f coll)
                    (if (null? coll)
                        nil
                        (if (f (first coll))
                            (cons
                                (first coll)
                                (where f (tail coll)))
                            (where f (tail coll)))))

                (where
                    (lambda (x)
                        (gt x 0))
                    (list
                        (- 1) 0 2 4 (- 10) 0 5))
                ",
                "(2 . (4 . (5 . nil)))"
            };

            yield return new object[]
            {
                @"
                (defun select (f coll)
                    (if (null? coll)
                        nil
                        (cons
                            (f (first coll))
                            (select f (tail coll)))))

                (select
                    (lambda (x)
                        (+ x 1))
                    (list 0 1))
                ",
                "(1 . (2 . nil))"
            };

            yield return new object[]
            {
                @"
                (defun and-then (f g)
                    (lambda (x)
                        (g (f x))))

                (defun compose (f g)
                    (and-then g f))

                (defun inc (x)
                    (+ 1 x))

                (defun multiply-by-3 (x)
                    (* x 3))

                (list
                    ((compose inc multiply-by-3) 0)
                    ((and-then inc multiply-by-3) 0))
                ",
                "(1 . (3 . nil))"
            };

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
