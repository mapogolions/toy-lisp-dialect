using System.Collections;
using System.Collections.Generic;
using Cl.Types;

namespace Cl.Tests.TestDataSources
{
    public class TypeErrorDataSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                @"(int-of-string #\a)",
                $"Expected {nameof(ClString)}, but found {nameof(ClChar)}"
            };
            yield return new object[]
            {
                @"(double-of-string #\a)",
                $"Expected {nameof(ClString)}, but found {nameof(ClChar)}"
            };
            yield return new object[]
            {
                "(string-of-int (list 1 2))",
                $"Expected {nameof(ClInt)}, but found {nameof(ClCell)}"
            };
            yield return new object[]
            {
                "(string-of-double (list 1 2))",
                $"Expected {nameof(ClDouble)}, but found {nameof(ClCell)}"
            };
            yield return new object[]
            {
                "(int-of-char nil)",
                $"Expected {nameof(ClChar)}, but found {ClCell.Nil.GetType().Name}"
            };
            yield return new object[]
            {
                "(char-of-int #f)",
                $"Expected {nameof(ClInt)}, but found {nameof(ClBool)}"
            };
            yield return new object[]
            {
                "(double-of-string 'some')",
                $"'some' cannot be converted to {nameof(ClDouble)}"
            };
            yield return new object[]
            {
                "(int-of-string 'some')",
                $"'some' cannot be converted to {nameof(ClInt)}"
            };

            yield return new object[]
            {
                "(- #t)",
                $"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {nameof(ClBool)}"
            };
            yield return new object[]
            {
                "(- 'some')",
                $"Expected {nameof(ClInt)} or {nameof(ClDouble)}, but found {nameof(ClString)}"
            };

            yield return new object[]
            {
                "(string? 1 '34')",
                "Arity exception: function expects 1 arg, but passed 2"
            };

            yield return new object[]
            {
                "(/ 10 5 nil)",
                "Arity exception: function expects 2 args, but passed 3"
            };
            yield return new object[]
            {
                "(null?)",
                "Arity exception: function expects 1 arg, but passed 0"
            };
            yield return new object[]
            {
                @"
                (defun f (a b c d e f g h) nil)
                (f)
                ",
                "Arity exception: function expects 8 args, but passed 0"
            };

            yield return new object[]
            {
                "(lower 12)",
                $"Expected {nameof(ClString)}, but found {nameof(ClInt)}"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
