using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day24
{
    public class Worker : IWorker
    {
        private const int size = 5;

        public object Work1()
        {
            var splits = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            char[,] array = new char[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    array[x, y] = splits[y][x];
                }
            }

            var biodiv = new HashSet<int>();
            int currbiodiv = CalcBioDiv(array);

            while (!biodiv.Contains(currbiodiv))
            {
                biodiv.Add(currbiodiv);
                array = NextIteration(array);
                currbiodiv = CalcBioDiv(array);
            }

            return currbiodiv;
        }

        public object Work2()
        {
            throw new NotImplementedException();
        }

        private int CalcBioDiv(char[,] array)
        {
            int mult = 1;
            int result = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    result += mult * (array[x, y] == '#' ? 1 : 0);
                    mult *= 2;
                }
            }
            return result;
        }

        private char[,] NextIteration(char[,] array)
        {
            char[,] newArray = new char[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int bugs = CalcNeighbors(array, x, y);

                    if (array[x, y] == '#' && bugs != 1)
                        newArray[x, y] = '.';
                    else if (array[x, y] == '.' && (bugs > 0 && bugs < 3))
                        newArray[x, y] = '#';
                    else
                        newArray[x, y] = array[x, y];
                }
            }

            return newArray;
        }

        private int CalcNeighbors(char[,] array, int x, int y)
        {
            char[] comparable = new char[4];

            if (x - 1 >= 0)
                comparable[0] = array[x - 1, y];
            if (x + 1 < size)
                comparable[1] = array[x + 1, y];
            if (y - 1 >= 0)
                comparable[2] = array[x, y - 1];
            if (y + 1 < size)
                comparable[3] = array[x, y + 1];

            return comparable.Sum(tile => tile == '#' ? 1 : 0);
        }

        private const string input = @"#.###
.....
#..#.
##.##
..#.#";
    }
}
