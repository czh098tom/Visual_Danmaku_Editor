using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    internal abstract class SimpleOperator : SingleResultOperator
    {
        public virtual int NumOfOperands => 2;

        public override sealed IEnumerable<ExpressionCalculatingException> OperandCounter(Stack<float> values, Stack<string> identifiers)
        {
            for (int i = 0; i < NumOfOperands; i++)
            {
                if (values.Count > 0)
                {
                    yield return null;
                }
                else
                {
                    yield return new ExpressionCalculatingException($"Argument count for operator "
                        + $"\"{(GetType().GetCustomAttributes(false).First((a) => a is StringAttribute) as StringAttribute).String}\""
                        + $" is not sufficient, required {NumOfOperands}, actual {NumOfOperands - i - 1}.", null);
                }
            }
        }

        protected override sealed float CalculateWithSignleResult(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers)
        {
            return CalculateWithNumberOnly(parameters);
        }

        protected abstract float CalculateWithNumberOnly(IReadOnlyList<float> parameters);
    }
}
