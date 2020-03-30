using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day3
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coordinate Create(int x, int y)
        {
            return new Coordinate(x, y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + 8191 * Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return other != null && X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
