## Toy LISP dialect

Yet another lisp interpreter

#### Inspired by [Liutos](https://github.com/Liutos/Camel-Lisp)


#### How to use

```sh
dotnet test
dotnet run --project .\Cl.Utop\Cl.Utop.csproj
```

By default REPL loads the [stdlib](./stdlib) file so you can use predefined functions written in toy lisp.

#### Hello World

```clojure
(define hello-world 'Hello World!')
(println hello-world)
```

In case you want more [examples](./examples.md)
