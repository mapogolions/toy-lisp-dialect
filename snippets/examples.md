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

### Compound function body

```lisp
(define f
    (lambda ()
        (begin
            (define x 10) ;; side effect
            (define y 11) ;; affected only lexical env
            (list x y))))

(f)
```

### `quote` vs `list`
```lisp
(quote (define x 10))
(list (define x 10))
```
