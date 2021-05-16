﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("/")]
    internal class DivisionOperator : OperatorBase
    {
        public override int Priority => 1;

        public override float Calculate(params float[] parameters)
        {
            return parameters[0] / parameters[1];
        }
    }
}
