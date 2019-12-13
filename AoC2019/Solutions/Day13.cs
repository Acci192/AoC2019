using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day13
    {
        private static int Blocks = 0;
        private static int Score = 0;
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { CountBlocks(computer.InputQueue, computer.OutputQueue); });
            
            computerThread.Start();
            programThread.Start();
            computerThread.Join();
            programThread.Abort();
            return Blocks.ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();
            var computer = new IntCodeComputer(program);
            var computerThread = new Thread(f => { computer.Run(); });
            var programThread = new Thread(f => { Play(computer.InputQueue, computer.OutputQueue); });
            computer.SetValueAt(0, 2);
            computerThread.Start();
            programThread.Start();
            computerThread.Join();
            programThread.Abort();
            return Score.ToString();
        }

        private static void Play(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            var paddleX = 0;
            var ballX = 0;
            while (true)
            {
                var X = (int)outputQueue.Take();
                var Y = (int)outputQueue.Take();
                var content = (int)outputQueue.Take();

                Score = X == -1 && Y == 0 ? content : Score;
                paddleX = content == 3 ? X : paddleX;

                if (content == 4)
                {
                    ballX = X;
                    if (paddleX > ballX)
                        inputQueue.Add(-1);
                    else if (paddleX < ballX)
                        inputQueue.Add(1);
                    else
                        inputQueue.Add(0);
                }
            }
        }

        private static void CountBlocks(BlockingCollection<long> inputQueue, BlockingCollection<long> outputQueue)
        {
            while (true)
            {
                var x = (int)outputQueue.Take();
                var y = (int)outputQueue.Take();
                var content = (int)outputQueue.Take();
                Blocks = content == 2 ? Blocks += 1 : Blocks;
            }
        }
    }
}
