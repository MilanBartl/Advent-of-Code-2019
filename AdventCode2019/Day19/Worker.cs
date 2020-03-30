using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day2;

namespace AdventCode2019.Day19
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            var comp = new IntCodeComp(input);
            long x = 0, y = 0;
            bool isY = false;
            int pulled = 0;

            comp.GetInput = () =>
            {
                if (isY)
                {
                    isY = false;
                    return y;
                }
                else
                {
                    isY = true;
                    return x;
                }
            };

            comp.WriteOutput = (output) =>
            {
                if (output > 0)
                    pulled++;
            };

            for (x = 0; x < 50; x++)
            {
                for (y = 0; y < 50; y++)
                {
                    comp.RunProgram();
                }
            }

            return pulled;
        }

        public object Work2()
        {
            var comp = new IntCodeComp(input);
            long x = 0, y = 0;
            bool isY = false;
            int size = 200;

            // offset identified by calculation and subsequent experimentation
            int offsetY = 1000;
            int offsetX = 1300;

            int[,] field = new int[size, size];

            comp.GetInput = () =>
            {
                if (isY)
                {
                    isY = false;
                    return y;
                }
                else
                {
                    isY = true;
                    return x;
                }
            };

            comp.WriteOutput = (output) =>
            {
                if (output > 0)
                    field[x - offsetX, y - offsetY] = 1;
                else
                    field[x - offsetX, y - offsetY] = 0;
            };

            // generate field
            for (x = offsetX; x < size + offsetX; x++)
            {
                for (y = offsetY; y < size + offsetY; y++)
                {
                    comp.RunProgram();
                }
            }     

            for (x = 0; x < size - 99; x++)
            {
                for (y = 0; y < size - 99; y++)
                {
                    if (field[x, y] == 1 && field[x + 99, y] == 1 && field[x, y + 99] == 1)
                    {
                        DisplayFieldWithObject(field, x, y, 100);
                        Console.WriteLine($"100x100 field found at: X: {x + offsetX} Y: {y + offsetY}; answer is {(x + offsetX) * 10000 + (y + offsetY)}");
                        return 0;
                    }
                }
            }
            return 0;
        }

        public void DisplayFieldWithObject(int[,] field, long objX, long objY, int objectSize)
        {
            int size = (int) Math.Sqrt(field.Length);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x >= objX && x < objX + objectSize && y >= objY & y < objY + objectSize)
                        Console.Write('0');
                    else
                        Console.Write(field[x, y] == 1 ? '#' : '.');
                }
                Console.WriteLine();
            }
        }

        private const string input = @"109,424,203,1,21102,11,1,0,1105,1,282,21102,1,18,0,1106,0,259,2101,0,1,221,203,1,21102,1,31,0,1106,0,282,21102,38,1,0,1105,1,259,20101,0,23,2,22101,0,1,3,21101,1,0,1,21101,57,0,0,1105,1,303,2101,0,1,222,21001,221,0,3,21002,221,1,2,21101,0,259,1,21102,80,1,0,1106,0,225,21102,89,1,2,21102,91,1,0,1105,1,303,2101,0,1,223,20101,0,222,4,21101,0,259,3,21102,1,225,2,21102,225,1,1,21102,118,1,0,1106,0,225,20101,0,222,3,21101,136,0,2,21101,133,0,0,1106,0,303,21202,1,-1,1,22001,223,1,1,21101,148,0,0,1105,1,259,1202,1,1,223,20102,1,221,4,21001,222,0,3,21102,18,1,2,1001,132,-2,224,1002,224,2,224,1001,224,3,224,1002,132,-1,132,1,224,132,224,21001,224,1,1,21102,195,1,0,106,0,108,20207,1,223,2,20102,1,23,1,21101,-1,0,3,21101,214,0,0,1105,1,303,22101,1,1,1,204,1,99,0,0,0,0,109,5,1202,-4,1,249,21201,-3,0,1,22102,1,-2,2,21202,-1,1,3,21102,1,250,0,1105,1,225,21201,1,0,-4,109,-5,2105,1,0,109,3,22107,0,-2,-1,21202,-1,2,-1,21201,-1,-1,-1,22202,-1,-2,-2,109,-3,2105,1,0,109,3,21207,-2,0,-1,1206,-1,294,104,0,99,22102,1,-2,-2,109,-3,2105,1,0,109,5,22207,-3,-4,-1,1206,-1,346,22201,-4,-3,-4,21202,-3,-1,-1,22201,-4,-1,2,21202,2,-1,-1,22201,-4,-1,1,21201,-2,0,3,21102,343,1,0,1106,0,303,1105,1,415,22207,-2,-3,-1,1206,-1,387,22201,-3,-2,-3,21202,-2,-1,-1,22201,-3,-1,3,21202,3,-1,-1,22201,-3,-1,2,21202,-4,1,1,21102,384,1,0,1105,1,303,1106,0,415,21202,-4,-1,-4,22201,-4,-3,-4,22202,-3,-2,-2,22202,-2,-4,-4,22202,-3,-2,-3,21202,-4,-1,-2,22201,-3,-2,1,21202,1,1,-4,109,-5,2106,0,0";
    }
}
