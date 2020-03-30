using AdventCode2019.Day3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day20
{
    public enum NodeType
    {
        PATH,
        TELEPORT,
        START,
        END
    }
    public class Node
    {
        public static Dictionary<Coordinate, Node> Field { get; private set; } = new Dictionary<Coordinate, Node>();

        public static Dictionary<Coordinate, Node> Teleports { get; private set; } = new Dictionary<Coordinate, Node>();

        public static Dictionary<string, List<Node>> TeleportIDs { get; private set; } = new Dictionary<string, List<Node>>();

        public static Node Start { get; private set; }

        public static Node End { get; private set; }

        public Coordinate Position { get; private set; }

        public List<Node> Edges { get; private set; } = new List<Node>();

        public NodeType Type { get; private set; }

        public char Symbol { get; private set; }

        public Node Soulmate { get; private set; }

        public int Distance { get; set; } = int.MaxValue;

        public bool? GoesDeeper { get; set; } = null;

        public Node(int x, int y, NodeType type, char symbol)
        {
            Position = new Coordinate(x, y);
            Type = type;
            Symbol = symbol;
            List<Coordinate> sides = GetSides(x, y);

            if (type == NodeType.PATH)
            {
                Field.Add(Position, this);

                foreach (var side in sides)
                {
                    if (Field.ContainsKey(side))
                    {
                        AddEdge(Field[side]);
                    }
                }
            }
            else
            {
                Teleports.Add(Position, this);

                foreach (var side in sides)
                {
                    if (Teleports.ContainsKey(side))
                    {
                        Node teleport = Teleports[side];
                        if (symbol == 'A' && teleport.Symbol == 'A')
                        {
                            Type = NodeType.START;
                            teleport.Type = NodeType.START;
                            Soulmate = teleport;
                            teleport.Soulmate = this;
                        }
                        else if (symbol == 'Z' && teleport.Symbol == 'Z')
                        {
                            Type = NodeType.END;
                            teleport.Type = NodeType.END;
                            Soulmate = teleport;
                            teleport.Soulmate = this;
                        }
                        else
                        {
                            string telId = string.Concat(new[] { symbol, teleport.Symbol });

                            var pair = new List<Node> { this, teleport };
                            if (TeleportIDs.ContainsKey(telId))
                            {
                                TeleportIDs[telId].AddRange(pair);
                            }
                            else
                            {
                                TeleportIDs.Add(telId, pair);
                            }
                        }
                    }
                }
            }
        }

        // Calculates distance to surrounding nodes
        public void SearchEdges()
        {
            foreach (var edge in Edges)
            {
                if (Distance < edge.Distance)
                {
                    edge.Distance = Distance + 1;

                    if (edge != End)
                    {
                        edge.SearchEdges();
                    }
                }
            }
        }

        // Finds START and END node positions
        public static Node FindStartAndEnd(Node tp1, Node tp2)
        {
            List<Coordinate> sides = new List<Coordinate>();

            var (x1, y1) = (tp1.Position.X, tp1.Position.Y);
            var (x2, y2) = (tp2.Position.X, tp2.Position.Y);

            sides.AddRange(GetSides(x1, y1));
            sides.AddRange(GetSides(x2, y2));

            foreach (var side in sides)
            {
                if (Field.ContainsKey(side))
                {
                    var node = Field[side];
                    switch (tp1.Type)
                    {
                        case NodeType.TELEPORT:
                            break;
                        case NodeType.START:
                            Start = node;
                            Start.GoesDeeper = null;
                            break;
                        case NodeType.END:
                            End = node;
                            End.GoesDeeper = null;
                            break;
                        default:
                            break;
                    }
                    return node;
                }
            }
            return null;
        }

        // Joins respective teleport nodes
        public static void JoinTeleports(List<Node> tps)
        {
            List<Coordinate> sides1 = new List<Coordinate>();
            List<Coordinate> sides2 = new List<Coordinate>();

            var (tp1, tp2, tp3, tp4) = (tps[0], tps[1], tps[2], tps[3]);

            var (x1, y1) = (tp1.Position.X, tp1.Position.Y);
            var (x2, y2) = (tp2.Position.X, tp2.Position.Y);

            sides1.AddRange(GetSides(x1, y1));
            sides1.AddRange(GetSides(x2, y2));

            Node node1 = null, node2 = null;

            foreach (var side in sides1)
            {
                if (Field.ContainsKey(side))
                {
                    var node = Field[side];
                    switch (node.Type)
                    {
                        case NodeType.PATH:
                            node1 = node;
                            node1.GoesDeeper = tp1.GoesDeeper;
                            break;
                        default:
                            break;
                    }
                }
            }

            (x1, y1) = (tp3.Position.X, tp3.Position.Y);
            (x2, y2) = (tp4.Position.X, tp4.Position.Y);

            sides2.AddRange(GetSides(x1, y1));
            sides2.AddRange(GetSides(x2, y2));

            foreach (var side in sides2)
            {
                if (Field.ContainsKey(side))
                {
                    var node = Field[side];
                    switch (node.Type)
                    {
                        case NodeType.PATH:
                            node2 = node;
                            node2.GoesDeeper = tp3.GoesDeeper;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (node1 != null && node2 != null)
            {
                node1.AddEdge(node2);
            }
        }

        public void AddEdge(Node edge)
        {
            if (!Edges.Contains(edge))
            {
                Edges.Add(edge);
                edge.AddEdge(this);
            }
        }

        public string GetAdjacentTeleport()
        {
            var teleport = Teleports.FirstOrDefault(tel => Utilities.CalculateManhattanDistance(tel.Value.Position, Position) == 1);
            if (!teleport.Equals(default(KeyValuePair<Coordinate, Node>)))
                return TeleportIDs.First(tid => tid.Value.Contains(teleport.Value)).Key;
            else
                return string.Empty;
        }

        public override string ToString()
        {
            return Position.ToString();
        }

        private static List<Coordinate> GetSides(int x, int y)
        {
            return new List<Coordinate>
            {
                new Coordinate(x - 1, y),
                new Coordinate(x + 1, y),
                new Coordinate(x, y - 1),
                new Coordinate(x, y + 1),
            };
        }
    }
}
