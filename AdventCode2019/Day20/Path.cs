using AdventCode2019.Day3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day20
{
    public class Path
    {
        public bool JustWentThrough { get; set; }

        public int Length { get { return Nodes.Count; } }

        public int Level { get; set; }

        public LinkedList<(Node node, int level)> Nodes { get; private set; }

        public Node CurrentPosition { get { return Nodes.Last.Value.node; } }

        public Path(int level)
        {
            Level = level;
            Nodes = new LinkedList<(Node, int)>();
            JustWentThrough = false;
        }

        public Path(Path parent)
        {
            Level = parent.Level;
            Nodes = new LinkedList<(Node, int)>(parent.Nodes);
            JustWentThrough = parent.JustWentThrough;
        }

        public void Add(Node node)
        {
            //Nodes.AddLast((node, Level));
            if (node.GoesDeeper.HasValue)
            {
                if (JustWentThrough)
                {
                    JustWentThrough = false;
                    if (node.GoesDeeper == true)
                        Level--;
                    else
                        Level++;

                    //Console.WriteLine($"Path {this.ID} went through portal {node.GetAdjacentTeleport()}");
                }
                else
                {
                    JustWentThrough = true;                    
                }
            }
            Nodes.AddLast((node, Level));
        }
    }
}
