using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor.IO
{
    public static class IOHelper
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        public static void Save(SaveModel saveModel, string path)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.Write(JsonConvert.SerializeObject(saveModel, settings));
        }

        public static SaveModel Load(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs);
            return JsonConvert.DeserializeObject<SaveModel>(sr.ReadToEnd(), settings);
        }
    }
}
