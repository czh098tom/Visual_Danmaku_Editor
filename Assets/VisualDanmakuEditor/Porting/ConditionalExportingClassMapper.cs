using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VisualDanmakuEditor.Porting
{
    public class ConditionalExportingClassMapper : DefaultExportingClassMapper
    {
        [JsonProperty]
        public string FormatSelector { get; private set; }

        [JsonProperty]
        public string FormatNegative { get; private set; }

        public override string Bind(object o)
        {
            if (GetField(o, FormatSelector) == "true")
            {
                return base.Bind(o);
            }
            else
            {
                return string.Format(FormatNegative, GetFieldArray(o, FieldNames));
            }
        }
    }
}
