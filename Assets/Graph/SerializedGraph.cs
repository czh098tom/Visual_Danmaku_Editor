using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Graph
{
    internal class SerializedGraph
    {
        internal List<Block> Blocks { get; set; }
        internal List<SerializedLink> Links { get; set; }

        internal SerializedGraph(Graph graph)
        {
            Blocks = graph.Blocks;
            Links = graph.Links.Select(l =>
                new SerializedLink()
                {
                    Block0 = Blocks.IndexOf(l.Block0),
                    Block1 = Blocks.IndexOf(l.Block1),
                    Pin0 = l.Pin0,
                    Pin1 = l.Pin1
                }).ToList();
        }
    }
}
