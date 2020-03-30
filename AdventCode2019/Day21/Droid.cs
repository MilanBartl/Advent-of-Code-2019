using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdventCode2019.Day2;

namespace AdventCode2019.Day21
{
    public class Droid
    {
        private IntCodeComp Comp { get; set; }

        public Queue<byte[]> Commands { get; private set; }

        private int commandIdx = 0;

        public long SuccessOutput { get; private set; } = -1;

        public Droid(IntCodeComp comp)
        {
            Commands = new Queue<byte[]>();
            Comp = comp;

            Comp.GetInput = () =>
            {
                if (commandIdx >= Commands.Peek().Length)
                {
                    commandIdx = 0;
                    Commands.Dequeue();
                    return 10;
                }
                else
                {
                    return Commands.Peek()[commandIdx++];
                }
            };

            Comp.WriteOutput = (output) =>
            {
                char asciiChar = Encoding.ASCII.GetChars(new byte[] { (byte)output })[0];

                if (asciiChar != '.' && asciiChar != '#' && asciiChar != '@' && output != 10)
                    SuccessOutput = output;
                else
                {
                    if (output == 10)
                        Console.WriteLine();
                    else
                        Console.Write(asciiChar);
                }
            };
        }

        public void AddCommand(string command)
        {
            Commands.Enqueue(Encoding.ASCII.GetBytes(command));

            if (command == "WALK" || command == "RUN")
                Run();
        }

        public void AddCommands(string commands)
        {
            var splits = commands.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in splits)
            {
                AddCommand(command);
            }
        }

        public void Run()
        {
            Comp.RunProgram();
        }
    }
}
