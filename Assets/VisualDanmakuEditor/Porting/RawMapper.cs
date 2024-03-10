using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace VisualDanmakuEditor.Porting
{
    public class RawMapper : DefaultExportingClassMapper
    {
        public override string Bind(object o)
        {
            return Format;
        }
    }
}
