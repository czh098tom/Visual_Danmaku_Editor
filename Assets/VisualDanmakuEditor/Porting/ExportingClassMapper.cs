using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

using Latticework.Reflection.Utilities;
using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor.Porting
{
    public class ExportingClassMapper
    {
        private static Dictionary<Type, List<ExportingClassMapper>> mapperCache = null;

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

        public static IReadOnlyList<ExportingClassMapper> GetMappersOfType(Type t)
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
                var arr = JsonConvert.DeserializeObject<ExportingClassMapper[]>(sr.ReadToEnd(), settings);
                mapperCache = new Dictionary<Type, List<ExportingClassMapper>>();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!mapperCache.ContainsKey(arr[i].TargetType))
                    {
                        mapperCache.Add(arr[i].TargetType, new List<ExportingClassMapper>(1));
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

        [JsonProperty]
        public Type TargetType { get; private set; }

        [JsonProperty]
        public Type TerminationType { get; set; }

        [JsonProperty]
        public string[] FieldNames { get; set; }

        [JsonProperty]
        public string Format { get; set; }

        public string Bind(object o)
        {
            Type t = o.GetType();

            string[] binded = new string[FieldNames.Length];

            for (int i = 0; i < FieldNames.Length; i++)
            {
                IMemberWithAccessor iam = t.GetAccessorsWithName(FieldNames[i], TerminationType
                    , BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .First();
                binded[i] = iam.GetValue(o).ToString();
                if (iam.Type == typeof(bool))
                {
                    binded[i] = binded[i].ToLower();
                }
            }

            return string.Format(Format, binded);
        }
    }
}
