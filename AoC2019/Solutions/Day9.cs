using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day9
    {
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            computer.AddToInput(1);
            computer.Run();
            return computer.OutputQueue.Take().ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            computer.AddToInput(2);
            computer.Run();
            return computer.OutputQueue.Take().ToString();
        }
    }
}
