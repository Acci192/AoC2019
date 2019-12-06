using AoC2019.Solutions;
using System;

namespace AoC2019
{
    public class Program
    {
        static void Main(string[] args)
        {
            Func<string, string> methodToRun = Day7.A;
            var input = System.IO.File.ReadAllText($"../../Inputs/{methodToRun.Method.DeclaringType?.Name}.txt");

            var result = methodToRun(input);

            Console.WriteLine(result);

            Console.Write("\nIf you want to test the execution time of the current method, enter a number of iterations: ");
            var readLine = Console.ReadLine();

            if (!int.TryParse(readLine, out var numIterations)) return;

            Benchmark.DisplayTimerProperties();
            Console.WriteLine();
            Benchmark.TimeOperations(methodToRun, input, numIterations);
            Console.ReadKey();
        }
    }
}
