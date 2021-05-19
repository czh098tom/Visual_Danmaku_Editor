using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("!")]
    internal class NotOperator : SimpleOperator
    {
        public override int Priority => 0;

        public override int NumOfOperands => 1;

        protected override float CalculateWithNumberOnly(IReadOnlyList<float> parameters)
        {
            return parameters[0] == 0 ? 1 : 0;
        }
    }
}
