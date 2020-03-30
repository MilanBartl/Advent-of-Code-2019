using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day2;

namespace AdventCode2019.Day7
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            var array = input.Split(new[] { ',' }).Select(x => long.Parse(x)).ToArray();

            var amp1 = new IntCodeComp(array);
            var amp2 = new IntCodeComp(array);
            var amp3 = new IntCodeComp(array);
            var amp4 = new IntCodeComp(array);
            var amp5 = new IntCodeComp(array);

            var phases = new List<int>() { 0, 1, 2, 3, 4 }.Permute();
            int res1 = 0, res2 = 0, res3 = 0, res4 = 0, res5 = 0;

            var maxThrusts = new List<int>();
            foreach (var phaseEnum in phases)
            {
                var phase = phaseEnum.ToList();

                amp1.GetInput = () =>
                {
                    amp1.GetInput = () =>
                    {
                        return 0;
                    };

                    return phase[0];
                };
                amp1.WriteOutput = (output) => res1 = (int) output;

                amp2.GetInput = () =>
                {
                    amp2.GetInput = () =>
                    {
                        return res1;
                    };

                    return phase[1];
                };
                amp2.WriteOutput = (output) => res2 = (int) output;

                amp3.GetInput = () =>
                {
                    amp3.GetInput = () =>
                    {
                        return res2;
                    };

                    return phase[2];
                };
                amp3.WriteOutput = (output) => res3 = (int) output;

                amp4.GetInput = () =>
                {
                    amp4.GetInput = () =>
                    {
                        return res3;
                    };

                    return phase[3];
                };
                amp4.WriteOutput = (output) => res4 = (int) output;

                amp5.GetInput = () =>
                {
                    amp5.GetInput = () =>
                    {
                        return res4;
                    };

                    return phase[4];
                };
                amp5.WriteOutput = (output) => res5 = (int) output;

                amp1.RunProgram();
                amp2.RunProgram();
                amp3.RunProgram();
                amp4.RunProgram();
                amp5.RunProgram();

                maxThrusts.Add(res5);
            }

            return maxThrusts.Max();
        }

        public object Work2()
        {
            var array = input.Split(new[] { ',' }).Select(x => long.Parse(x)).ToArray();

            var phases = new List<int>() { 5, 6, 7, 8, 9 }.Permute();

            var maxThrusts = new List<long>();
            foreach (var phaseEnum in phases)
            {
                var phase = phaseEnum.ToList();
                long result = 0;

                var amp1 = new Amplifier(new IntCodeComp(array), phase[0]);
                var amp2 = new Amplifier(new IntCodeComp(array), phase[1]);
                var amp3 = new Amplifier(new IntCodeComp(array), phase[2]);
                var amp4 = new Amplifier(new IntCodeComp(array), phase[3]);
                var amp5 = new Amplifier(new IntCodeComp(array), phase[4]);

                amp1.SetOutput((output) =>
                    { amp2.Input = output; });
                amp2.SetOutput((output) => 
                    { amp3.Input = output; });
                amp3.SetOutput((output) =>
                    { amp4.Input = output; });
                amp4.SetOutput((output) =>
                    { amp5.Input = output; });
                amp5.SetOutput((output) =>
                {
                    amp1.Input = output;
                    result = output;
                });

                var task1 = new Task(amp1.Run);
                var task2 = new Task(amp2.Run);
                var task3 = new Task(amp3.Run);
                var task4 = new Task(amp4.Run);
                var task5 = new Task(amp5.Run);

                task1.Start();
                task2.Start();
                task3.Start();
                task4.Start();
                task5.Start();

                amp1.Input = 0;

                task5.Wait();

                maxThrusts.Add(result);
            }

            return maxThrusts.Max();
        }

        private const string testInput = @"3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0";

        private const string testInput2 = @"3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0";

        private const string testInput3 = @"3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0";

        private const string input = @"3,8,1001,8,10,8,105,1,0,0,21,38,47,72,97,122,203,284,365,446,99999,3,9,1001,9,3,9,1002,9,5,9,1001,9,4,9,4,9,99,3,9,102,3,9,9,4,9,99,3,9,1001,9,2,9,102,5,9,9,101,3,9,9,1002,9,5,9,101,4,9,9,4,9,99,3,9,101,5,9,9,1002,9,3,9,101,2,9,9,102,3,9,9,1001,9,2,9,4,9,99,3,9,101,3,9,9,102,2,9,9,1001,9,4,9,1002,9,2,9,101,2,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,101,1,9,9,4,9,3,9,101,1,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,99,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,99
";
    }
}
