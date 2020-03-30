using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2019.Day14
{
    public class Worker : IWorker
    {
        private List<Equation> Equations { get; set; }

        private Dictionary<string, int> Leftovers { get; set; }

        public object Work1()
        {
            var splits = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Equations = new List<Equation>();
            foreach (string split in splits)
            {
                Equations.Add(new Equation(split));
            }

            var fuelEq = Equations.Single(e => e.Result.name == "FUEL");

            Leftovers = new Dictionary<string, int>();

            long producedOre = GetRequirements(fuelEq, 1);

            return producedOre;
        }

        public object Work2()
        {
            var splits = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Equations = new List<Equation>();
            foreach (string split in splits)
            {
                Equations.Add(new Equation(split));
            }

            Leftovers = new Dictionary<string, int>();

            var fuelEq = Equations.Single(e => e.Result.name == "FUEL");

            int counter = 0;
            long ore = 0;
            do
            {
                ore += GetRequirements(fuelEq, 1);
                counter++;
            } while (counter < 1000000);

            return Math.Ceiling((1000000000000 / (double)ore) * counter);
        }


        private long GetRequirements(Equation eq, int desiredQuantity)
        {
            var res = eq.Result;

            // get spare result resources
            int spare = 0;
            if (Leftovers.ContainsKey(res.name))
            {
                spare = Leftovers[res.name];
                Leftovers.Remove(res.name);
            }

            // calculate required amount of result resource 
            int partialAmount = CalculateAmount(desiredQuantity, res.amount, spare, out int leftover);

            // add leftover to dictionary
            if (leftover > 0)
            {
                Leftovers.Add(res.name, leftover);
            }

            // return sum of ORE required
            if (eq.Reactants.First().Key == "ORE")
            {
                //Console.WriteLine($"Consume {partialAmount * eq.Reactants.First().Value} ORE to create {desiredQuantity} {res.name}");

                return partialAmount * eq.Reactants.First().Value;
            }
            else // further conversion to ORE is required
            {
                long amount = 0;
                foreach (var reactant in eq.Reactants)
                {
                    var producer = Equations.Single(e => e.Result.name == reactant.Key);

                    //Console.WriteLine($"Consume {partialAmount * reactant.Value} {reactant.Key} to create {desiredQuantity} {res.name}");

                    for (int i = 0; i < partialAmount; i++)
                    {
                        amount += GetRequirements(producer, reactant.Value);
                    }
                }
                return amount;
            }
        }

        private int CalculateAmount(int desiredQuantity, int production, int spare, out int leftover)
        {
            int amount = 0;
            int quantity = 0;
            while (desiredQuantity - spare > quantity)
            {
                amount++;
                quantity += production;
            }

            leftover = quantity - desiredQuantity + spare;
            return amount;
        }

        private const string smallTest = @"10 ORE => 10 A
1 ORE => 1 B
7 A, 1 B => 1 C
7 A, 1 C => 1 D
7 A, 1 D => 1 E
7 A, 1 E => 1 FUEL";

        private const string mediumTest = @"9 ORE => 2 A
8 ORE => 3 B
7 ORE => 5 C
3 A, 4 B => 1 AB
5 B, 7 C => 1 BC
4 C, 1 A => 1 CA
2 AB, 3 BC, 4 CA => 1 FUEL";

        private const string largerTest = @"157 ORE => 5 NZVS
165 ORE => 6 DCFZ
44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
179 ORE => 7 PSHF
177 ORE => 5 HKGWZ
7 DCFZ, 7 PSHF => 2 XJWVT
165 ORE => 2 GPVTF
3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT";

        private const string largeTest = @"171 ORE => 8 CNZTR
7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
114 ORE => 4 BHXH
14 VRPVC => 6 BMBT
6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL
6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
5 BMBT => 4 WPTQ
189 ORE => 9 KTJDG
1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
12 VRPVC, 27 CNZTR => 2 XDBXC
15 KTJDG, 12 BHXH => 5 XCVML
3 BHXH, 2 VRPVC => 7 MZWV
121 ORE => 7 VRPVC
7 XCVML => 6 RJRHP
5 BHXH, 4 VRPVC => 5 LTCX";

        private const string input = @"1 XVCBM, 12 SWPQ => 7 VMWSR
10 SBLTQ, 14 TLDR => 6 HJFPQ
1 VWHXC, 2 GZDQ, 3 PCLMJ => 4 VJPLN
9 MGVG => 7 WDPF
1 FBXD, 5 FZNZR => 6 GZDQ
5 TJPZ, 1 QNMZ => 5 SWPQ
12 XWQW, 1 HJFPQ => 8 JPKNC
15 CPNC, 2 TXKRN, 2 MTVQD => 9 LBRSX
5 VJPLN, 1 VSTRK, 2 GFQLV => 5 NLZKH
1 TLDR => 4 TNRZW
2 VCFM => 7 FZNZR
1 PSTRV, 5 RTDV => 8 VCFM
2 PSTRV => 9 SFWJG
4 XWQW => 2 BHPS
1 ZWFNW, 19 JKRWT, 2 JKDL, 8 PCLMJ, 7 FHNL, 22 MSZCF, 1 VSTRK, 7 DMJPR => 1 ZDGF
22 XVCBM, 8 TBLM => 1 MTVQD
101 ORE => 1 WBNWZ
6 VNVXJ, 1 FBXD, 13 PCLMJ => 9 MGVG
13 SHWB, 1 WDPF, 4 QDTW => 6 FHNL
9 VSTRK => 2 VZCML
20 LZCDB => 7 KNPM
2 LBRSX, 9 GRCD => 3 SHWB
5 BHPS => 6 SQJLW
1 RTDV => 6 GRCD
6 SBLTQ, 6 XWQW => 5 CPNC
153 ORE => 3 RTDV
6 LZCDB, 1 SBLTQ => 3 PCLMJ
1 RTDV, 2 TJPZ => 5 LZCDB
24 QNMZ => 4 TXKRN
19 PCLMJ, 7 VNVXJ => 6 RKRVJ
12 RKRVJ, 11 QNMZ => 3 JKRWT
4 SFWJG => 9 FBXD
16 WDPF, 4 TXKRN => 6 DMJPR
3 QNMZ => 1 VSTRK
9 VSTRK => 4 ZWFNW
7 QBWN, 1 TLDR => 4 QDTW
7 VJPLN, 1 NLZKH, 15 JPKNC, 3 SHWB, 1 MSZCF, 3 VMWSR => 6 QDHGS
14 QXQZ => 7 XWQW
152 ORE => 9 TJPZ
1 PJVJ, 10 QBWN, 19 NLZKH => 6 MSZCF
21 TLDR, 13 VNVXJ, 5 BHPS => 4 QBWN
1 GZDQ, 6 GRCD => 9 TLDR
4 BHPS => 8 MZBL
1 FZNZR => 2 VNVXJ
1 VNVXJ => 5 GFQLV
13 LZCDB => 2 QXQZ
3 MNFJX => 5 VWHXC
1 GZDQ, 2 VMWSR => 6 WZMHW
9 HJFPQ, 3 RKRVJ => 4 QNMZ
8 TJPZ => 9 SBLTQ
30 WBNWZ => 5 TBLM
1 PCLMJ => 3 GNMTQ
30 SQJLW, 3 QNMZ, 9 WDPF => 5 PJVJ
10 GRCD, 15 SBLTQ, 22 GFQLV => 4 XVCBM
30 PJVJ, 10 JPKNC, 3 DXFDR, 10 VZCML, 59 MZBL, 40 VWHXC, 1 ZDGF, 13 QDHGS => 1 FUEL
4 GNMTQ, 6 VMWSR, 19 RKRVJ, 5 FKZF, 4 VCFM, 2 WZMHW, 7 KNPM, 5 TNRZW => 7 DXFDR
152 ORE => 9 PSTRV
2 BHPS, 5 TXKRN, 2 PJVJ => 4 FKZF
2 XWQW, 2 VCFM, 13 BHPS => 8 MNFJX
3 XWQW => 2 JKDL";
    }
}
