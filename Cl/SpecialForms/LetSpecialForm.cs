using System.Collections.Generic;
using System.Linq;
using Cl.Core;
using Cl.Errors;
using Cl.Core.Extensions;
using Cl.Types;

namespace Cl.SpecialForms
{
    public class LetSpecialForm : TaggedSpecialForm
    {
        public LetSpecialForm(ClObj cdr) : base(ClSymbol.Let, cdr) { }

        public override IContext Reduce(IContext ctx)
        {
            var tail = BuiltIn.Tail(Cdr);
            if (tail == ClCell.Nil || BuiltIn.Cdr(tail) != ClCell.Nil)
            {
                throw new SyntaxError("Invalid body of the let special form");
            }
            var beginBody = VariableDefinitionExpressions().Append(BuiltIn.Car(tail)).ListOf();
            var begin = new ClCell(ClSymbol.Begin, beginBody);
            var lambda = BuiltIn.ListOf(ClSymbol.Lambda, ClCell.Nil, begin);
            return BuiltIn.ListOf(lambda).Reduce(ctx);
        }

        private IEnumerable<ClObj> VariableDefinitionExpressions()
        {
            var expressions = BuiltIn.Head(Cdr).Cast<ClCell>();
            var bindings = BuiltIn.Seq(expressions)
                .Select(expression => {
                    if (expression is not ClCell variableDefinition)
                    {
                        throw new SyntaxError("Invalid bindings format");
                    }
                    if (variableDefinition == ClCell.Nil)
                    {
                        throw new SyntaxError($"Variable definition expression cannot be {nameof(ClCell.Nil)}");
                    }
                    if (BuiltIn.Cddr(variableDefinition) != ClCell.Nil)
                    {
                        throw new SyntaxError("Variable definition expression should have format (var val)");
                    }
                    return variableDefinition;
                });
            return bindings
                .Select(x => BuiltIn.ListOf(ClSymbol.Define, BuiltIn.First(x), BuiltIn.Second(x)));
        }
    }
}
