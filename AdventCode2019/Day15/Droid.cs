using AdventCode2019.Day2;
using AdventCode2019.Day3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventCode2019.Day15
{
    public class Droid
    {
        private IntCodeComp Comp { get; set; }

        public Coordinate CurrentLocation { get; private set; }

        private Direction Input { get; set; }

        private char Output { get; set; } = ' ';

        public Dictionary<Coordinate, int> VisitedCoords { get; private set; }

        public Droid(IntCodeComp comp)
        {
            Comp = comp;
            CurrentLocation = Coordinate.Create(0, 0);
            VisitedCoords = new Dictionary<Coordinate, int>()
            {
                { CurrentLocation, 1 }
            };

            Comp.GetInput = () =>
            {
                while (Input == Direction.None)
                    Thread.Sleep(50);

                int direction = (int)Input;
                Input = Direction.None;

                return direction;
            };

            Comp.WriteOutput = (output) =>
            {
                switch (output)
                {
                    case 0:
                        Output = '#';
                        break;
                    case 1:
                        Output = '.';
                        break;
                    case 2:
                        Output = '@';
                        break;
                    default:
                        throw new Exception($"Invalid return code '{output}'.");
                }
            };
        }

        public void Start()
        {
            var program = new Task(() =>
            {
                Comp.RunProgram();
            });
            program.Start();
        }

        public char MoveDirection(Direction dir)
        {
            Input = dir;

            while (Output == ' ')
                Thread.Sleep(50);

            char output = Output;
            Output = ' ';

            switch (output)
            {
                case '#':
                    break;
                case '.':
                case '@':
                    switch (dir)
                    {
                        case Direction.North:
                            CurrentLocation = Coordinate.Create(CurrentLocation.X, CurrentLocation.Y - 1);
                            break;
                        case Direction.South:
                            CurrentLocation = Coordinate.Create(CurrentLocation.X, CurrentLocation.Y + 1);
                            break;
                        case Direction.West:
                            CurrentLocation = Coordinate.Create(CurrentLocation.X - 1, CurrentLocation.Y);
                            break;
                        case Direction.East:
                            CurrentLocation = Coordinate.Create(CurrentLocation.X + 1, CurrentLocation.Y);
                            break;
                        case Direction.None:
                        default:
                            throw new Exception("Invalid direction given to droid. This should never happen.");
                    }

                    // add visited location
                    if (VisitedCoords.ContainsKey(CurrentLocation))
                        VisitedCoords[CurrentLocation]++;
                    else
                        VisitedCoords.Add(CurrentLocation, 1);
                    break;
                default:
                    throw new Exception($"Invalid return character '{output}'.");
            }

            return output;
        }
    }

    public enum Direction
    {
        None = 0,
        North = 1,
        South = 2,
        West = 3,
        East = 4
    };
}
