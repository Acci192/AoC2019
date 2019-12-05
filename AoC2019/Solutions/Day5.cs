using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day5
    {
        public static string A(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();

            var computer = new IntCodeComputer(program);
            computer.Run(1);
            return string.Empty;
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();

            var computer = new IntCodeComputer(program);
            computer.Run(5);
            return string.Empty;
        }
    }
}
