using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Graph
{
    public class Graph
    {
        internal List<Block> Blocks { get; } = new List<Block>();
        internal List<Link> Links { get; } = new List<Link>();

        public IReadOnlyList<Block> BlockList => Blocks;
        public IReadOnlyList<Link> LinkList => Links;

        public Graph() { }

        internal Graph(SerializedGraph serializedGraph)
        {
            Blocks = serializedGraph.Blocks;
            Links = serializedGraph.Links.Select(l =>
                new Link()
                {
                    Block0 = Blocks[l.Block0],
                    Block1 = Blocks[l.Block1],
                    Pin0 = l.Pin0,
                    Pin1 = l.Pin1
                }).ToList();
        }

        public IEnumerable<Link> LinksOnPin(Block block, int pin)
        {
            return Links.Where(l => (l.Block0 == block && l.Pin0 == pin) || (l.Block1 == block && l.Pin1 == pin));
        }

        public Link LinkBetween(Block block0, int pin0, Block block1, int pin1)
        {
            return Links.FirstOrDefault(l => (l.Block0 == block0 && l.Pin0 == pin0 && l.Block1 == block1 && l.Pin1 == pin1)
                || (l.Block1 == block0 && l.Pin1 == pin0 && l.Block0 == block1 && l.Pin0 == pin1));
        }

        public Block AddBlock(string typeName)
        {
            Block b = Block.FromDefinition(Definition<BlockDefinition>.Get(typeName));
            Blocks.Add(b);
            return b;
        }

        public Link AddLink(Block block0, int pin0, Block block1, int pin1)
        {
            Link l = new Link() { Block0 = block0, Block1 = block1, Pin0 = pin0, Pin1 = pin1 };
            Links.Add(l);
            return l;
        }

        public Link RemoveLink(Link l)
        {
            Links.Remove(l);
            return l;
        }

        public Link RemoveLink(Block block0, int pin0, Block block1, int pin1) => RemoveLink(LinkBetween(block0, pin0, block1, pin1));
    }
}
