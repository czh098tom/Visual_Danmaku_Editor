using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDanmakuEditor.Models.AdvancedRepeat
{
    public class AdvancedRepeatModel : LinkedList<VariableModelBase>
    {
        public int Times { get; set; } = 10;
    }
}
