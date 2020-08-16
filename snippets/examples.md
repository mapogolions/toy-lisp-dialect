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

### `callable?`
```lisp
(define x
    (list
        (callable? tail)
        (callable? (lambda (x) x))
        (callable? 3)))
x
```

### check primitive
```lisp
(define x
    (list
        (char? #\a)
        (int? 12)
        (float? 3.3)))
x
```

### `defun` is more consice version of `define-lambda`
```lisp
(defun f (x y)
    (list x y))

(f 10 11)

;; the same as
(define f
    (lambda (x y)
        (list x y)))

(f 10 11)
```
