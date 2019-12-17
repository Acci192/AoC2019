using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day17
    {
        public static char[,] Grid = new char[75, 75];
        public static string Result_A = string.Empty;
        public static string Result_B = string.Empty;
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic_A(computer.InputQueue, computer.OutputQueue); });
            
            computerThread.Start();
            programThread.Start();

            computerThread.Join();
            programThread.Join();
            return Result_A;
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic_B(computer.InputQueue, computer.OutputQueue); });
            
            computer.SetValueAt(0, 2);
            computerThread.Start();
            programThread.Start();

            computerThread.Join();
            programThread.Join();

            return Result_B;
        }

        private static void HandleLogic_A(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            var x = 1;
            var y = 1;
            var width = 0;
            var count = 0;

            var lastChar = (char)0;
            while (true)
            {
                var output = outputQueue.Take();
                
                if (output == long.MinValue)
                    break;
                if (output == 10)
                {
                    y++;
                    if (width == 0)
                        width = x;
                    x = 1;
                    continue;
                }
                if (lastChar == 10 && output == 10)
                    y = 0;
                lastChar = (char)output;

                Grid[x, y] = (char)output;
                x++;

            }

            var height = y;
            var intersections = new List<(int, int)>();
            for (var gy = 1; gy < height; gy++)
            {
                for (var gx = 1; gx < width; gx++)
                {
                    if (Grid[gx, gy - 1] == '#'
                        && Grid[gx, gy + 1] == '#'
                        && Grid[gx - 1, gy] == '#'
                        && Grid[gx + 1, gy] == '#'
                        && Grid[gx, gy] == '#')
                        intersections.Add((gx, gy));
                    //Console.Write(Grid[gx, gy]);
                }
                //Console.WriteLine();
            }

            Result_A = intersections.Select(i => (i.Item1 - 1) * (i.Item2 - 1)).Sum().ToString();
        }

        private static void HandleLogic_B(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            var input = "A,B,A,C,C,A,B,C,B,B\nL,8,R,10,L,8,R,8\nL,12,R,8,R,8\nL,8,R,6,R,6,R,10,L,8\nn\n";
            input.ToList().ForEach(c => inputQueue.Add(c));
            
            while (true)
            {
                var output = outputQueue.Take();
                if (output > char.MaxValue)
                {
                    Result_B = output.ToString();
                    return;
                }

                //Console.Write((char)output);
            }
        }
    }
}
