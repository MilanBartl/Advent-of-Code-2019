using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventCode2019.Day2;

namespace AdventCode2019.Day23
{
    public class Nic
    {
        private bool processInX = true;

        private bool processOutAdd = true;

        private bool processOutX = false;

        private (long address, long x, long y) packet;

        private IntCodeComp Computer { get; set; }

        public Queue<(long x, long y)> Queue { get; private set; }

        public int Address { get; private set; }

        public Action<(long address, long x, long y)> SendPacket { get; set; }

        public bool IsIdle { get; private set; }

        public Nic(IntCodeComp comp, int address)
        {
            Computer = comp;
            Queue = new Queue<(long x, long y)>();
            Address = address;
            IsIdle = false;

            // set first address input
            Computer.GetInput = () =>
            {
                // set packet queue input
                Computer.GetInput = () =>
                {
                    Computer.Pause = true;

                    if (Queue.Count == 0)
                    {
                        IsIdle = true;
                        return -1;
                    }
                    else
                    {
                        IsIdle = false;

                        if (processInX)
                        {
                            processInX = false;
                            return Queue.Peek().x;
                        }
                        else
                        {
                            processInX = true;
                            return Queue.Dequeue().y;
                        }
                    }
                };

                Computer.Pause = true;
                return Address;
            };  

            Computer.WriteOutput = (output) =>
            {
                if (processOutAdd)
                {
                    processOutAdd = false;
                    processOutX = true;
                    packet = (output, -1, -1);
                }
                else if (processOutX)
                {
                    processOutX = false;
                    packet.x = output;
                }
                else
                {
                    processOutAdd = true;
                    packet.y = output;

                    SendPacket(packet);
                }
            };
        }

        public void Run()
        {
            if (Computer.IsRunning)
                Computer.ContinueProgram();
            else
                Computer.RunProgram();
        }
    }
}
