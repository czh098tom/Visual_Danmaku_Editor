using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String(",")]
    public class CommaOperator : OperatorBase
    {
        public override int Priority => 10;

        public override void Calculate(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers)
        {
            result.Push(parameters[0]);
            result.Push(parameters[1]);
        }

        public override IEnumerable<ExpressionCalculatingException> OperandCounter(Stack<float> values, Stack<string> identifiers)
        {
            for(int i = 0; i < 2; i++)
            {
                if (values.Count > 0)
                {
                    yield return null;
                }
                else
                {
                    yield return new ExpressionCalculatingException($"Argument count for operator "
                        + $"\"{(GetType().GetCustomAttributes(false).First((a) => a is StringAttribute) as StringAttribute).String}\""
                        + $" is not sufficient, required 2, actual {1 - i}.", null);
                }
            }
        }
    }
}
