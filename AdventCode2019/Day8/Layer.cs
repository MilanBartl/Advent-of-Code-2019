using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day8
{
    public class Layer
    {
        public int[,] Data { get; private set; }

        public int[] RawData { get; private set; }

        public Layer(string input, int width, int height)
        {
            RawData = new int[input.Length];

            Data = new int[width, height];
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = int.Parse(input[index].ToString());
                    Data[x, y] = value;
                    RawData[index] = value;
                    index++;
                }
            }
        }
    }
}
