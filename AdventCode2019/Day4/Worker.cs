using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day4
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            int counter = 0;
            for (int i = bottom; i < top + 1; i++)
            {
                if (MeetsCriteria1(i))
                    counter++;
            }

            return counter;
        }

        public object Work2()
        {
            int counter = 0;
            for (int i = bottom; i < top + 1; i++)
            {
                if (MeetsCriteria2(i))
                    counter++;
            }

            return counter;
        }

        private bool MeetsCriteria1(int number)
        {
            int[] array = ConvertToArray(number);

            bool hasDouble = false;
            for (int i = 1; i < 6; i++)
            {
                int prev = array[i - 1];
                int act = array[i];

                if (prev == act)
                    hasDouble = true;

                if (prev > act)
                    return false;
            }

            return hasDouble;
        }

        private bool MeetsCriteria2(int number)
        {
            int[] array = ConvertToArray(number);

            var dict = new Dictionary<int, int>();
            int pprev = -1;
            for (int i = 1; i < 6; i++)
            {
                int prev = array[i - 1];
                int act = array[i];

                if (prev == act)
                {
                    if (dict.ContainsKey(act))
                        dict[act]++;
                    else
                        dict.Add(act, 2);
                }

                if (prev > act)
                    return false;

                pprev = prev;
            }

            return dict.Any(x => x.Value == 2);
        }

        private int[] ConvertToArray(int number)
        {
            int[] array = new int[6];
            int index = 5;
            for (int i = 10; i < 10000000; i *= 10)
            {
                int value = (number % i);
                array[index] = value * 10 / i;
                number -= value;
                index--;
            }
            return array;
        }

        private const int bottom = 171309;
        private const int top = 643603;
    }
}
