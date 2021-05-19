using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("/")]
    internal class DivisionOperator : SimpleOperator
    {
        public override int Priority => 1;

        protected override float CalculateWithNumberOnly(IReadOnlyList<float> parameters)
        {
            return parameters[0] / parameters[1];
        }
    }
}
