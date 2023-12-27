## Toy LISP dialect

Yet another lisp interpreter

#### Inspired by [Liutos](https://github.com/Liutos/Camel-Lisp)


#### How to use

```sh
dotnet test
```

```sh
dotnet run --project .\Cl.Utop\Cl.Utop.csproj
```

#### Hello World

```clojure
(defun for-each (n f)
    (if (gte n 0)
        (begin
            (f n)
            (for-each (- n 1) f))
        nil))
(println 'hello world')
```

In case you want some more [examples](./examples.md)
