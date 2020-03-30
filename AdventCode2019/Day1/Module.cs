using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day1
{
    public class Module
    {
        public string Mass { get; set; }

        private int intMass = -1;

        public int IntMass
        {
            get
            {
                if (intMass < 0)
                    intMass = int.Parse(Mass);
                return intMass;
            }
        }

        public Module(string mass)
        {
            Mass = mass;
        }

        public int GetFuelReq()
        {
            double divide = IntMass / 3;
            return (int)Math.Floor(divide) - 2;
        }

        public int GetFuelReq2()
        {
            return CalcFuelReq(IntMass);
        }

        private int CalcFuelReq(int input)
        {
            double divide = input / 3;
            int res = (int)Math.Floor(divide) - 2;

            if (res <= 0)
                return 0;
            else
                return res + CalcFuelReq(res);
        }
    }
}
