using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models.AdvancedRepeat
{
    public class AdvancedRepeatModel : LinkedList<VariableModelBase>
    {
        public string Times { get; set; } = "6";
    }
}
