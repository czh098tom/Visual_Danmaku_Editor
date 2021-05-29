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
        public int Interval2 { get; set; }

        public string Style { get; set; } = "ball_mid";
        public string Color { get; set; } = "COLOR_RED";

        public IEnumerable<PredictableBulletModel> GetPredictableBulletModels()
        {
            return new TaskIntepreter(this).GetPredictableBulletModels();
        }
    }
}
