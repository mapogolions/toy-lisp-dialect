## Toy LISP dialect

#### Inspired by [Liutos](https://github.com/Liutos/Camel-Lisp)

#### How to use
```sh
> dotnet test
> dotnet run --project <project-name>
```

If you want to test some code, the best choise for you might be the [CodeSnippetsTests](./Cl.Tests/CodeSnippetsTests.cs) class. The last provides basic scaffolding. Just put your code with expected result into the [CodeSnippetsDataSource](./Cl.Tests/TestDataSources/CodeSnippetsDataSource.cs) and run `dotnet test` command.

[Examples](./examples.md)
