﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day2
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            var array = input.Split(new[] { ',' }).Select(x => long.Parse(x)).ToArray();

            var computer = new IntCodeComp(array);
            return computer.RunProgram(12, 2);
        }

        public object Work2()
        {
            var array = input.Split(new[] { ',' }).Select(x => long.Parse(x)).ToArray();
            var computer = new IntCodeComp(array);

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    long result = computer.RunProgram(noun, verb);
                    if (result == output)
                        return 100 * noun + verb;
                }
            }

            throw new Exception("Failed to find the result.");
        }

        private const string testInput = "1,1,1,4,99,5,6,0,99";

        private const string input = "1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,13,1,19,1,6,19,23,2,23,6,27,1,5,27,31,1,10,31,35,2,6,35,39,1,39,13,43,1,43,9,47,2,47,10,51,1,5,51,55,1,55,10,59,2,59,6,63,2,6,63,67,1,5,67,71,2,9,71,75,1,75,6,79,1,6,79,83,2,83,9,87,2,87,13,91,1,10,91,95,1,95,13,99,2,13,99,103,1,103,10,107,2,107,10,111,1,111,9,115,1,115,2,119,1,9,119,0,99,2,0,14,0";

        private const int output = 19690720;
    }
}
