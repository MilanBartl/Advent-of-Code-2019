using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day3;

namespace AdventCode2019.Day10
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            var asteroids = LoadAsteroids(input);
            var counts = new List<int>();

            foreach (Coordinate ast in asteroids)
            {
                var others = new List<Coordinate>();
                asteroids.ForEach(a => { if (a != ast) others.Add(a); });

                var angles = new List<double>();
                for (int i = 0; i < others.Count; i++)
                {
                    double angle = Math.Atan2(others[i].Y - ast.Y, others[i].X - ast.X);
                    if (!angles.Contains(angle))
                        angles.Add(angle);
                }
                counts.Add(angles.Count);
            }

            return counts.Max();
        }

        public object Work2()
        {
            var asteroids = LoadAsteroids(input);
            var counts = new Dictionary<Coordinate, Dictionary<double, List<Coordinate>>>();
            
            foreach (Coordinate ast in asteroids)
            {
                var others = new List<Coordinate>();
                asteroids.ForEach(a => { if (a != ast) others.Add(a); });

                var angles = new Dictionary<double, List<Coordinate>>();
                for (int i = 0; i < others.Count; i++)
                {
                    double angle = Math.Atan2(others[i].Y - ast.Y, others[i].X - ast.X);
                    if (!angles.ContainsKey(angle))
                        angles.Add(angle, new List<Coordinate>() { others[i] });
                    else
                        angles[angle].Add(others[i]);
                }
                counts.Add(ast, angles);
            }

            Coordinate laser = counts.Keys.First(k => counts[k].Count == counts.Keys.Max(key => counts[key].Count));
            Console.WriteLine($"Laser established at {laser.X},{laser.Y}");
            var astField = counts[laser];

            var angleList = astField.Keys.ToList();
            angleList.Sort();

            var negativeAngles = angleList.Where(x => x < 0).ToList();
            angleList.RemoveRange(0, negativeAngles.Count());
            //negativeAngles.Reverse();
            angleList = angleList.Concat(negativeAngles).ToList();

            foreach (List<Coordinate> angleAsts in astField.Values)
            {
                angleAsts.Sort((x, y) => 
                {
                    int distX = Utilities.CalculateManhattanDistance(laser, x);
                    int distY = Utilities.CalculateManhattanDistance(laser, y);

                    if (distX < distY)
                        return -1;
                    else if (distX == distY)
                        return 0;
                    else
                        return 1;
                });
            }

            int counter = 0;
            int angleIdx = angleList.IndexOf(-90 * Math.PI / 180);
            Coordinate destroyed = null;
            while (counter != 200)
            {
                if (angleIdx >= angleList.Count)
                    angleIdx = 0;

                if (astField[angleList[angleIdx]].Count > 0)
                {
                    destroyed = astField[angleList[angleIdx]][0];

                    Console.WriteLine($"Vaporized at {destroyed.X},{destroyed.Y}");

                    astField[angleList[angleIdx]].RemoveAt(0);
                    counter++;
                }
                angleIdx++;
            }

            return 100 * destroyed.X + destroyed.Y;
        }

        private List<Coordinate> LoadAsteroids(string datain)
        {
            var asteroids = new List<Coordinate>();

            var lines = datain.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int y = 0;
            foreach (string line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            break;
                        case '#':
                            asteroids.Add(new Coordinate(x, y));
                            break;
                        default:
                            throw new ArgumentException($"Invalid character '{line[x]}' in the map.");
                    }
                }
                y++;
            }
            return asteroids;
        }

        private const string test = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        private const string input = @".#......#...#.....#..#......#..##..#
..#.......#..........#..##.##.......
##......#.#..#..#..##...#.##.###....
..#........#...........#.......##...
.##.....#.......#........#..#.#.....
.#...#...#.....#.##.......#...#....#
#...#..##....#....#......#..........
....#......#.#.....#..#...#......#..
......###.......#..........#.##.#...
#......#..#.....#..#......#..#..####
.##...##......##..#####.......##....
.....#...#.........#........#....#..
....##.....#...#........#.##..#....#
....#........#.###.#........#...#..#
....#..#.#.##....#.........#.....#.#
##....###....##..#..#........#......
.....#.#.........#.......#....#....#
.###.....#....#.#......#...##.##....
...##...##....##.........#...#......
.....#....##....#..#.#.#...##.#...#.
#...#.#.#.#..##.#...#..#..#..#......
......#...#...#.#.....#.#.....#.####
..........#..................#.#.##.
....#....#....#...#..#....#.....#...
.#####..####........#...............
#....#.#..#..#....##......#...#.....
...####....#..#......#.#...##.....#.
..##....#.###.##.#.##.#.....#......#
....#.####...#......###.....##......
.#.....#....#......#..#..#.#..#.....
..#.......#...#........#.##...#.....
#.....####.#..........#.#.......#...
..##..#..#.....#.#.........#..#.#.##
.........#..........##.#.##.......##
#..#.....#....#....#.#.......####..#
..............#.#...........##.#.#..
";
    }
}
