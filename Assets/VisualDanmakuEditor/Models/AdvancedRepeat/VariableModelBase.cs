using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat
{
    public abstract class VariableModelBase
    {
        public static readonly Regex scalingRegexp = new Regex(@"^\s*(?<const>[\+-]?\d*\.?\d*)\s*\*\s*\((?<expr>.*)\)\s*$");

        public string VariableName { get; set; } = "";

        public abstract float GetVariableValue(int currentIndex, int total, Func<string, float> getValue);

        public virtual void AdjustDensity(int before, int after) { }
        public virtual void AdjustLength(int before, int after, float pin) { }

        public static string Scale(string exp, float prop)
        {
            string s = exp;
            Match m = scalingRegexp.Match(s);
            if (m.Success)
            {
                return $"{prop * Convert.ToSingle(m.Groups["const"].Value)}*({m.Groups["expr"].Value})";
            }
            else
            {
                return $"{prop}*({s})";
            }
        }
    }
}
