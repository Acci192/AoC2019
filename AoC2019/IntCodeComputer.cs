using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019
{
    public class IntCodeComputer
    {
        private const string LogFilePath = "LogFile.txt";
        private const string BackupFolderPath = "Backup";
        private LogLevel _loggingEnabled;
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

        public IntCodeComputer(List<long> program, int extraMemory = 1000000, LogLevel loggingEnabled = LogLevel.None)
        {
            Memory = new List<long>(program);
            
            Memory.AddRange(new long[extraMemory]);
            InputQueue = new BlockingCollection<long>();
            _loggingEnabled = loggingEnabled;
            if (_loggingEnabled != LogLevel.None)
            {
                if (File.Exists(LogFilePath))
                {
                    Directory.CreateDirectory(BackupFolderPath);
                    var destination = $"{BackupFolderPath}/{Path.GetFileNameWithoutExtension(LogFilePath)}_{DateTime.Now:yyyyMMddTHHmmss}.txt";
                    File.Copy(LogFilePath, destination);
                }
                File.WriteAllText(LogFilePath, string.Empty);
            }
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
            LogOperationInfo();
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
                    //Memory[memoryA] = int.Parse(Console.ReadLine());
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

        private void LogOperationInfo()
        {
            if (_loggingEnabled == LogLevel.None)
                return;

            var logString = new StringBuilder();
            var opCode = Memory[PC] % 100;

            var modeA = (Memory[PC] / 100) % 10;
            var modeB = (Memory[PC] / 1000) % 10;
            var modeC = (Memory[PC] / 10000) % 10;

            var memoryA = GetMemoryAddress(1, modeA);
            var memoryB = GetMemoryAddress(2, modeB);
            var memoryC = GetMemoryAddress(3, modeC);

            var valueA = memoryA > 0 ? GetValueAt(memoryA) : int.MinValue;
            var valueB = memoryB > 0 ? GetValueAt(memoryB) : int.MinValue;
            var valueC = memoryC > 0 ? GetValueAt(memoryC) : int.MinValue;

            if(_loggingEnabled == LogLevel.Debug)
            {
                logString.AppendLine($"OpCode: {opCode}, PC: {PC}, RelativeBase: {RelativeBase}");
                logString.AppendLine($"\tParameter A: Mode '{modeA}', MemoryAddress '{memoryA}', ValueA {valueA}");
                logString.AppendLine($"\tParameter B: Mode '{modeB}', MemoryAddress '{memoryB}', ValueB {valueB}");
                logString.AppendLine($"\tParameter C: Mode '{modeC}', MemoryAddress '{memoryC}', ValueC {valueC}");
            }
            
            switch (opCode)
            {
                case 1: //Add: c = a + b
                    logString.AppendLine($"\t\tMemory[{memoryC}] = {valueA} + {valueB}");
                    break;
                case 2: //Multiply: c = a * b
                    logString.AppendLine($"\t\tMemory[{memoryC}] = {valueA} * {valueB}");
                    break;
                case 3: // Read: a = [input]
                    logString.AppendLine($"\t\tMemory[{memoryA}] = {InputQueue.First()}");
                    break;
                case 4: // Write: [output] = a
                    logString.AppendLine($"\t\tOutput = {valueA}");
                    break;
                case 5: //Jump-if-true: if a != 0 ? Pc = b : Pc += 3
                    if (valueA != 0)
                        logString.AppendLine($"\t\tPC = {valueB}");
                    break;
                case 6://Jump-if-false: if a == 0 ? Pc = b : Pc += 3
                    if (valueA == 0)
                        logString.AppendLine($"\t\tPC = {valueB}");
                    break;
                case 7: //Less than: if a < b ? c = 1 : c = 0
                    if(valueA < valueB)
                        logString.AppendLine($"\t\tMemory[{memoryC}] = 1");
                    else
                        logString.AppendLine($"\t\tMemory[{memoryC}] = 0");
                    break;
                case 8: //Equals: if a == b ? c = 1 : c = 0
                    if (valueA == valueB)
                        logString.AppendLine($"\t\tMemory[{memoryC}] = 1");
                    else
                        logString.AppendLine($"\t\tMemory[{memoryC}] = 0");
                    break;
                case 9: //Adjust Relative base: RelativeBase = a
                    logString.AppendLine($"\t\tRelative base = {valueA}");
                    break;
                case 99: //Halt
                    logString.AppendLine($"\t\tHALT");
                    break;
                default:
                    throw new Exception($"OpCode: '{opCode}' is not implemented yet");
            }
            File.AppendAllText(LogFilePath, logString.ToString());
        }

        public enum LogLevel
        {
            None,
            Info,
            Debug
        }
    }
}
