﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("||")]
    internal class OrOperator : OperatorBase
    {
        public override int Priority => 9;

        public override float Calculate(params float[] parameters)
        {
            return parameters[0] != 0 || parameters[1] != 0 ? 1 : 0;
        }
    }
}
