using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

using Latticework.Reflection.Utilities;

namespace VisualDanmakuEditor.Porting
{
    public class DefaultExportingClassMapper : ExportingClassMapperBase
    {
        [JsonProperty]
        public Type TargetType { get; private set; }

        [JsonProperty]
        public Type TerminationType { get; private set; }

        [JsonProperty]
        public string[] FieldNames { get; private set; }

        [JsonProperty]
        public string Format { get; private set; }

        public override string Bind(object o)
        {
            return string.Format(Format, GetFieldArray(o, FieldNames));
        }

        protected string[] GetFieldArray(object o, string[] fieldNames)
        {
            string[] binded = new string[fieldNames.Length];

            for (int i = 0; i < fieldNames.Length; i++)
            {
                binded[i] = GetField(o, fieldNames[i]);
            }

            return binded;
        }

        protected string GetField(object o, string fieldName)
        {
            IMemberWithAccessor iam = o.GetType().GetAccessorsWithName(fieldName, TerminationType
                , BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .First();
            string binded = iam.GetValue(o).ToString();
            if (iam.Type == typeof(bool))
            {
                binded = binded.ToLower();
            }
            return binded;
        }
    }
}
