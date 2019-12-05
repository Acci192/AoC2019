using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class IntCodeComputer
    {
        public List<int> Inputs { get; set; }
        private int inputCounter = 0;
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

        public void Run(params int[] inputs)
        {
            Inputs = inputs.ToList();
            while (ExecuteOperation() || ProgramCounter >= Memory.Count) ;
        }

        private bool ExecuteOperation()
        {
            var opCode = Memory[ProgramCounter] % 100;
            var arguments = ParseArgument(opCode);
            switch (opCode)
            {
                case 1:
                    Memory[GetValueFromArgument(arguments[2], true)] = GetValueFromArgument(arguments[0]) + GetValueFromArgument(arguments[1]);
                    IncrementProgramCounter(4);
                    break;
                case 2:
                    Memory[GetValueFromArgument(arguments[2], true)] = GetValueFromArgument(arguments[0]) * GetValueFromArgument(arguments[1]);
                    IncrementProgramCounter(4);
                    break;
                case 3:
                    Memory[GetValueFromArgument(arguments[0], true)] = GetInputValue();
                    IncrementProgramCounter(2);
                    break;
                case 4:
                    Console.WriteLine(GetValueFromArgument(arguments[0]));
                    IncrementProgramCounter(2);
                    break;
                case 5:
                    if(GetValueFromArgument(arguments[0]) == 0)
                        IncrementProgramCounter(3);
                    else
                        ProgramCounter = GetValueFromArgument(arguments[1]);
                    break;
                case 6:
                    if (GetValueFromArgument(arguments[0]) != 0)
                        IncrementProgramCounter(3);
                    else
                        ProgramCounter = GetValueFromArgument(arguments[1]);
                    break;
                case 7:
                    Memory[GetValueFromArgument(arguments[2], true)] = GetValueFromArgument(arguments[0]) < GetValueFromArgument(arguments[1]) ? 1 : 0;
                    IncrementProgramCounter(4);
                    break;
                case 8:
                    Memory[GetValueFromArgument(arguments[2], true)] = GetValueFromArgument(arguments[0]) == GetValueFromArgument(arguments[1]) ? 1 : 0;
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

        private List<Argument> ParseArgument(int opCode)
        {
            var information = new List<Argument>();
            var fullOpCode = Memory[ProgramCounter];
            var numOfArguments = 0;
            switch (opCode)
            {
                case 99:
                    break;
                case 3:
                case 4:
                    numOfArguments = 1;
                    break;
                case 5:
                case 6:
                    numOfArguments = 2;
                    break;
                case 1:
                case 2:
                case 7:
                case 8:
                    numOfArguments = 3;
                    break;
            }

            var argumentModeHelper = 100;
            for(var i = 0; i < numOfArguments; i++)
            {
                var argumentMode = (fullOpCode / argumentModeHelper) % 10;
                information.Add(new Argument { Mode = argumentMode, Value = GetValueAt(ProgramCounter + i + 1) });
                argumentModeHelper *= 10;
            }
            return information;
        }

        private int GetValueFromArgument(Argument argument, bool MemoryPosition = false)
        {
            if (MemoryPosition)
                return argument.Value;
            switch (argument.Mode)
            {
                case 0:
                    return Memory[argument.Value];
                case 1:
                    return argument.Value;
                default:
                    throw new Exception("Invalid Argument Mode");
            }
        }

        private int GetInputValue()
        {
            if (Inputs.Count > inputCounter)
                return Inputs[inputCounter++];
            else
            {
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var result))
                    return result;
                throw new Exception($"Input needs to be an integer. Input from console was '{userInput}'");
            }
        }

        private class Argument
        {
            public int Mode { get; set; }
            public int Value { get; set; }
        }
    }
}
