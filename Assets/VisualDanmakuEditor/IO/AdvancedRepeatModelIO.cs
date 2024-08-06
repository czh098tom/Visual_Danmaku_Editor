using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor.IO
{
    public class AdvancedRepeatModelIO
    {
        [JsonProperty] public VariableModelBase[] VariableModels { get; private set; }
        [JsonProperty] public string Times { get; private set; }
        [JsonProperty] public string Interval { get; private set; }

        public AdvancedRepeatModelIO() { }

        public AdvancedRepeatModelIO(AdvancedRepeatModel model)
        {
            VariableModels = model.ToArray();
            Times = model.Times;
            Interval = model.Interval;
        }

        public AdvancedRepeatModel ToModel()
        {
            var model = new AdvancedRepeatModel()
            {
                Times = Times,
                Interval = Interval,
            };
            foreach (var item in VariableModels)
            {
                model.AddLast(new LinkedListNode<VariableModelBase>(item));
            }
            return model;
        }
    }
}
