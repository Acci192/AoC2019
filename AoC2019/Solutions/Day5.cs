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

            var inputQueue = new BlockingCollection<int> { 1 };
            var outputQueue = new BlockingCollection<int>();
            var computer = new IntCodeComputer(program, inputQueue, outputQueue);
            computer.Run();
            
            return outputQueue.Last().ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();
            var inputQueue = new BlockingCollection<int> { 5 };
            var outputQueue = new BlockingCollection<int>();
            var computer = new IntCodeComputer(program, inputQueue, outputQueue);
            computer.Run();
            return outputQueue.Take().ToString();
        }
    }
}
