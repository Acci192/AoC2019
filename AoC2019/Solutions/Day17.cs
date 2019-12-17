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
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic(computer.InputQueue, computer.OutputQueue); });

            computerThread.Start();
            programThread.Start();

            computerThread.Join();
            programThread.Join();
            //programThread.Abort();

            //var openPaths = 0;
            //for (var y = 0; y < Height; y++)
            //{
            //    for (var x = 0; x < Width; x++)
            //    {
            //        if (Grid[x, y] == '.')
            //            openPaths++;
            //    }
            //}
            
            return string.Empty;
        }

        private static void HandleLogic(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            var x = 1;
            var y = 1;
            var width = 0;
            while (true)
            {
                var output = outputQueue.Take();
                if (output == long.MinValue)
                    break;
                if (output == 10)
                {
                    y++;
                    if(width == 0)
                        width = x;
                    x = 1;
                    continue;
                }
                Grid[x, y] = (char)output;
                x++;
            }

            var height = y;



            var intersections = new List<(int, int)>();
            for(var gy = 1; gy < height; gy++)
            {
                for(var gx = 1; gx < width; gx++)
                {
                    if (Grid[gx, gy - 1] == '#'
                        && Grid[gx, gy + 1] == '#'
                        && Grid[gx - 1, gy] == '#'
                        && Grid[gx + 1, gy] == '#'
                        && Grid[gx, gy] == '#')
                        intersections.Add((gx, gy));
                    Console.Write(Grid[gx, gy]);
                }
                Console.WriteLine();
            }

            var sum = 0;
            foreach(var i in intersections)
            {
                sum += (i.Item1 - 1) * (i.Item2 - 1);
            }
            Console.WriteLine(sum);
        }

        public static string B(string input)
        {
            var rows = input.Split('\n');
            return string.Empty;
        }
    }
}
