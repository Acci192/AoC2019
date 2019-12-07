﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019
{
    public class IntCodeComputer
    {
        public BlockingCollection<int> InputQueue { get; set; }
        public BlockingCollection<int> OutputQueue { get; set; }
        public List<int> Memory { get; set; }
        public int PC { get; set; } = 0;
        public string Name { get; set; }

        public IntCodeComputer(List<int> program, BlockingCollection<int> inputQueue = null, BlockingCollection<int> outputQueue = null)
        {
            Memory = new List<int>(program);
            InputQueue = inputQueue;
            OutputQueue = outputQueue;
        }

        public int GetValueAt(int index, int mode = 1)
        {
            switch (mode)
            {
                case 0:
                    return Memory[Memory[index]];
                case 1:
                    return Memory[index];
                default:
                    throw new Exception("Invalid Argument Mode");
            }
        }

        public void SetValueAt(int index, int value)
        {
            Memory[index] = value;
        }

        public void Run()
        {
            while (ExecuteOperation()) ;
        }

        private bool ExecuteOperation()
        {
            var opCode = Memory[PC] % 100;
            var modeOne = (Memory[PC] / 100) % 10;
            var modeTwo = (Memory[PC] / 1000) % 10;
            var modeThree = (Memory[PC] / 10000) % 10;
            switch (opCode)
            {
                case 1:
                    Memory[GetValueAt(PC + 3)] = GetValueAt(PC+1, modeOne) + GetValueAt(PC + 2, modeTwo);
                    IncrementProgramCounter(4);
                    break;
                case 2:
                    Memory[GetValueAt(PC + 3)] = GetValueAt(PC + 1, modeOne) * GetValueAt(PC + 2, modeTwo);
                    IncrementProgramCounter(4);
                    break;
                case 3:
                    Memory[GetValueAt(PC+1)] = InputQueue.Take();
                    IncrementProgramCounter(2);
                    break;
                case 4:
                    OutputQueue.Add(GetValueAt(PC + 1, modeOne));
                    IncrementProgramCounter(2);
                    break;
                case 5:
                    if(GetValueAt(PC + 1, modeOne) == 0)
                        IncrementProgramCounter(3);
                    else
                        PC = GetValueAt(PC + 2, modeTwo);
                    break;
                case 6:
                    if (GetValueAt(PC + 1, modeOne) != 0)
                        IncrementProgramCounter(3);
                    else
                        PC = GetValueAt(PC + 2, modeTwo);
                    break;
                case 7:
                    Memory[GetValueAt(PC + 3)] = GetValueAt(PC + 1, modeOne) < GetValueAt(PC + 2, modeTwo) ? 1 : 0;
                    IncrementProgramCounter(4);
                    break;
                case 8:
                    Memory[GetValueAt(PC + 3)] = GetValueAt(PC + 1, modeOne) == GetValueAt(PC + 2, modeTwo) ? 1 : 0;
                    IncrementProgramCounter(4);
                    break;
                case 99:
                    IncrementProgramCounter(1);
                    return false;
                default:
                    throw new Exception($"OpCode: '{opCode}' is not implemented yet");
            }
            return true;
        }

        private void IncrementProgramCounter(int increment)
        {
            PC = PC + increment;
        }
    }
}
