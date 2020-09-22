#### lexical environment

```clojure
(define f
    (lambda (x)
        (lambda (x)
            (lambda () x))))

(define g (f 10))
(define h (g 11))
(h)
```

#### compound function body

```clojure
(define f
    (lambda ()
        (begin
            (define x 10) ;; side effect
            (define y 11) ;; affected only lexical env
            (list x y))))

(f)
```

#### `quote` vs `list`
```clojure
(quote (define x 10))
(list (define x 10))
```

#### `callable?`
```clojure
(define x
    (list
        (callable? tail)
        (callable? (lambda (x) x))
        (callable? 3)))
x
```

#### check primitive
```clojure
(define x
    (list
        (char? #\a)
        (int? 12)
        (double? 3.3)))
x
```

#### `defun` is more consice version of `define-lambda`
```clojure
(defun f (x y)
    (list x y))

(f 10 11)

;; the same as
(define f
    (lambda (x y)
        (list x y)))

(f 10 11)
```

#### `cond`

```clojure
(define x
    (cond
        (#f 10)
        (nil 11)
        (else 0)))

(define y
    (cond
        (#t 10)
        (else 11)))

(list x y)
```

#### `let` under the hood
```clojure
(let ((x 1)
      (y x))
    (list x y))

;; the same as
((lambda ()
    (begin
        (define x 1)
        (define y x)
        (list x y))))
```

#### look under the hood
```clojure
(head (quote (begin 1 2)))
```

#### `list` vs `cons`
```clojure
(cons 1
    (cons 2
        (cons 3 nil)))

;; the same as
(list 1 2 3)
```

#### there are no negative numbers, but there is a special function
```clojure
(- 10)
```

#### sum
```clojure
(+ 1 2 3 4 (- 2.5))
```

#### arithmetic operations
```clojure
(list
    (+ 1 2 3 4 (- 2.5))
    (* 1 2 3 4)
    (+)
    (*)
    (/ 10 2.5))
```

#### simple map
```clojure
(defun plus-n (n)
    (lambda (number)
        (+ number n)))

(defun map (number fn)
    (fn number))

(map 10 (plus-n 1))
```

#### builtin map
```clojure
(map
    (list 'foo' 'bar' 'baz')
        (lambda (x) (repeat 2 x)))

(map
    (list 'foo' 'bar' 'baz') upper)
```


#### echo
```clojure
(echo
    (map (list 1 2) (lambda (x) (+ x 1))))
```
