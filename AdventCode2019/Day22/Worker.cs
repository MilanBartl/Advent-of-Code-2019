using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2019.Day22
{
    public class Worker : IWorker
    {
        public object Work1()
        {
            //var deck = Enumerable.Range(0, 10).ToArray();
            var deck = Enumerable.Range(0, 10007).ToArray();
            var commands = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
                deck = ProcessCommand(command, deck);

            // display result
            /*for (int i = 0; i < deck.Length; i++)
            {
                Console.Write($"{deck[i]} ");
            }*/

            return deck.ToList().IndexOf(2019);
        }

        public object Work2()
        {
            var deck = Enumerable.Range(0, 10).ToArray();
            //var deck = Enumerable.Range(0, 119315717510047).ToArray();
            var commands = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
                deck = ProcessCommand(command, deck);

            return deck.ToList().IndexOf(2020);
        }

        private int[] ProcessCommand(string command, int[] deck)
        {        
            var buffer = new int[deck.Length];

            if (command.Contains("increment"))
            {
                int increment = int.Parse(Regex.Match(command, @"-?\d+$").Value);
                int index = 0;

                if (increment > 0)
                {
                    for (int i = 0; i < deck.Length; i++)
                    {
                        buffer[index] = deck[i];

                        if (index + increment > deck.Length)
                            index = index + increment - deck.Length;
                        else
                            index += increment;
                    }
                }
                else
                {
                    for (int i = 0; i < deck.Length; i++)
                    {
                        buffer[index] = deck[i];

                        if (index + increment <= 0)
                            index = index + increment + deck.Length;
                        else
                            index += increment;
                    }
                }
            }
            else if (command.Contains("cut"))
            {
                int cut = int.Parse(Regex.Match(command, @"-?\d+$").Value);

                if (cut > 0)
                {
                    int[] cutarray = new int[cut];
                    Array.Copy(deck, cutarray, cut);
                    Array.Copy(deck, cut, buffer, 0, deck.Length - cut);
                    Array.Copy(cutarray, 0, buffer, deck.Length - cut, cut);
                }
                else
                {
                    cut = Math.Abs(cut);
                    int[] cutarray = new int[cut];
                    Array.Copy(deck, deck.Length - cut, cutarray, 0, cut);
                    Array.Copy(cutarray, 0, buffer, 0, cut);
                    Array.Copy(deck, 0, buffer, cut, deck.Length - cut);
                }
            }
            else // new stack
            {
                for (int i = 0; i < deck.Length; i++)
                {
                    buffer[i] = deck[deck.Length - 1 - i];
                }
            }
            return buffer;
        }

        private const string test = @"deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1";

        private const string input = @"deal with increment 64
deal into new stack
cut 1004
deal with increment 31
cut 5258
deal into new stack
deal with increment 5
cut -517
deal with increment 67
deal into new stack
cut -4095
deal with increment 27
cut 4167
deal with increment 30
cut -5968
deal into new stack
deal with increment 40
deal into new stack
deal with increment 57
cut -5128
deal with increment 75
deal into new stack
deal with increment 75
cut -1399
deal with increment 12
cut -2107
deal with increment 9
cut -7110
deal into new stack
deal with increment 14
cut 3318
deal into new stack
deal with increment 57
cut -8250
deal with increment 5
deal into new stack
cut 903
deal with increment 28
deal into new stack
cut 2546
deal with increment 68
cut 9343
deal with increment 67
cut -6004
deal with increment 24
deal into new stack
cut -816
deal with increment 66
deal into new stack
deal with increment 13
cut 5894
deal with increment 43
deal into new stack
cut 4550
deal with increment 67
cut -3053
deal with increment 42
deal into new stack
deal with increment 32
cut -5985
deal with increment 18
cut -2808
deal with increment 44
cut -1586
deal with increment 16
cut 2173
deal with increment 53
cut 5338
deal with increment 48
cut -2640
deal with increment 36
deal into new stack
deal with increment 13
cut -5520
deal with increment 61
cut -3199
deal into new stack
cut 4535
deal with increment 17
cut -4277
deal with increment 72
cut -7377
deal into new stack
deal with increment 37
cut 6665
deal into new stack
cut 908
deal into new stack
cut 9957
deal with increment 31
cut 9108
deal with increment 44
cut -7565
deal with increment 33
cut -7563
deal with increment 23
cut -3424
deal with increment 63
cut -3513
deal with increment 74";
    }
}
