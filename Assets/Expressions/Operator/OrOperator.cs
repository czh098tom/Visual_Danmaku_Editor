﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String("||")]
    internal class OrOperator : SimpleOperator
    {
        public override int Priority => 9;

        protected override float CalculateWithNumberOnly(IReadOnlyList<float> parameters)
        {
            return parameters[0] != 0 || parameters[1] != 0 ? 1 : 0;
        }
    }
}
