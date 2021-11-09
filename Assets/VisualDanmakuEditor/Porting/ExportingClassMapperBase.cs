using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor.Porting
{
    public abstract class ExportingClassMapperBase
    {
        private static Dictionary<Type, List<ExportingClassMapperBase>> mapperCache = null;

        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new List<JsonConverter>()
            {
                new TypeToStringConverter(null, typeof(TaskModel).Assembly.FullName, typeof(List<>).Assembly.FullName
                    , typeof(LinkedList<>).Assembly.FullName)
            }
        };

        public static IReadOnlyList<ExportingClassMapperBase> GetMappersOfType(Type t)
        {
            if (mapperCache == null) EstablishMapping();
            return mapperCache[t];
        }

        public static void EstablishMapping()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(Path.Combine(Application.streamingAssetsPath, "exportingMapping.json"));
                var arr = JsonConvert.DeserializeObject<DefaultExportingClassMapper[]>(sr.ReadToEnd(), settings);
                mapperCache = new Dictionary<Type, List<ExportingClassMapperBase>>();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!mapperCache.ContainsKey(arr[i].TargetType))
                    {
                        mapperCache.Add(arr[i].TargetType, new List<ExportingClassMapperBase>(1));
                    }
                    mapperCache[arr[i].TargetType].Add(arr[i]);
                }
            }
            finally
            {
                sr?.Close();
            }
        }

        public static string[] BindExportString(object o)
        {
            return GetMappersOfType(o.GetType()).Select((m) => m.Bind(o)).ToArray();
        }

        public abstract string Bind(object o);
    }
}
