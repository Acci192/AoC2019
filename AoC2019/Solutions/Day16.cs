using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day16
    {
        public static string A(string input)
        {
            var basePattern = new[] { 0, 1, 0, -1 };

            for(var phase = 0; phase < 100; phase++)
            {
                var inputSignal = input.Select(c => int.Parse($"{c}")).ToList();
                var newSignal = new List<int>();
                for (var j = 1; j <= inputSignal.Count; j++)
                {
                    var pattern = new List<int>();
                    while (pattern.Count <= inputSignal.Count)
                    {
                        foreach (var p in basePattern)
                        {
                            pattern.AddRange(Enumerable.Repeat(p, j));
                        }
                    }
                    pattern = pattern.Skip(1).Take(inputSignal.Count).ToList();

                    newSignal.Add(Math.Abs(inputSignal.Select((x, i) => x * pattern[i]).Sum()) % 10);
                }
                input = string.Join("", newSignal);
            }
            
            return input.Substring(0, 8);
        }

        public static string B(string input)
        {
            var internalOffset = int.Parse(input.Substring(0, 7));
            input = string.Join("", Enumerable.Repeat(input, 10000)).Substring(internalOffset);

            var inputLength = input.Length;
            var inputSignal = input.Select(c => int.Parse($"{c}")).ToList();
            for (var phase = 0; phase < 100; phase++)
            {
                var lastValue = inputSignal[inputLength - 1];
                var newSignal = new List<int> { lastValue };
                for (var offset = inputLength - 2; offset >= 0; offset--)
                {
                    newSignal.Add((newSignal[newSignal.Count - 1] + inputSignal[offset]) % 10);
                }
                newSignal.Reverse();
                inputSignal = newSignal;
            }

            return string.Join("", inputSignal.Take(8));
        }

    }
}
