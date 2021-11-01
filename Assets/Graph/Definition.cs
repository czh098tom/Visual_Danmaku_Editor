using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Latticework.Graph
{
    public static class Definition<T> where T : INamed
    {
        private static readonly Dictionary<string, T> definitions = new Dictionary<string, T>();

        public static T Get(string name)
        {
            return definitions[name];
        }

        public static void LoadDefinition(string json)
        {
            T[] def = JsonConvert.DeserializeObject<T[]>(json);
            for (int i = 0; i < def.Length; i++)
            {
                definitions.Add(def[i].TypeName, def[i]);
            }
        }
    }
}
