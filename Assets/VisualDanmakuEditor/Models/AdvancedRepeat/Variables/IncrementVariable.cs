using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat.Variables
{
    public class IncrementVariable : VariableModelBase
    {
        public string Begin { get; set; } = "0";
        public string Increment { get; set; } = "5";

        public override float GetVariableValue(int currentIndex, int total, Func<string, float> getValue)
        {
            float begin = new Expression(Begin).Calculate(getValue);
            float increment = new Expression(Increment).Calculate(getValue);
            return begin + currentIndex * increment;
        }

        public override void AdjustDensity(int before, int after)
        {
            Increment = Scale(Increment, after / (float)before);
        }
    }
}
