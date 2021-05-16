using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat.Variables
{
    public class LinearVariable : VariableModelBase
    {
        public string Begin { get; set; } = "0";
        public string End { get; set; } = "0";

        public bool IsPrecisely { get; set; } = true;

        public override float GetVariableValue(int currentIndex, int total, Func<string, float> getValue)
        {
            float beg = new Expression(Begin).Calculate(getValue);
            float end = new Expression(End).Calculate(getValue);
            if (IsPrecisely) total--;
            return (end - beg) * (currentIndex / (float)total) + beg;
        }
    }
}
