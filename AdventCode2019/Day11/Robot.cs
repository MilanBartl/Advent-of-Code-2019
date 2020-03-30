using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day2;
using AdventCode2019.Day3;

namespace AdventCode2019.Day11
{
    public class Robot
    {
        private bool isPainting = true;

        private IntCodeComp Comp { get; set; }

        private Coordinate Position { get; set; }

        private Direction Direction { get; set; }

        public Dictionary<Coordinate, int> VisitedFields { get; private set; }

        public Robot(IntCodeComp comp, int startColor)
        {
            Comp = comp;
            Position = new Coordinate(0, 0);
            Direction = Direction.Up;
            VisitedFields = new Dictionary<Coordinate, int>
            {
                { Position, startColor }
            };

            comp.GetInput = () =>
            {
                if (VisitedFields.ContainsKey(Position))
                    return VisitedFields[Position];
                else
                    throw new IndexOutOfRangeException("This should not happen.");
            };

            comp.WriteOutput = (output) =>
            {
                if (isPainting)
                {
                    VisitedFields[Position] = (int) output;
                    isPainting = false;
                }
                else
                {
                    if (output == 0)
                    {
                        switch (Direction)
                        {
                            case Direction.Up:
                                Position = new Coordinate(Position.X - 1, Position.Y);
                                Direction = Direction.Left;
                                break;
                            case Direction.Down:
                                Position = new Coordinate(Position.X + 1, Position.Y);
                                Direction = Direction.Right;
                                break;
                            case Direction.Left:
                                Position = new Coordinate(Position.X, Position.Y - 1);
                                Direction = Direction.Down;
                                break;
                            case Direction.Right:
                                Position = new Coordinate(Position.X, Position.Y + 1);
                                Direction = Direction.Up;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (output == 1)
                    {
                        switch (Direction)
                        {
                            case Direction.Up:
                                Position = new Coordinate(Position.X + 1, Position.Y);
                                Direction = Direction.Right;
                                break;
                            case Direction.Down:
                                Position = new Coordinate(Position.X - 1, Position.Y);
                                Direction = Direction.Left;
                                break;
                            case Direction.Left:
                                Position = new Coordinate(Position.X, Position.Y + 1);
                                Direction = Direction.Up;
                                break;
                            case Direction.Right:
                                Position = new Coordinate(Position.X, Position.Y - 1);
                                Direction = Direction.Down;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        throw new ArgumentException($"Error. Incorrect turn mode value '{output}'.");

                    if (!VisitedFields.ContainsKey(Position))
                        VisitedFields.Add(Position, 0);

                    isPainting = true;
                }
            };
        }

        public void Run()
        {
            Comp.RunProgram();
        }
    }

    public enum Direction
    {
        Up,
        Down, 
        Left,
        Right
    }
}
