using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor.Models
{
    public class TaskModel : LinkedList<AdvancedRepeatModel>
    {
        public string VelocityExpression { get; set; } = "3";
        public string RotationExpression { get; set; } = "0";
        public string XExpression { get; set; } = "0";
        public string YExpression { get; set; } = "0";

        public int Interval { get; set; }

        public IEnumerable<PredictableBulletModel> GetPredictableBulletModels()
        {
            return new TaskIntepreter()
            {
                Root = First,
                VelocityExpression = VelocityExpression,
                RotationExpression = RotationExpression,
                XExpression = XExpression,
                YExpression = YExpression,
                Interval = Interval
            }.GetPredictableBulletModels();
        }
    }
}
