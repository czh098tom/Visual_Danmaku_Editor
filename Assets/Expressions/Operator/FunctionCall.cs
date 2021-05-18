using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String(identifier)]
    internal class FunctionCall : OperatorBase
    {
        public const string identifier = "[call]";

        public override int Priority => 10;

        protected override float Calculate(params float[] parameters)
        {
            throw new NotImplementedException();
        }

        public override float CalculateWithIdentifiers(float[] parameters, string[] identifiers)
        {
            switch (identifiers[0])
            {
                case "sin":
                    return Convert.ToSingle(Math.Sin(parameters[1] * Math.PI / 180));
                case "cos":
                    return Convert.ToSingle(Math.Cos(parameters[1] * Math.PI / 180));
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
