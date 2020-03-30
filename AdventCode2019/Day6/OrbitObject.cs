using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day6
{
    public class OrbitObject : IEquatable<OrbitObject>
    {
        public string Name { get; private set; }

        public OrbitObject Parent { get; set; }

        public List<OrbitObject> Satellites { get; private set; }

        public OrbitObject(string name)
        {
            Name = name;

            Satellites = new List<OrbitObject>();
        }

        public int CountOrbits(int distance)
        {
            return distance + Satellites.Sum(sat => sat.CountOrbits(distance + 1));
        }

        public bool HasChild(string child)
        {
            return Satellites.Any(x => x.Name.Equals(child) || x.HasChild(child));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OrbitObject);
        }

        public bool Equals(OrbitObject other)
        {
            return other != null && Name == other.Name;
        }
    }
}
