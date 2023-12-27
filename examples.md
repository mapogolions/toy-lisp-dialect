#### lexical environment

```clojure
(define f
    (lambda (x)
        (lambda (x)
            (lambda () x))))

(define g (f 10))
(define h (g 11))
(h) ;; or (call h)
```

#### compound function body

```clojure
(define f
    (lambda ()
        (begin
            (define x 10) ;; side effect
            (define y 11) ;; affected lexical env only
            (list x y))))

(f) ;; or (call f)
```

#### `quote` vs `list`
```clojure
(quote (define x 10))

(list (define x 10))
```

#### `callable?`
```clojure
(list
    (callable? tail)
    (callable? (lambda (x) x))
    (callable? 3))
```

#### check primitive
```clojure
(list
    (char? #\a)
    (int? 12)
    (double? 3.3))
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

(defun custom-map (number fn)
    (fn number))

(custom-map 10 (plus-n 1))
```

#### print(ln)
```clojure
(println 10 11)
(print 10 11)
```

#### Captured variable contains the value at time of evaluation, not the time of capture
```clojure
(defun f ()
    (begin
        (define x 11)
        (define g (lambda () x))
        (set! x 12)
        g))

(call (f)) ;; 12
```

#### counter
```clojure
(defun counter (n)
    (lambda ()
        (begin
            (set! n (+ 1 n))
            n)))

(define start-from-10 (counter 10))
(define start-from-20 (counter 20))

(list
    (call start-from-10)
    (call start-from-20)
    (call start-from-10)
    (call start-from-20))
```

#### increment/decrement
```clojure
(defun create-inc-dec-pair (n)
    (begin
        (defun dec ()
            (begin
                (set! n (+ n (- 1)))
                n))
        (defun inc ()
            (begin
                (set! n (+ n 1))
                n))
        (list  inc dec)))

(define obj
    (create-inc-dec-pair 0))

(call (first obj))

(call (second obj))

(call (second obj))
```

#### koa-compose
```clojure
(defun koa-compose (middleware)
    (lambda (context)
        (begin
            (define index (- 1))
            (defun dispatch (i)
                (if (gte index i)
                    (println 'Next must be called only once')
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

(fn 0)
```
