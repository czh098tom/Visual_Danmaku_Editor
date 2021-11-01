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
        public BulletModelBase BulletModel { get; set; } = new BulletModelBase();

        public string Interval
        {
            get => First.Value.Interval;
            set => First.Value.Interval = value;
        }
        public string Interval2
        {
            get => First?.Next?.Value.Interval ?? "0";
            set
            {
                if (First != null && First.Next != null) First.Next.Value.Interval = value;
            }
        }

        public IEnumerable<PredictableBulletModel> GetPredictableBulletModels()
        {
            return new TaskIntepreter(this).GetPredictableBulletModels();
        }
    }
}
