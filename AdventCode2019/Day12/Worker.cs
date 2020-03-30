using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day12
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            var moons = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(row => new Moon(row)).ToList();

            for (int i = 0; i < 1000; i++)
            {
                moons[0].ApplyGravity(moons[1]);
                moons[1].ApplyGravity(moons[0]);

                moons[0].ApplyGravity(moons[2]);
                moons[2].ApplyGravity(moons[0]);

                moons[0].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[0]);

                moons[1].ApplyGravity(moons[2]);
                moons[2].ApplyGravity(moons[1]);

                moons[1].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[1]);

                moons[2].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[2]);

                moons.ForEach(moon => moon.Move());
            }

            return moons.Sum(moon => moon.GetEnergy());
        }

        public object Work2()
        {
            var moons = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(row => new Moon(row)).ToList();

            var moon0 = new Moon(moons[0]);
            var moon1 = new Moon(moons[1]);
            var moon2 = new Moon(moons[2]);
            var moon3 = new Moon(moons[3]);

            var xHash = new HashSet<(int, int, int, int, int, int, int, int)>();
            var yHash = new HashSet<(int, int, int, int, int, int, int, int)>();
            var zHash = new HashSet<(int, int, int, int, int, int, int, int)>();

            bool xFound = false, yFound = false, zFound = false;

            int xCounter = 0, yCounter = 0, zCounter = 0;
            for (long i = 0; i < long.MaxValue; i++)
            {
                var Xs = (moons[0].X, moons[0].VelX, moons[1].X, moons[1].VelX, moons[2].X, moons[2].VelX, moons[3].X, moons[3].VelX);
                var Ys = (moons[0].Y, moons[0].VelY, moons[1].Y, moons[1].VelY, moons[2].Y, moons[2].VelY, moons[3].Y, moons[3].VelY);
                var Zs = (moons[0].Z, moons[0].VelZ, moons[1].Z, moons[1].VelZ, moons[2].Z, moons[2].VelZ, moons[3].Z, moons[3].VelZ);

                if (xHash.Contains(Xs))
                    xFound = true;
                else
                {
                    xHash.Add(Xs);
                    xCounter++;
                }

                if (yHash.Contains(Ys))
                    yFound = true;
                else
                {
                    yHash.Add(Ys);
                    yCounter++;
                }

                if (zHash.Contains(Zs))
                    zFound = true;
                else
                {
                    zHash.Add(Zs);
                    zCounter++;
                }

                if (xFound && yFound && zFound)
                    break;

                moons[0].ApplyGravity(moons[1]);
                moons[1].ApplyGravity(moons[0]);

                moons[0].ApplyGravity(moons[2]);
                moons[2].ApplyGravity(moons[0]);

                moons[0].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[0]);

                moons[1].ApplyGravity(moons[2]);
                moons[2].ApplyGravity(moons[1]);

                moons[1].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[1]);

                moons[2].ApplyGravity(moons[3]);
                moons[3].ApplyGravity(moons[2]);

                moons.ForEach(moon => moon.Move());
            }

            return LeastCommonMultiple(xCounter, LeastCommonMultiple(yCounter, zCounter));
        }

        private static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static long LeastCommonMultiple(long a, long b) => (a / GreatestCommonFactor(a, b)) * b;

        private string test = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";

        private string input = @"<x=-19, y=-4, z=2>
<x=-9, y=8, z=-16>
<x=-4, y=5, z=-11>
<x=1, y=9, z=-13>";
    }
}
