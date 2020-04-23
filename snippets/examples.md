### Lexical Environment

```lisp
(define f
    (lambda (x)
        (lambda (x)
            (lambda () x))))

(define g (f 10))
(define h (g 11))
(h)
```

### Partial application
```lisp
(((lambda (x x) x) 10) 11) ; 11
(((lambda (x y) x) 10) 11) ; 10
```
