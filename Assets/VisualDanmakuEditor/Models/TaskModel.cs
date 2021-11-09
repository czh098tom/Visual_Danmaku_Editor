using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor.Models
{
    public class TaskModel : LinkedList<AdvancedRepeatModel>
    {
        public string Name { get; set; } = "Task";

        public int BeginTime { get; set; }
        public BulletModelBase BulletModel { get; set; }
        public ObjectModelBase Shooter { get; set; }

        public IEnumerable<PredictableBulletModelBase> GetPredictableBulletModels()
        {
            return new TaskIntepreter(this).GetPredictableBulletModels();
        }
    }
}
