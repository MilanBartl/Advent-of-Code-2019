using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day3
{
    public class Wire
    {
        public HashSet<Coordinate> Coordinates { get; set; }

        public LinkedList<Coordinate> Route { get; set; }

        public Wire()
        {
            Coordinates = new HashSet<Coordinate>();
            Route = new LinkedList<Coordinate>();
        }

        public int GetRouteTo(Coordinate destination)
        {
            var actual = Route.First;

            int counter = 1;
            while (!actual.Value.Equals(destination))
            {
                if (actual == Route.Last)
                    throw new Exception($"Failed to locate destination point [{destination.X},{destination.Y}]");

                actual = actual.Next;
                counter++;
            }

            return counter;
        }
    }
}
