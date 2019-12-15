using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day15
    {
        public const int Height = 50;
        public const int Width = 50;
        public static char[,] Grid = new char[Width, Height];
        public static HashSet<(int, int)> DeadEnds = new HashSet<(int, int)>();
        public static (int, int) OxygenTank;
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic(computer.InputQueue, computer.OutputQueue, false); });
            DeadEnds.Clear();


            computerThread.Start();
            programThread.Start();
            
            programThread.Join();
            computerThread.Abort();

            var openPaths = 0;
            for(var y = 0; y < Height; y++)
            {
                for(var x = 0; x < Width; x++)
                {
                    if (Grid[x, y] == '.')
                        openPaths++;
                }
            }

            var result = openPaths - DeadEnds.Count;
            return result.ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { HandleLogic(computer.InputQueue, computer.OutputQueue, true); });
            DeadEnds.Clear();


            computerThread.Start();
            programThread.Start();

            programThread.Join();
            computerThread.Abort();

            var oxygenFront = new List<(int, int)> { OxygenTank };
            var count = 0;
            while (oxygenFront.Any())
            {
                var newFront = new List<(int, int)>();
                foreach(var o in oxygenFront)
                {
                    Grid[o.Item1, o.Item2] = 'O';
                    UpdateOxygenFront(o.Item1, o.Item2, newFront);
                }
                oxygenFront = newFront;
                count++;
            }
            return (count-1).ToString();
        }

        private static void UpdateOxygenFront(int x, int y, List<(int, int)> currentFront)
        {
            var north = (x, y: y - 1);
            var east = (x: x + 1, y);
            var south = (x, y: y + 1);
            var west = (x: x - 1, y);

            if(Grid[north.x, north.y] == '.')
            {
                Grid[north.x, north.y] = 'O';
                currentFront.Add(north);
            }
            if (Grid[east.x, east.y] == '.')
            {
                Grid[east.x, east.y] = 'O';
                currentFront.Add(east);
            }
            if (Grid[south.x, south.y] == '.')
            {
                Grid[south.x, south.y] = 'O';
                currentFront.Add(south);
            }
            if (Grid[west.x, west.y] == '.')
            {
                Grid[west.x, west.y] = 'O';
                currentFront.Add(west);
            }
        }

        private static void HandleLogic(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue, bool fullSearch)
        {
            var x = Width / 2;
            var y = Height / 2;
            var direction = 1;
            while (true)
            {
                direction = ChooseDirection(x, y, direction);
                if (direction == -1)
                    return;
                inputQueue.Add(direction);

                var output = outputQueue.Take();

                switch (output)
                {
                    case 0:
                        if (direction == 1)
                        {
                            Grid[x, y - 1] = '#';
                        } else if(direction == 2)
                        {
                            Grid[x, y + 1] = '#';
                        }else if(direction == 3)
                        {
                            Grid[x - 1, y] = '#';
                        }
                        else
                        {
                            Grid[x + 1, y] = '#';
                        }
                        break;
                    case 1:
                        Grid[x, y] = '.';
                        switch (direction)
                        {
                            case 1:
                                y -= 1;
                                break;
                            case 2:
                                y += 1;
                                break;
                            case 3:
                                x -= 1;
                                break;
                            case 4:
                                x += 1;
                                break;
                        }
                        break;
                    case 2:
                        Grid[x, y] = '.';
                        switch (direction)
                        {
                            case 1:
                                y -= 1;
                                break;
                            case 2:
                                y += 1;
                                break;
                            case 3:
                                x -= 1;
                                break;
                            case 4:
                                x += 1;
                                break;
                        }
                        if (fullSearch)
                        {
                            OxygenTank = (x, y);
                            break;
                        }
                        else
                        {
                            Grid[x, y] = 'O';
                            return;
                        }  
                }
            }
        }

        private static int ChooseDirection(int x, int y, int lastDirection)
        {
            if (Grid[x, y-1] == default(char))
                return 1;
            if (Grid[x+1, y] == default(char))
                return 4;
            if (Grid[x, y+1] == default(char))
                return 2;
            if (Grid[x-1, y] == default(char))
                return 3;
            
            var options = GetOptions(x, y);
            if (options.Count > 1)
                return options.First(o => o != lastDirection);
            else if (options.Count == 1)
                return options.First();
            else
                return -1;
        }

        private static List<int> GetOptions(int x, int y)
        {
            var options = new List<int>();
            var north = (x, y: y - 1);
            var east = (x: x + 1, y);
            var south = (x, y: y + 1);
            var west = (x: x - 1, y);
            if (Grid[north.x, north.y] == '.' && !DeadEnds.Contains(north))
                options.Add(1);
            if (Grid[east.x, east.y] == '.' && !DeadEnds.Contains(east))
                options.Add(4);
            if (Grid[south.x, south.y] == '.' && !DeadEnds.Contains(south))
                options.Add(2);
            if (Grid[west.x, west.y] == '.' && !DeadEnds.Contains(west))
                options.Add(3);

            if (options.Count == 1)
                DeadEnds.Add((x, y));
            return options;
        }
    }
}
