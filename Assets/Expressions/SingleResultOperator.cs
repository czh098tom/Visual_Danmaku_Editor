using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    internal abstract class SingleResultOperator : OperatorBase
    {
        public override void Calculate(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers)
        {
            result.Push(CalculateWithSignleResult(parameters, identifiers));
        }

        protected abstract float CalculateWithSignleResult(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers);
    }
}
