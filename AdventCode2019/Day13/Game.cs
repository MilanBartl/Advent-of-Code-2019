using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day2;
using AdventCode2019.Day3;

namespace AdventCode2019.Day13
{
    public class Game
    {
        private bool IsX, IsY, IsTile;

        private IntCodeComp Comp { get; set; }

        private int X { get; set; }

        private int Y { get; set; }

        public Dictionary<Coordinate, int> Tiles { get; private set; }

        public long Score { get; private set; }

        public Game(IntCodeComp comp)
        {
            Comp = comp;
            Score = 0;
            IsX = true;

            Tiles = new Dictionary<Coordinate, int>();

            Comp.WriteOutput = (output) =>
            {
                if (IsX)
                {
                    X = (int)output;

                    IsX = false;
                    IsY = true;
                }
                else if (IsY)
                {
                    Y = (int)output;

                    IsY = false;
                    IsTile = true;
                }
                else if (IsTile)
                {
                    if (X == -1 && Y == 0)
                        Score = output;
                    else
                    {
                        var position = new Coordinate(X, Y);
                        if (Tiles.ContainsKey(position))
                            Tiles[position] = (int)output;
                        else
                            Tiles.Add(position, (int)output);
                    }

                    IsTile = false;
                    IsX = true;
                }
                else
                    throw new Exception("This should never happen.");
            };

            Comp.GetInput = () =>
            {
                //Display();

                int paddlePos = Tiles.First(t => t.Value == 3).Key.X;
                int ballPos = Tiles.First(t => t.Value == 4).Key.X;

                if (paddlePos < ballPos)
                    return 1;
                else if (paddlePos > ballPos)
                    return -1;
                else
                    return 0;
            };
        }

        public void Run()
        {
            Comp.RunProgram();
        }

        private void Display()
        {
            int maxX = Tiles.Keys.Select(coord => coord.X).Max();
            int maxY = Tiles.Keys.Select(coord => coord.Y).Max();

            int width = maxX + 1;
            int height = maxY + 1;
            char[,] canvas = new char[height, width];

            // initiate canvas
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    canvas[x, y] = '.';
                }
            }

            // fill in painted fields
            foreach (var tile in Tiles.Where(t => t.Value != 0))
            {
                char c;
                switch (tile.Value)
                {
                    case 1:
                        c = 'W';
                        break;
                    case 2:
                        c = 'B';
                        break;
                    case 3:
                        c = '_';
                        break;
                    case 4:
                        c = '°';
                        break;
                    default:
                        throw new ArgumentException($"Incorrect tile value: {tile.Value}'.");
                }
                canvas[tile.Key.Y, tile.Key.X] = c;
            }

            // print the canvas (inverted around X axis)
            for (int x = height - 1; x >= 0; x--)
            {
                for (int y = 0; y < width; y++)
                {
                    Console.Write(canvas[x, y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
