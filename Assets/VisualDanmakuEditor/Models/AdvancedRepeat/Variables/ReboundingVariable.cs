using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat.Variables
{
    public class ReboundingVariable : VariableModelBase
    {
        public string First { get; set; } = "-1";
        public string Another { get; set; } = "1";

        public override float GetVariableValue(int currentIndex, int total, Func<string, float> getValue)
        {
            float first = new Expression(First).Calculate(getValue);
            float another = new Expression(Another).Calculate(getValue);
            return currentIndex % 2 == 0 ? first : another;
        }
    }
}
