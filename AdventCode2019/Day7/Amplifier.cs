using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventCode2019.Day2;

namespace AdventCode2019.Day7
{
    public class Amplifier
    {
        private IntCodeComp Comp { get; set; }

        public long? Input { get; set; }

        public Amplifier(IntCodeComp comp, int phase)
        {
            Comp = comp;

            Comp.GetInput = () =>
            {
                Comp.GetInput = () =>
                {
                    while (Input == null)
                    {
                        Thread.Sleep(100);
                    }

                    long value = Input.Value;
                    Input = null;
                    return value;
                };

                return phase;
            };
        }

        public void Run()
        {
            Comp.RunProgram();
        }

        public void SetOutput(Action<long> outputAction)
        {
            Comp.WriteOutput = outputAction;
        }
    }
}
