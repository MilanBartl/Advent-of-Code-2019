﻿using AdventCode2019.Day3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2019.Day20
{
    public class Worker : IWorker
    {
        private int width;
        private int height;

        private HashSet<int> pathIds = new HashSet<int>();

        public Worker()
        {
            var splits = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            width = splits[0].Length;
            height = splits.Length;

            for (int y = 0; y < splits.Length; y++)
            {
                var line = splits[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                    {
                        new Node(x, y, NodeType.PATH, '.');
                    }
                    else if (Regex.Match(line[x].ToString(), "[A-Z]").Success)
                    {
                        Node node = new Node(x, y, NodeType.TELEPORT, line[x]);

                        if (x <= 1 || y <= 1 || x >= width - 2 || y >= height - 2)
                            node.GoesDeeper = false;
                        else
                            node.GoesDeeper = true;
                    }
                }
            }

            foreach (var tp in Node.Teleports.Values)
            {
                if (tp.Soulmate != null)
                    Node.FindStartAndEnd(tp, tp.Soulmate);
            }

            foreach (var tp in Node.TeleportIDs.Values)
            {
                Node.JoinTeleports(tp);
            }
        }

        public object Work1()
        {
            Node.Start.Distance = 0;
            Node.Start.SearchEdges();

            var all = new Dictionary<Coordinate, Node>();
            all = all.Concat(Node.Field).ToDictionary(x => x.Key, x => x.Value);
            all = all.Concat(Node.Teleports).ToDictionary(x => x.Key, x => x.Value);

            DrawMap(all, width, height);

            return Node.End.Distance;
        }

        private Dictionary<(Node, int), int> nodeLevelDistance = new Dictionary<(Node, int), int>();
        private Dictionary<Coordinate, bool> deadEnds = new Dictionary<Coordinate, bool>();

        public object Work2()
        {
            var pathList = new List<Path>();
            var startPath = new Path(0);
            startPath.Add(Node.Start);
            pathList.Add(startPath);

            nodeLevelDistance.Add((Node.Start, 0), 1);

            var nodes = new Dictionary<Coordinate, Node>();
            nodes = nodes.Concat(Node.Field).ToDictionary(x => x.Key, x => x.Value);
            nodes = nodes.Concat(Node.Teleports).ToDictionary(x => x.Key, x => x.Value);

            while (pathList.All(path => path.CurrentPosition != Node.End || path.Level != 0) && pathList.Any())
            {
                var newPaths = new List<Path>();

                foreach (var path in pathList)
                {
                    var edges = path.CurrentPosition.Edges;//.Where(edge => !path.Nodes.Contains((edge, path.Level))).ToList();
                   
                    //var edges = path.CurrentPosition.Edges.Where(edge => !path.Nodes.Select(n => n.node).Contains(edge)).ToList();

                    if (edges.Count == 0)
                    {
                        //Console.WriteLine($"Dead end detected for path {pathId} at {path.CurrentPosition}, level {path.Level} with length {path.Length}. Removing the path.");
                        if (!deadEnds.ContainsKey(path.CurrentPosition.Position))
                            deadEnds.Add(path.CurrentPosition.Position, true);
                        continue;
                    }

                    foreach (var edge in edges)
                    {
                        if (edge.GoesDeeper.HasValue && path.CurrentPosition.GoesDeeper.HasValue && path.Nodes.Last.Previous.Value.node.GoesDeeper.HasValue)
                        {
                            //Console.WriteLine($"Skipped creating an edge {edge.Position} for path {pathId}, don't go back to the teleport!");
                            continue;
                        }

                        var newPath = new Path(path);
                        newPath.Add(edge);

                        // add path only when in non-negative level and not too deep
                        if (newPath.Level >= 0 && newPath.Level <= Node.TeleportIDs.Count)
                        {
                            if (nodeLevelDistance.ContainsKey((edge, newPath.Level)))
                            {
                                var previousDistance = nodeLevelDistance[(edge, newPath.Level)];

                                if (previousDistance < newPath.Length)
                                {
                                    //Console.WriteLine($"Removed path {newPath.ID} with distance {newPath.Length}, there is better distance {previousDistance} at level {newPath.Level}.");
                                    continue;
                                }
                                else
                                {
                                    nodeLevelDistance[(edge, newPath.Level)] = newPath.Length;
                                }
                            }
                            else
                                nodeLevelDistance.Add((edge, newPath.Level), newPath.Length);

                            newPaths.Add(newPath);
                        }                        
                        /*else
                            Console.WriteLine($"Removed path {newPath.ID} in level {newPath.Level} with distance {newPath.Length}");*/
                    }
                }

                pathList = newPaths;
            }

            DrawMap(nodes, width, height);

            if (pathList.Any())
            {
                var finishPath = pathList.First(path => path.CurrentPosition == Node.End && path.Level == 0);
                ShowPath(finishPath);
                return finishPath.Length - 1;  // don't count the start node
            }
            else
                return "End cannot be reached.";
        }

        private void ShowPath(Path path)
        {
            var current = path.Nodes.First;
            do
            {
                Console.WriteLine($"Pos: {current.Value.node.Position}, Level: {current.Value.level}");
                
                if (current.Value.node.GoesDeeper == true)
                {
                    Console.WriteLine($"Went through teleport {current.Value.node.GetAdjacentTeleport()}");
                }       

            } while ((current = current.Next) != null);
        }

        private void DrawMap(Dictionary<Coordinate, Node> map, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var coord = Coordinate.Create(x, y);
                    if (map.ContainsKey(coord))
                    {
                        Node node = map[coord];
                        ConsoleColor color = ConsoleColor.White;
                        switch (node.Type)
                        {
                            case NodeType.PATH:
                                break;
                            case NodeType.TELEPORT:
                                color = ConsoleColor.Blue;
                                break;
                            case NodeType.START:
                            case NodeType.END:
                                color = ConsoleColor.Yellow;
                                break;
                            default:
                                break;
                        }
                        if (node == Node.Start)
                            color = ConsoleColor.Green;
                        else if (node == Node.End)
                            color = ConsoleColor.Green;
                        else if (deadEnds.ContainsKey(coord))
                            color = ConsoleColor.Red;

                        WriteColor(node.Edges.Count.ToString(), color);
                    }
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        private void WriteColor(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ResetColor();
        }

        private const string test = @"         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       ";

        private const string largerTest = @"                   A               
                   A               
  #################.#############  
  #.#...#...................#.#.#  
  #.#.#.###.###.###.#########.#.#  
  #.#.#.......#...#.....#.#.#...#  
  #.#########.###.#####.#.#.###.#  
  #.............#.#.....#.......#  
  ###.###########.###.#####.#.#.#  
  #.....#        A   C    #.#.#.#  
  #######        S   P    #####.#  
  #.#...#                 #......VT
  #.#.#.#                 #.#####  
  #...#.#               YN....#.#  
  #.###.#                 #####.#  
DI....#.#                 #.....#  
  #####.#                 #.###.#  
ZZ......#               QG....#..AS
  ###.###                 #######  
JO..#.#.#                 #.....#  
  #.#.#.#                 ###.#.#  
  #...#..DI             BU....#..LF
  #####.#                 #.#####  
YN......#               VT..#....QG
  #.###.#                 #.###.#  
  #.#...#                 #.....#  
  ###.###    J L     J    #.#.###  
  #.....#    O F     P    #.#...#  
  #.###.#####.#.#####.#####.###.#  
  #...#.#.#...#.....#.....#.#...#  
  #.#####.###.###.#.#.#########.#  
  #...#.#.....#...#.#.#.#.....#.#  
  #.###.#####.###.###.#.#.#######  
  #.#.........#...#.............#  
  #########.###.###.#############  
           B   J   C               
           U   P   P               ";

        private const string multilevelTest = @"             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     ";

        private const string input = @"                                         J       F           Z     S   X           H     T                                             
                                         Q       V           V     K   P           K     D                                             
  #######################################.#######.###########.#####.###.###########.#####.###########################################  
  #...#...#.#...........#.#.....#.#.....#...#.#.#.....#.#...#...#...#.#...#.........#.#...................#.#.#.#...#.#.....#.......#  
  ###.###.#.#######.#####.#.#.#.#.###.#.###.#.#.#.###.#.###.#.#####.#.#.#.#######.###.#.#.#.###.###.#.###.#.#.#.###.#.###.#.###.###.#  
  #.#.....#.#.#.#.#...#.#...#.#.....#.#.....#.......#.#.........#...#...#.#.....#.....#.#.#.#.#...#.#.#.#.......#...#.....#.#.....#.#  
  #.#####.#.#.#.#.#.###.#########.#.###.#.#####.#######.#.#########.###.#####.#.#####.#.#####.#########.#.#######.#.###.#####.#.#.#.#  
  #.#...#.....#...#...#.....#...#.#.....#.#.#.......#...#.#...#.....#.#.......#.#.....#.#...........#...#.#.#.....#.#.......#.#.#.#.#  
  #.###.#.###.###.###.###.###.#####.###.###.###.#########.#.#.#.#.###.#######.#####.###.#.#.###########.###.#.#######.###.###.#####.#  
  #...#.#.#.#...#.#.....#...#.........#.....#.#...#.........#.#.#.....#.........#.#.#.#...#...#...#.......#.....#...#.#.#.#.#.#.#.#.#  
  ###.#.###.###.#.###.###.###############.###.#.###.#.###.###.#######.#.###.#####.#.#.#.#.###.#.#######.###.#####.###.#.###.#.#.#.###  
  #.#.................#.#.#...#...#.........#...#.#.#.#...#.....#.#...#.#.....#.#.....#.#.#.......#.#.......#.....#.#.#.............#  
  #.#################.#.#.###.#.#####.#.###.###.#.###.#.#.###.###.#.###.#######.#.###.#####.#######.###.###.###.###.#.#.###.###.#####  
  #.#...#...#...#.#...............#.#.#.#...#.......#.#.#...#.#.......#.......#...#...#...#...............#.......#.......#.#.....#.#  
  #.###.#.#####.#.#.#####.#####.###.#####.#####.#.###########.###.#.#.#.#######.###.#.#.#.###.#####.###.#########.###.#.###########.#  
  #...#.....#.#.....#...#.#.#.#.............#...#.#.#...#.#...#...#.#.#.#...#...#...#.#.#.........#...#.....#...#.#...#...#.#.#.#...#  
  #.###.###.#.#####.###.#.#.#.###.#######.#.###.###.###.#.###.###.###.#.#.#########.#.###.#.###.###.#####.###.#######.#####.#.#.###.#  
  #...#...#.#.#...#.#.....#.............#.#...#.......#...#.....#.#...#.....#...#.#.#.#...#.#.....#.....#.....#.....#.#.#...#.#.....#  
  ###.#.#####.###.###.#.#.###.#.#.#######.###.###.###.#.#.###.#.#####.###.###.###.#.#.#.#######.###.#.###########.###.#.###.#.###.###  
  #.#...#.#...........#.#.#...#.#.#...#...#...#.#...#.#.#.....#.#.#...#...........#.#.#...#.#.....#.#.....#.#...#.#.....#.#...#.#...#  
  #.#.###.#.#####.#.#.###.#########.#####.###.#.#.###########.###.###.#.#.#.###.#####.#.###.###########.###.###.#.###.###.#.###.#.###  
  #.#.....#.#.....#.#.#.......#.....#...#.#...#.....#.....#.#.....#...#.#.#...#.....#.#.#.#...#...#.#.....#.#.......#.........#.#.#.#  
  #.#.###########################.#####.###.#.#.#########.#.#.#######.#.#######.#.###.#.#.#.###.#.#.#.#.###.###.#####.#.###.###.#.#.#  
  #.....#.....#.#...#.....#.#...#.....#.#.#.#.#...#...#.#...#...#.....#.....#.#.#.#...#...#.....#...#.#.#.....#.#.#.#.#...#.#.#.....#  
  #.#.###.#####.#.#.#####.#.###.###.###.#.###.#.###.###.#.#.#.#######.#.#.###.#.#####.###.#.###############.###.#.#.###.#####.###.###  
  #.#.....#...#...#.....#.#.....#...#.#...#...#.......#...#...#.......#.#.#.#...#...#.#.....#.#.#.....#.#.................#...#.#...#  
  ###.#######.#####.#.#.#.#####.#.#.#.###.###.#.#.###.#.#######.###.###.###.###.#.#.#.#.#.###.#.###.###.#######.###.#####.###.#.#.###  
  #...#.#.#...#.#.#.#.#.....#.#.#.#...#.....#.#.#...#.#.....#.#.#...#.....#...#.#.#...#.#.#.#.......#.....#.#.....#.....#.#.....#.#.#  
  #.#.#.#.#.###.#.#########.#.#.#####.###.###.#.#.###.#.#.#.#.#####.###.#.###.#####.###.###.#.###.#.###.#.#.###.#############.###.#.#  
  #.#.#.#...#.#.#.....#.............#.#.....#.#.#.#...#.#.#.....#.....#.#.....#...#...#...#.#.#.#.#...#.#.#...#.#...#.#...#.....#...#  
  #.###.#.###.#.###.#####.###.#####.#.#####.#.###.###.#######.#######.###.###.###.###.#.###.#.#.###.#####.###.#.###.#.###.###.###.#.#  
  #...#.....#...#.#.....#.#.#...#.#.............#.#.....#...#.....#.#...#.#...#...#...#.......#...#.#.#...#.....#.#...#.....#...#.#.#  
  #.#######.###.#.###.#####.#####.#####.#.###.###.###.#####.#.#####.#.#######.#.#.#.#####.###.#.#####.###.#.#####.###.###.#####.###.#  
  #...#.#.....#.....#...#...#...#.#.....#.#...#.#.#...#.......#...#.#...#.....#.#.#...#...#...#...#...#.#.....#.#.#.#.#...#...#...#.#  
  #.###.###.###.#######.#.#####.#.###########.#.#.###.#.###.###.#.#.#.#######.#.#.###.###.#.###.###.###.#.###.#.#.#.#.###.#.###.###.#  
  #.#.#.............#.#...#.#.#...#.#...........#.#...#...#...#.#.........#...#.#.......#.#.......#.........#.....#.#...#...........#  
  #.#.#.#####.#.#.#.#.#.#.#.#.#.###.#####.#########.#####.#####.###########.###.#############.#######.#.###########.###.#######.#.###  
  #...#.#...#.#.#.#.#...#.#.#...#...#    J         R     L     Y           L   I             X    #.#.#.#.#.#.#...#.#...#.....#.#.#.#  
  #.###.#.###########.#####.###.#.###    N         P     P     L           S   B             P    #.###.#.#.#.#.###.#.#######.#.###.#  
  #...#.#...........#.#.#.#...#...#.#                                                             #.#.....#.........#.....#.#...#.#..AA
  #.###.#.#.#########.#.#.###.#.###.#                                                             #.###.#.#.#.###.#.#.###.#.###.#.#.#  
YL..#.....#.#.#.#.#.#.#..............EK                                                           #.....#.#.#.#...#...#.......#.#....UT
  #.###.#.###.#.#.#.#.###.#####.###.#                                                             #.#####.#.#.#.###.###.#######.###.#  
  #.....#.......#.......#.#.#.#.#.#.#                                                           ZV..#...#.#.#.#...#...#.#.....#.....#  
  #####.###.###.#######.###.#.#.#.###                                                             #.#.###.#.###.#######.###.#.#.#.#.#  
  #.......#...#.#.........#.........#                                                             #.#.#.#.....#.#...#.......#...#.#.#  
  #########.#######.###.#.#.###.#####                                                             ###.#.#.#.#.#.#.#########.###.###.#  
  #.#...#...........#...#...#.#.#...#                                                             #.....#.#.#.#.#.........#...#.#.#.#  
  #.###.#####################.#####.#                                                             ###.#.#########.#.#.###.#######.###  
  #.............#...........#...#.#..YS                                                           #...#.#...#.#.#.#.#.#.........#...#  
  #####.###.###.#.#####.###.###.#.#.#                                                             #.#.#.#.###.#.#####.#####.#######.#  
  #.......#...#.......#...#.........#                                                           FV..#.#...#.....#...#...#...#...#...#  
  ###.#.#.#.#.#.#.###.#.###.###.###.#                                                             #####.#####.#.###.###.#.###.#.#.#.#  
JB..#.#.#.#.#.#.#...#.#.#.....#.#.#.#                                                             #...........#.........#.....#...#..ZP
  #.#.###########.#.###.#########.###                                                             #.#.#######################.#######  
ZZ....#.....#.#.#.#.#.....#.........#                                                             #.#.#.#...#.#.............#.#......IB
  #.#####.###.#.#######.###.###.#####                                                             #####.#.#.#.#.#.#.#.#####.###.#.#.#  
  #.#...........#.....#.#...#.#.....#                                                           QR......#.#.#...#.#.#...#.....#.#.#.#  
  ###.#.#####.#####.#########.#.###.#                                                             #####.#.#.#.#####.#########.#.###.#  
  #.#.#...#.#.....#.#.......#.....#.#                                                             #.#.....#.....#...#.#...#...#...#.#  
  #.#####.#.###.###.###.#####.#.#.#.#                                                             #.###.#.###.###.#.#.#.###.###.#####  
  #.#...#.....#...#.#.#.#.#.#.#.#.#..ZP                                                           #...#.#...#...#.#.#.#.#.......#.#.#  
  #.#.#.#####.#.###.#.#.#.#.#####.###                                                             #.#################.#.#.#.#####.#.#  
YS....#.......#...................#.#                                                           TD..#...#.....#.#...#.#.#.#.#.....#..ZJ
  #######.###.#########.###.#.###.#.#                                                             #.###.###.###.#.###.#.###.#.###.#.#  
EK......#.#.#...#.........#.#.#.....#                                                             #.......#...#.....#.#.#...#...#.#.#  
  #####.###.#####.#.#################                                                             ###.#.###.###.#.###.#.#######.#.#.#  
  #.....#.#.#...#.#.#.#.#.....#.#...#                                                             #...#.#.....#.#.#.....#.......#...#  
  ###.#.#.#.#.#######.#.#.###.#.#.###                                                             #####.###.###.#.###.#.###.#.#.#.#.#  
  #...#.#.......#.........#.#.#.#...#                                                             #.#.#.........#.....#.....#.#.#.#.#  
  ###.###.###.#.#####.###.#.#.#.#.###                                                             #.#.#######################.#######  
  #.#.......#.#.......#.#.#.#........QP                                                           #...#...#...........#.....#.#.....#  
  #.###########.#####.#.###.#########                                                             #.#.#.#.#.###.#####.#.#.#####.###.#  
MP........#...#...#...#.......#.....#                                                             #.#...#.....#.#.....#.#.....#...#.#  
  #######.#.#########.#.#####.#.###.#                                                             #.###.#.#.#.#.#####.#.#####.###.#.#  
  #.........#...#...#.#.#.....#...#..TV                                                         NU....#.#.#.#.#.#.........#.....#.#..RP
  #.###.#######.#.#####.###.#####.###                                                             #.###.###############.#######.#.###  
  #.#...#.......#.#.#...#...#.....#.#                                                             #.#.#...#...#.#...#.#.#...#.......#  
  #.#####.#.#####.#.#.#####.###.###.#                                                             ###.#.###.###.###.#.###.#####.#####  
  #.......#...........#...#.....#.#.#                                                           RX....#.#.#.#.....#.#.#...#.#...#....JN
  #######.#######.#.#.#.#########.#.#                                                             #.#####.#.#.#####.#.#.#.#.#######.#  
  #...#.......#...#.#.#.....#...#....MP                                                           #.#.#...#...#.........#.#.....#...#  
  #.###################.###.#.#.#.#.#                                                             #.#.###.###.#####.#.#.#####.#####.#  
OF..#...#...#...#.#.#.....#...#.#.#..NZ                                                           #.#.....#...#...#.#.#.#.#.#.#.#.#.#  
  #.###.###.###.#.#.###.#.#####.#.#.#                                                             #.#.#.###.#.###.###.#.#.#.#.#.#.#.#  
QP..#...#...........#...#.#.....#.#.#                                                             #...#.....#.........#.............#  
  #.#.#.###.###.#.#.#####.#.#####.###                                                             #.#.#.#.###.###.###.#.#.#.#.###.#.#  
  #...#.....#...#.#.......#.......#.#                                                             #.#.#.#...#.#.#.#...#.#.#.#.#.#.#.#  
  #.#####.###.#.#.#.#.#.###.#.#.#.#.#      O           U     J   J       Z         H       S      ###.#####.#.#.#####.#.#.#####.#.###  
  #.....#.#...#.#.#.#.#.#...#.#.#...#      F           T     Q   B       J         K       K      #.....#.#.#.#.......#.#...#.......#  
  #.#.###.#.#.#.###.#.#.###.###.#.#########.###########.#####.###.#######.#########.#######.###########.#.#####.#.#.#.#####.###.#####  
  #.#...#.#.#.#...#.#.#.#.....#.#...#.......#...#.#...#.#.....#.........#...#...#.....#.#...#...#...........#...#.#.#.....#.#.......#  
  #.#.###.#.#.#.###.#.###.#####.#.#######.###.#.#.#.#.#.###.#######.#######.#.#.#.#####.###.#.###.#####.#.###.#.#####.#########.#.#.#  
  #.#.#.#.#.#.#...#.#...#...#...#...#...#.....#...#.#...#.#...#.#...#...#...#.#.#...#.#.#.......#...#...#...#.#.#.......#.#.....#.#.#  
  #.#.#.###.###.#.#######.#.###.#######.#######.#.#.#####.#.#.#.#.#####.#.###.#.#.###.#.#.#####.#########.###.###.#.###.#.#.###.#.###  
  #.#.#.#...#...#.#.......#...#.#...#.#.....#...#.#...#.....#...#.#...........#.#.......#...#.#.....#.....#.....#.#.#.....#...#.#.#.#  
  #.#.#.#.#.###.###.###.###.#######.#.#.#.#.#.#.#####.#####.#####.###.###.#.###.###.###.###.#.#######.#.#.#######.#.#####.#####.###.#  
  #.#...#.#.#...#.....#.#...#...........#.#...#.#.#.....#.....#.#...#.#...#.#.#...#...#.#.......#...#.#.#...#.....#...#...#.#.......#  
  #.#.###.#.#######.#.#####.#.###.###.###.#######.#.#.###.#####.#.#####.#.###.#.#######.#.###.#.#.#####.#.#####.###.#.###.#.###.###.#  
  #.#...#.#.....#.#.#.#...#.#.#.#...#.#...#.......#.#...#.......#.....#.#...#...#...#.#.#...#.#.#.....#.#.#.#.....#.#.#.....#.#...#.#  
  #.#.#####.#.###.#.#.###.#.###.#.#######.#.#.###.#####.###.###.###.#########.#.#.###.#.#.#####.#.#####.###.#.###.###.###.#.#.#.###.#  
  #.#.#.....#...#.#.#.#.....#.....#.........#.#.....#.#...#.#...#.......#.#...#...#.....#...#.........#.#.....#.#...#.#...#...#...#.#  
  ###.###.###.###.#######.#.#####.###########.#####.#.#.#######.#.#.###.#.#####.#####.###.###.###.###########.#.###.#####.#####.#.#.#  
  #.#.#.#...#...#.....#...#...#...#...............#.#...#.......#.#...#...#.#.....#.#...#...#.#.#.#.#.#.#.#.......#...#.....#...#.#.#  
  #.#.#.#.###.###.#.###.###.#######.#.#.#.#.###########.#####.#.#.#.#####.#.#.#.###.###.#.#####.###.#.#.#.#####.#########.#.#####.###  
  #.....#.#.#...#.#.....#.......#.#.#.#.#.#.#.......#...#.....#.#.#.#.#...#.#.#.#...#...#.....#.#.#.#.#.....#.....#.......#.#...#...#  
  #.#.###.#.#.#######.#.#.###.###.#######.#######.###.#######.###.###.#####.#.#.#.#.###.#.#####.#.#.#.#.###########.#.#.#.###.###.###  
  #.#...#...#.#.....#.#.#.#...#.#.#.#...........#.#.....#.....#...#...#.....#.#...#.#...#...#.#...#.....#.#.....#.#.#.#.#.......#.#.#  
  #.#####.#.#####.#####.#.#.###.#.#.#####.#.#####.#.###.###.#.###.###.###.#.###.#######.#.###.#.###.###.#.###.###.#.#.###.#####.#.#.#  
  #...#...#...#.........#.#.#.........#...#.#.....#.#.....#.#.#.#.....#.#.#.......#.#...#.......#.#.#.#.#.....#...#.#.#.......#.#...#  
  #.###.#.#.#.###.###.###.#######.###.###.#.#####.###.#.#####.#.#.#.###.###.###.###.###.###.#.###.#.#.###.#####.###.#####.#######.#.#  
  #...#.#.#.#.#.#...#...#...#.#.....#...#.#.....#.....#...#.....#.#.#.#.....#.#.#.#.....#...#...............#.....#.....#...#.#...#.#  
  #.###.#.#.###.#.###.#######.#.#.#####.#.#######.#.###.###.###.#.###.###.#.#.###.###.#####.#.#.###.#########.#####.#.#####.#.#######  
  #.#.#.#.#.#.....#...#.#.#.....#.#.......#.#.#.#.#...#.#.#.#.#.#.#.....#.#.....#...#.....#.#.#...#...#...#...#...#.#...#.....#.....#  
  ###.#.###.#.#.###.###.#.#.#.#.###.#####.#.#.#.#.#######.###.#.#.#.#.#.#######.###.###.#######.#########.###.#.#.#.#######.###.#.###  
  #.#...#.#.#.#...#.#.......#.#.#...#...........#.#...#.........#.#.#.#...#.....#.........#.#.#...#.....#.....#.#.#.#.#.#.#.....#...#  
  #.###.#.#.###.#.#.#.#.###.###.#.#######.#.###.#.#.#####.#####.#.###.#.###.#.#.#.###.#####.#.#.#######.#.###.###.###.#.#.###.#.#.#.#  
  #.....#...#...#.#.#.#.#...#...#.#...#...#.#.#.#.......#.#.....#.#...#...#.#.#.#...#.#.#.#.......#.#.......#.#.#.......#.#...#.#.#.#  
  #.#.#########.###.#.#####.###.#####.###.###.#.#######.#####.###.#.###.#####.###.###.#.#.#.#.#.###.#####.###.#.#.#######.#####.###.#  
  #.#.#...........#.#.#.....#.#.#.....#.#.#.....#.#.#.....#...#...#.#.#.#.#.#.#...#.#.#.#...#.#.....#.#.....#.#.#.............#.#...#  
  #######.###.#.###.#.#.#####.###.###.#.#.###.###.#.#.#######.###.#.#.#.#.#.#.###.#.###.#.###.#######.###.#####.#.###################  
  #...#...#...#...#.#.#...#.#...#.#.....#.#.#.....#.....#.......#...#.......#...#.......#.#.#...........#.#.#.#.#.#...........#.#.#.#  
  ###.#.#####.#####.#.###.#.###.#.###.#.###.###.#.#.###.###.#.#.#.#.#.#########.#######.###.#.#.#########.#.#.#.#.#.#####.#.###.#.#.#  
  #.........#.#.....#.#...#.......#...#.........#.#.#.....#.#.#.#.#.#...#.......#...........#.#.........................#.#.........#  
  #########################################.#######.###########.#######.###.#####.#############.#####################################  
                                           L       R           N       L   T     N             Q                                       
                                           P       X           Z       S   V     U             R                                       
";
    }
}
