using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor.IO
{
    public class SaveModel
    {
        [JsonProperty] public FixedObject Object { get; private set; }
        [JsonProperty] public TaskModelIO[] TaskModels { get; private set; }

        public SaveModel() { }
        public SaveModel(FixedObject @object, IEnumerable<TaskModel> taskModels)
        {
            Object = @object;
            TaskModels = taskModels.Select(t => new TaskModelIO(t)).ToArray();
        }
    }
}
