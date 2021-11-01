using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Latticework.Graph
{
    public class PinDefinition : INamed
    {
        public static bool Validate(PinDefinition @in, PinDefinition @out)
        {
            return @in.AllowedTypes.Contains(@out.TypeName) || @out.AllowedTypes.Contains(@in.TypeName);
        }

        [JsonProperty]
        public string TypeName { get; private set; }
        [JsonProperty]
        public string[] AllowedTypes { get; private set; }
        [JsonProperty]
        public bool CanBeLiteral { get; private set; }

        public Pin CreatePin()
        {
            return new Pin
            {
                TypeName = TypeName
            };
        }
    }
}
