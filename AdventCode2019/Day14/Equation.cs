using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCode2019.Day14
{
    public class Equation
    {
        private string equation = "";

        public (string name, int amount) Result { get; private set; }

        public Dictionary<string, int> Reactants { get; private set; }

        public Equation(string equation)
        {
            this.equation = equation;

            Reactants = new Dictionary<string, int>();

            var sides = equation.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);

            var splits = Regex.Match(sides[1], @"\d+ [A-Z]+").Value.Split(' ');
            Result = (splits[1], int.Parse(splits[0]));

            foreach (var reactant in sides[0].Split(','))
            {
                splits = Regex.Match(reactant, @"\d+ [A-Z]+").Value.Split(' ');
                Reactants.Add(splits[1], int.Parse(splits[0]));
            }
        }

        public override string ToString()
        {
            return equation;
        }
    }
}
