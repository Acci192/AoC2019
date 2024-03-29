﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day3
    {
        public static string A(string input)
        {
            var wires = input.Split('\n').Select(wire => wire.Split(',').Select(x => new Instruction(x)).ToList()).ToList();
            return FindShortestDistance(wires, true);
        }

        public static string B(string input)
        {
            var wires = input.Split('\n').Select(wire => wire.Split(',').Select(x => new Instruction(x)).ToList()).ToList();
            return FindShortestDistance(wires, false);
        }

        private static string FindShortestDistance(List<List<Instruction>> wires, bool useManhattan)
        {
            var occupiedSpace = new Dictionary<(int, int), int>();

            var shortestPath = int.MaxValue;
            for (var wireIndex = 0; wireIndex < 2; wireIndex++)
            {
                var x = 0;
                var y = 0;
                var counter = 0;
                foreach (var instruction in wires[wireIndex])
                {
                    for (var i = 0; i < instruction.Length; i++)
                    {
                        counter++;
                        switch (instruction.Direction)
                        {
                            case 'R':
                                x++;
                                break;
                            case 'L':
                                x--;
                                break;
                            case 'U':
                                y--;
                                break;
                            case 'D':
                                y++;
                                break;
                        }

                        if (wireIndex == 0 && !occupiedSpace.ContainsKey((x, y)))
                            occupiedSpace.Add((x, y), counter);
                        else if (wireIndex != 0 && occupiedSpace.ContainsKey((x, y)))
                        {
                            var distance = useManhattan ? Math.Abs(x) + Math.Abs(y) : occupiedSpace[(x, y)] + counter;
                            shortestPath = distance < shortestPath ? distance : shortestPath;
                        }
                    }
                }
            }
            return shortestPath.ToString();
        }

        private class Instruction
        {
            public char Direction { get; set; }
            public int Length { get; set; }

            public Instruction(string instruction)
            {
                Direction = instruction.First();
                Length = int.Parse(instruction.Substring(1));
            }
        }
    }
}
