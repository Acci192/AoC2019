using System;
using System.Collections.Concurrent;
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
            computer.AddToInput(1);
            computer.Run();
            
            return computer.OutputQueue.Last().ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();
            var computer = new IntCodeComputer(program);
            computer.AddToInput(5);
            computer.Run();
            return computer.OutputQueue.Take().ToString();
        }
    }
}
