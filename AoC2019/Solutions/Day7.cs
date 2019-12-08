using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AoC2019.Solutions
{
    public class Day7
    {
        public static string A(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();
            var max = 0;
            var settings = UniquePhaseSettings(0, 4);

            foreach (var setting in settings)
            {
                var computerA = new IntCodeComputer(program);
                computerA.AddToInput(setting[0], 0);
                computerA.Run();

                var computerB = new IntCodeComputer(program);
                computerB.AddToInput(setting[1], computerA.OutputQueue.Take());
                computerB.Run();

                var computerC = new IntCodeComputer(program);
                computerC.AddToInput(setting[2], computerB.OutputQueue.Take());
                computerC.Run();

                var computerD = new IntCodeComputer(program);
                computerD.AddToInput(setting[3], computerC.OutputQueue.Take());
                computerD.Run();

                var computerE = new IntCodeComputer(program);
                computerE.AddToInput(setting[4], computerD.OutputQueue.Take());
                computerE.Run();

                var value = computerE.OutputQueue.Take();
                if (value > max)
                    max = value;
            }
            
            return max.ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();
            var max = 0;
            var settings = UniquePhaseSettings(5, 9);

            foreach(var setting in settings)
            {
                var computerA = new IntCodeComputer(program);
                var computerB = new IntCodeComputer(program) { InputQueue = computerA.OutputQueue };
                var computerC = new IntCodeComputer(program) { InputQueue = computerB.OutputQueue };
                var computerD = new IntCodeComputer(program) { InputQueue = computerC.OutputQueue };
                var computerE = new IntCodeComputer(program) { InputQueue = computerD.OutputQueue };
                computerA.InputQueue = computerE.OutputQueue;
                computerA.AddToInput(setting[0], 0);
                computerB.AddToInput(setting[1]);
                computerC.AddToInput(setting[2]);
                computerD.AddToInput(setting[3]);
                computerE.AddToInput(setting[4]);

                new Thread(f => { computerA.Run(); }).Start();
                new Thread(f => { computerB.Run(); }).Start();
                new Thread(f => { computerC.Run(); }).Start();
                new Thread(f => { computerD.Run(); }).Start();
                var threadE = new Thread(f => { computerE.Run(); });
                threadE.Start();
                threadE.Join();
                var value = computerE.OutputQueue.Take();
                if (value > max)
                    max = value;
            }
            
            return max.ToString();
        }

        private static List<int[]> UniquePhaseSettings(int low, int high)
        {
            var settings = new List<int[]>();
            for (var aPhase = low; aPhase <= high; aPhase++)
            for (var bPhase = low; bPhase <= high; bPhase++)
            for (var cPhase = low; cPhase <= high; cPhase++)
            for (var dPhase = low; dPhase <= high; dPhase++)
            for (var ePhase = low; ePhase <= high; ePhase++)
            {
                var uniquePhaseSetting = new HashSet<int> { aPhase, bPhase, cPhase, dPhase, ePhase };
                if (uniquePhaseSetting.Count == 5)
                    settings.Add(new[] { aPhase, bPhase, cPhase, dPhase, ePhase });
            }
            return settings;
        }
    }
}
