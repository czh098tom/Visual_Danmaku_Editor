using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("<=")]
    internal class NotBiggerThanOperator : SimpleOperator
    {
        public override int Priority => 3;

        protected override float CalculateWithNumberOnly(IReadOnlyList<float> parameters)
        {
            return parameters[1] <= parameters[0] ? 1 : 0;
        }
    }
}
