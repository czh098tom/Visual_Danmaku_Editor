using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("-")]
    internal class SubstractionOperator : SimpleOperator
    {
        public override int Priority => 2;

        protected override float CalculateWithNumberOnly(IReadOnlyList<float> parameters)
        {
            return parameters[1] - parameters[0];
        }
    }
}
