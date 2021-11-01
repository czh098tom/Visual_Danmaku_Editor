using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Graph
{
    public class Link
    {
        public Block Block0 { get; internal set; }
        public int Pin0 { get; internal set; }
        public Block Block1 { get; internal set; }
        public int Pin1 { get; internal set; }
    }
}
