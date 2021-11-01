using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Latticework.Graph
{
    public class BlockDefinition : INamed
    {
        public class Pin
        {
            public enum Placing { Left, Right }

            public string TypeName { get; private set; }
            public bool AllowMultiple { get; private set; }
            public Placing PinPlacing { get; private set; }
        }

        [JsonProperty]
        public string TypeName { get; private set; }
        [JsonProperty]
        public Pin[] Pins { get; private set; }
    }
}
