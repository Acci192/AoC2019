using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class IntCodeComputer
    {
        public List<int> Memory { get; set; }
        public int ProgramCounter { get; set; } = 0;
        public int StaticIncrement { get; set; }

        public IntCodeComputer(List<int> program, int staticIncrement = 0)
        {
            Memory = new List<int>(program);
            StaticIncrement = staticIncrement;
        }

        public int GetValueAt(int index)
        {
            return Memory[index];
        }

        public void SetValueAt(int index, int value)
        {
            Memory[index] = value;
        }

        public void Run()
        {
            while (ExecuteOperation() || ProgramCounter >= Memory.Count) ;
        }

        private bool ExecuteOperation()
        {
            var opCode = GetValueAt(ProgramCounter);
            var a = GetValueAt(ProgramCounter + 1);
            var b = GetValueAt(ProgramCounter + 2);
            var c = GetValueAt(ProgramCounter + 3);

            switch (opCode)
            {
                case 1:
                    Memory[c] = Memory[a] + Memory[b];
                    IncrementProgramCounter(4);
                    break;
                case 2:
                    Memory[c] = Memory[a] * Memory[b];
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
            ProgramCounter = StaticIncrement == 0 ? ProgramCounter + increment : ProgramCounter + StaticIncrement;
        }
    }
}
