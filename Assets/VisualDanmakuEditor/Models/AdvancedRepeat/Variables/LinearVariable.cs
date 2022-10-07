using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat.Variables
{
    public class LinearVariable : VariableModelBase
    {
        private static readonly Regex linearForm
            = new Regex(@"^\s*(?<const1>[\+-]?\d*\.?\d*)\s*\*\s*\((?<expr1>.*)\)\s*\+\s*(?<const2>[\+-]?\d*\.?\d*)\s*\*\s*\((?<expr2>.*)\)\s*$");

        private static readonly float epsilon = 1e-6f;

        public string Begin { get; set; } = "0";
        public string End { get; set; } = "0";

        public bool IsPrecisely { get; set; } = false;

        public override float GetVariableValue(int currentIndex, int total, Func<string, float> getValue)
        {
            float beg = new Expression(Begin).Calculate(getValue);
            float end = new Expression(End).Calculate(getValue);
            if (IsPrecisely) total--;
            return (end - beg) * (currentIndex / (float)total) + beg;
        }

        public override void AdjustLength(int before, int after, float pin)
        {
            float prop;
            if (!IsPrecisely)
            {
                prop = after / (float)before;
            }
            else
            {
                prop = (after - 1) / (float)(before - 1);
            }

            Match formBeg = linearForm.Match(Begin);
            Match formEnd = linearForm.Match(End);

            string oldBeg = Begin;
            string oldEnd = End;

            if (formBeg.Success && formEnd.Success
                && formBeg.Groups["expr1"].Value == formEnd.Groups["expr1"].Value
                && formBeg.Groups["expr2"].Value == formEnd.Groups["expr2"].Value
                && MathF.Abs(Convert.ToSingle(formBeg.Groups["const1"].Value) + Convert.ToSingle(formBeg.Groups["const2"].Value) - 1) < epsilon
                && MathF.Abs(Convert.ToSingle(formEnd.Groups["const1"].Value) + Convert.ToSingle(formEnd.Groups["const2"].Value) - 1) < epsilon)
            {
                float a1x = Convert.ToSingle(formBeg.Groups["const1"].Value);
                float a2x = Convert.ToSingle(formEnd.Groups["const1"].Value);
                oldBeg = formBeg.Groups["expr1"].Value;
                oldEnd = formBeg.Groups["expr2"].Value;
                if (MathF.Abs(a1x - a2x - 1) > epsilon)
                {
                    float t = (a1x - 1) / (a1x - a2x - 1);
                    float k = 1 / (a1x - a2x);
                    prop /= k;
                    pin = t - t / k + pin / k;
                }
            }
            float a1 = pin - pin * prop;
            float a2 = pin - (pin - 1) * prop;
            if (MathF.Abs(a1) > epsilon || MathF.Abs(a2 - 1) > epsilon)
            {
                Begin = $"{1 - a1:f7}*({oldBeg})+{a1:f7}*({oldEnd})";
                End = $"{1 - a2:f7}*({oldBeg})+{a2:f7}*({oldEnd})";
            }
            else
            {
                Begin = oldBeg;
                End = oldEnd;
            }
        }
    }
}
