using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2019.Day12
{
    public class Moon
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public int Z { get; private set; }

        public int VelX { get; private set; }

        public int VelY { get; private set; }

        public int VelZ { get; private set; }

        public Moon(string position)
        {
            var splits = position.Split(' ');
            X = int.Parse(Regex.Match(splits[0], @"-?\d+").Value);
            Y = int.Parse(Regex.Match(splits[1], @"-?\d+").Value);
            Z = int.Parse(Regex.Match(splits[2], @"-?\d+").Value);
        }

        public Moon(Moon moon)
        {
            X = moon.X;
            Y = moon.Y;
            Z = moon.Z;
            VelX = moon.VelX;
            VelY = moon.VelY;
            VelZ = moon.VelZ;
        }

        public void ApplyGravity(Moon other)
        {
            if (other.X > X)
                VelX++;
            else if (other.X < X)
                VelX--;

            if (other.Y > Y)
                VelY++;
            else if (other.Y < Y)
                VelY--;

            if (other.Z > Z)
                VelZ++;
            else if (other.Z < Z)
                VelZ--;
        }

        public void Move()
        {
            X += VelX;
            Y += VelY;
            Z += VelZ;
        }

        public int GetEnergy()
        {
            return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) * (Math.Abs(VelX) + Math.Abs(VelY) + Math.Abs(VelZ));
        }
    }
}
