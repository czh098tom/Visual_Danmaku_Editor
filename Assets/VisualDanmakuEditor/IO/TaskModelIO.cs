using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor.IO
{
    public class TaskModelIO
    {
        [JsonProperty] public AdvancedRepeatModelIO[] Repeats { get; private set; }
        [JsonProperty] public int BeginTime { get; private set; }
        [JsonProperty] public BulletModelBase BulletModel { get; private set; }
        [JsonProperty] public ObjectModelBase Shooter { get; private set; }

        public TaskModelIO() { }

        public TaskModelIO(TaskModel task)
        {
            Repeats = task.Select(r => new AdvancedRepeatModelIO(r)).ToArray();
            BeginTime = task.BeginTime;
            BulletModel = task.BulletModel;
            Shooter = task.Shooter;
        }

        public TaskModel ToTaskModel()
        {
            var model = new TaskModel()
            {
                BeginTime = BeginTime,
                BulletModel = BulletModel,
                Shooter = Shooter,
            };
            foreach(var item in Repeats)
            {
                model.AddLast(new LinkedListNode<Models.AdvancedRepeat.AdvancedRepeatModel>(item.ToModel()));
            }
            return model;
        }
    }
}
