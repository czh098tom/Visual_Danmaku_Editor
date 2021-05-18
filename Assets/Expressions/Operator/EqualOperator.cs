using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("==")]
    internal class EqualOperator : OperatorBase
    {
        public override int Priority => 4;

        protected override float Calculate(params float[] parameters)
        {
            return parameters[0] == parameters[1] ? 1 : 0;
        }
    }
}
