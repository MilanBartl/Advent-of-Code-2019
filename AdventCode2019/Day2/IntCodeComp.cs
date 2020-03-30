using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day2
{
    public class IntCodeComp
    {
        private long[] input;

        private long[] memory;

        private long address = 0;

        private long relativeBase = 0;

        public bool Pause { get; set; }

        public bool IsRunning { get; set; }

        public Func<long> GetInput { get; set; }

        public Action<long> WriteOutput { get; set; }

        public IntCodeComp(string strInput)
        {
            var array = strInput.Split(new[] { ',' }).Select(x => long.Parse(x)).ToArray();
            input = array;
        }

        public IntCodeComp(long[] datain)
        {
            input = datain;
        }

        public long RunProgram()
        {
            MemInit();
            IsRunning = true;
            return Run();
        }

        public long RunProgram(int noun, int verb)
        {
            MemInit();
            IsRunning = true;

            memory[1] = noun;
            memory[2] = verb;

            return Run();
        }

        public long ContinueProgram()
        {
            return Run();
        }

        private long Run()
        {
            bool stop = false;

            while (!stop && !Pause)
            {
                stop = ExecuteInstruction();
            }
            Pause = false;

            return memory[0];
        }

        private bool ExecuteInstruction()
        {
            bool stop = false;
            int step = 0;
            long value1, value2;

            int opcode = GetOpcode(memory[address], out int mode1, out int mode2, out int mode3);

            switch (opcode)
            {
                case 1: // addition
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    SetValue(mode3, 3, value1 + value2);
                    step = 4;
                    break;
                case 2: // multiplication
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    SetValue(mode3, 3, value1 * value2);
                    step = 4;
                    break;
                case 3: // input
                    value1 = GetInput();
                    SetValue(mode1, 1, value1);
                    step = 2;
                    break;
                case 4: // output
                    WriteOutput(GetValue(mode1, 1));
                    step = 2;
                    break;
                case 5: // jump if true
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    if (value1 != 0)
                    {
                        address = value2;
                        step = 0;
                    }
                    else
                        step = 3;
                    break;
                case 6: // jump if false
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    if (value1 == 0)
                    {
                        address = value2;
                        step = 0;
                    }
                    else
                        step = 3;
                    break;
                case 7: // less than
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    if (value1 < value2)
                        SetValue(mode3, 3, 1);
                    else
                        SetValue(mode3, 3, 0);

                    step = 4;
                    break;
                case 8: // equals
                    value1 = GetValue(mode1, 1);
                    value2 = GetValue(mode2, 2);

                    if (value1 == value2)
                        SetValue(mode3, 3, 1);
                    else
                        SetValue(mode3, 3, 0);

                    step = 4;
                    break;
                case 9: // set relative base
                    relativeBase += GetValue(mode1, 1);
                    step = 2;
                    break;
                case 99:
                    stop = true;
                    step = 1;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown instruction '{memory[address]}' at address '{address}'.");
            }
            MoveAddress(step);

            return stop;
        }

        private void SetValue(int mode, int offset, long value)
        {
            long pointer;
            switch (mode)
            {
                case 0:
                    pointer = memory[address + offset];
                    break;
                case 1:
                    pointer = memory[address + offset];
                    break;
                case 2:
                    pointer = memory[address + offset] + relativeBase;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid SetValue operation mode '{mode}'.");
            }

            if (pointer >= memory.Length)
                EnhanceMemory(pointer);
            memory[pointer] = value;
        }

        private long GetValue(int mode, int offset)
        {
            long pointer = 0;
            switch (mode)
            {      
                case 0:       
                    pointer = memory[address + offset];
                    break;
                case 1:
                    pointer = address + offset;
                    break;
                case 2:
                    pointer = memory[address + offset] + relativeBase;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid GetValue operation mode '{mode}'.");
            }

            if (pointer >= memory.Length)
                EnhanceMemory(pointer);
            return memory[pointer];
        }

        private void EnhanceMemory(long pointer)
        {
            var newmem = new long[2 * pointer];
            Array.Copy(memory, newmem, memory.Length);
            memory = newmem;
        }

        private void MoveAddress(int step)
        {
            address += step;
        }

        private void MemInit()
        {
            address = 0;
            memory = new long[input.Length];
            Array.Copy(input, memory, input.Length);
        }

        private int GetOpcode(long value, out int mode1, out int mode2, out int mode3)
        {
            int opcode = (int) value % 100;
            value -= opcode;

            mode1 = (int) value % 1000;
            value -= mode1;
            mode1 /= 100;

            mode2 = (int) value % 10000;
            value -= mode2;
            mode2 /= 1000;

            mode3 = (int) value % 100000;
            value -= mode3;
            mode3 /= 10000;

            if (value != 0)
                throw new Exception($"Failed to correctly decompose instruction opcode and parameter modes. The value should be '0' and is '{value}'.");

            return opcode;
        }
    }
}
