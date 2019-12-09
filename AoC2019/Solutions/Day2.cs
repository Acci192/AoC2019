using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day2
    {
        public static string A(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();

            var computer = new IntCodeComputer(program);
            computer.SetValueAt(1, 12);
            computer.SetValueAt(2, 2);
            computer.Run();
            return computer.GetValueAt(0).ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(long.Parse).ToList();

            var range = Enumerable.Range(0, 99);
            foreach(var noun in range)
            {
                foreach(var verb in range)
                {
                    var computer = new IntCodeComputer(program, 0);
                    computer.SetValueAt(1, noun);
                    computer.SetValueAt(2, verb);
                    computer.Run();
                    if (computer.Memory[0] == 19690720)
                        return (100 * noun + verb).ToString();
                }
            }
            return string.Empty;
        }
    }
}
