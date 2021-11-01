using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Latticework.Graph
{
    public class Block : INamed
    {
        public static Block FromDefinition(BlockDefinition blockDefinition)
        {
            return new Block()
            {
                TypeName = blockDefinition.TypeName,
                X = 0,
                Y = 0,
                Literals = new string[blockDefinition.Pins.Length]
            };
        }

        public string TypeName { get; private set; }
        [JsonIgnore]
        public BlockDefinition ReferredDefinition => this.ReferredDefinition<BlockDefinition>();

        public float X { get; set; }
        public float Y { get; set; }
        public string[] Literals { get; set; }
    }
}
