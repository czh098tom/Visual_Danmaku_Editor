using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.AdvancedRepeat
{
    public abstract class VariableModelBase
    {
        public string VariableName { get; set; } = "";

        public abstract float GetVariableValue(int currentIndex, int total, Func<string, float> getValue);
    }
}
