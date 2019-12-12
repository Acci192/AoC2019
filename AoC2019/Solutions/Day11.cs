using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AoC2019.Solutions
{
    public class Day11
    {
        public static Dictionary<(int, int), int> paintedPanels;
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic(computer.InputQueue, computer.OutputQueue); });

            paintedPanels = new Dictionary<(int, int), int>();
            computerThread.Start();
            programThread.Start();
            computerThread.Join();
            programThread.Abort();
            return paintedPanels.Count.ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic(computer.InputQueue, computer.OutputQueue); });

            paintedPanels = new Dictionary<(int, int), int> { [(0, 0)] = 1 };
            computerThread.Start();
            programThread.Start();
            computerThread.Join();
            programThread.Abort();
            var whitePanels =  paintedPanels.Where(x => x.Value == 1).ToDictionary(x => x.Key, x => x.Value);

            for (var y = whitePanels.Min(panel => panel.Key.Item2); y <= whitePanels.Max(panel => panel.Key.Item2); y++)
            {
                for(var x = whitePanels.Min(panel => panel.Key.Item1); x <= whitePanels.Max(panel => panel.Key.Item1); x++)
                {
                    if (whitePanels.ContainsKey((x, y)))
                        Console.Write('#');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
            return string.Empty;
        }

        private static void HandleLogic(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            var x = 0;
            var y = 0;
            var direction = 0;
            while (true)
            {
                if (paintedPanels.TryGetValue((x, y), out var currentPanel))
                    inputQueue.Add(currentPanel);
                else
                    inputQueue.Add(0);

                var newColor = (int)outputQueue.Take();
                var turn = (int)outputQueue.Take();
                paintedPanels[(x, y)] = newColor;

                direction = turn == 0 ? (direction + 3) % 4 : (direction + 1) % 4;

                switch (direction)
                {
                    case 0:
                        y--;
                        break;
                    case 1:
                        x++;
                        break;
                    case 2:
                        y++;
                        break;
                    case 3:
                        x--;
                        break;
                }
            }
        }
    }
}
