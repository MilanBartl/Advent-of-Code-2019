using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day16
{
    public class Worker : IWorker
    {
        private int[] basePattern = new int[] { 0, 1, 0, -1 };

        public object Work1()
        {
            var signal = input.ToList().Select(c => int.Parse(c.ToString())).ToArray();

            for (int i = 0; i < 100; i++)
            {
                int[] buffer = new int[signal.Length];

                for (int n = 0; n < signal.Length; n++)
                {
                    int[] pattern = ComputePattern(n);
                    buffer[n] = CalculateValue(signal, pattern);
                }
                Array.Copy(buffer, signal, signal.Length);
            }

            return string.Join("", signal.Take(8));
        }

        public object Work2()
        {
            var signal = input.ToList().Select(c => int.Parse(c.ToString())).ToArray();
            /*var repeat = new int[signal.Length * 10000];
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < signal.Length; j++)
                {
                    repeat[j + i * j] = signal[j];
                }
            }*/

            string watch = "";

            for (int i = 0; i < 100; i++)
            {
                int[] buffer = new int[signal.Length];

                for (int n = 0; n < signal.Length; n++)
                {
                    int[] pattern = ComputePattern(n);
                    buffer[n] = CalculateValue(signal, pattern);
                }
                string bufferWatch = string.Join(" ", buffer);
                watch = watch + bufferWatch + Environment.NewLine;
                Array.Copy(buffer, signal, signal.Length);
            }

            int offset = int.Parse(string.Join("", signal.Take(7)));
            offset %= signal.Length;

            return string.Join("", signal.Skip(offset).Take(8));
        }

        private int CalculateValue(int[] source, int[] pattern)
        {
            int value = 0;
            int patIdx = 1;
            for (int i = 0; i < source.Length; i++)
            {
                if (patIdx == pattern.Length)
                    patIdx = 0;

                value += pattern[patIdx++] * source[i];
            }
            if (value < 10 && value > -10)
                return Math.Abs(value);
            else
                return Math.Abs(value) % 10;
        }

        private int[] ComputePattern(int position)
        {
            position++; // convert to 1-based indexing

            int[] pattern = new int[position * basePattern.Length];

            int patIdx = 0;
            for (int i = 0; i < basePattern.Length; i++)
            {
                for (int j = 0; j < position; j++)
                {
                    pattern[patIdx++] = basePattern[i];
                }
            }
            return pattern;
        }

        private const string smallInput = @"12345678";

        private const string input = @"59777373021222668798567802133413782890274127408951008331683345339720122013163879481781852674593848286028433137581106040070180511336025315315369547131580038526194150218831127263644386363628622199185841104247623145887820143701071873153011065972442452025467973447978624444986367369085768018787980626750934504101482547056919570684842729787289242525006400060674651940042434098846610282467529145541099887483212980780487291529289272553959088376601234595002785156490486989001949079476624795253075315137318482050376680864528864825100553140541159684922903401852101186028076448661695003394491692419964366860565639600430440581147085634507417621986668549233797848";
    }
}
