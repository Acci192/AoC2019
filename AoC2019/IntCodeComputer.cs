using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019
{
    public class IntCodeComputer
    {
        private BlockingCollection<long> _inputQueue;
        public BlockingCollection<long> InputQueue
        {
            get => _inputQueue;
            set
            {
                _inputQueue?.Dispose();
                _inputQueue = value;
            }
        }
        public BlockingCollection<long> OutputQueue { get; } = new BlockingCollection<long>();
        public List<long> Memory { get; }
        private int PC { get; set; } = 0;
        private int RelativeBase { get; set; } = 0;

        public IntCodeComputer(List<long> program, int extraMemory = 1000000)
        {
            Memory = new List<long>(program);
            
            Memory.AddRange(new long[extraMemory]);
            InputQueue = new BlockingCollection<long>();
        }

        public void AddToInput(params long[] inputs)
        {
            foreach(var i in inputs)
            {
                InputQueue.Add(i);
            }
        }

        public long GetValueAt(int memoryAddress)
        {
            return Memory[memoryAddress];
        }


        public int GetMemoryAddress(int parameter, long mode)
        {
            switch (mode)
            {
                case 0:
                    return (int)Memory[PC + parameter];
                case 1:
                    return PC + parameter;
                case 2:
                    return (int)Memory[PC + parameter] + RelativeBase;
                default:
                    throw new Exception("Invalid Argument Mode");
            }
        }

        public void SetValueAt(int index, long value)
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
            var memoryA = GetMemoryAddress(1, (Memory[PC] / 100) % 10);
            var memoryB = GetMemoryAddress(2, (Memory[PC] / 1000) % 10);
            var memoryC = GetMemoryAddress(3, (Memory[PC] / 10000) % 10);

            switch (opCode)
            {
                case 1: //Add: c = a + b
                    Memory[memoryC] = GetValueAt(memoryA) + GetValueAt(memoryB);
                    PC += 4;
                    break;
                case 2: //Multiply: c = a * b
                    Memory[memoryC] = GetValueAt(memoryA) * GetValueAt(memoryB);
                    PC += 4;
                    break;
                case 3: // Read: a = [input]
                    Memory[memoryA] = InputQueue.Take();
                    PC += 2;
                    break;
                case 4: // Write: [output] = a
                    OutputQueue.Add(GetValueAt(memoryA));
                    PC += 2;
                    break;
                case 5: //Jump-if-true: if a != 0 ? Pc = b : Pc += 3
                    PC = GetValueAt(memoryA) != 0 ? (int)GetValueAt(memoryB) : PC + 3;
                    break;
                case 6://Jump-if-false: if a == 0 ? Pc = b : Pc += 3
                    PC = GetValueAt(memoryA) == 0 ? (int)GetValueAt(memoryB) : PC + 3;
                    break;
                case 7: //Less than: if a < b ? c = 1 : c = 0 
                    Memory[memoryC] = GetValueAt(memoryA) < GetValueAt(memoryB) ? 1 : 0;
                    PC += 4;
                    break;
                case 8: //Equals: if a == b ? c = 1 : c = 0
                    Memory[memoryC] = GetValueAt(memoryA) == GetValueAt(memoryB) ? 1 : 0;
                    PC += 4;
                    break;
                case 9: //Adjust Relative base: RelativeBase = a
                    RelativeBase += (int)GetValueAt(memoryA);
                    PC += 2;
                    break;
                case 99: //Halt
                    return false;
                default:
                    throw new Exception($"OpCode: '{opCode}' is not implemented yet");
            }
            return true;
        }
    }
}
