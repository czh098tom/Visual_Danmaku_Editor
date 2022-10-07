using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat
{
    public class AdvancedRepeatModel : LinkedList<VariableModelBase>
    {
        public string Times { get; set; } = "6";
        public string Interval { get; set; } = "0";

        public void AdjustLength(int amount)
        {
            if (int.TryParse(Times, out int t))
            {
                foreach (VariableModelBase vmb in this)
                {
                    vmb.AdjustLength(t, t + amount, 0);
                }
                AddTimes(amount);
            }
        }

        public void AdjustDensity(int amount)
        {
            if (int.TryParse(Times, out int t))
            {
                foreach (VariableModelBase vmb in this)
                {
                    vmb.AdjustDensity(t, t + amount);
                }
                AddTimes(amount);
            }
        }

        private void AddTimes(int amount)
        {
            Times = $"{Convert.ToInt32(Times) + amount}";
        }
    }
}
