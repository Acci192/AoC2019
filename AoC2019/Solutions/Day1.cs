using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day1
    {
        public static string A(string input)
        {
            return input.Split('\n').Select(int.Parse).Sum(x => x/3-2).ToString();
        }

        public static string B(string input)
        {
            return input.Split('\n').Select(int.Parse).Select(x => x / 3 - 2).Select(FuelCounter).Sum().ToString();
        }

        private static int FuelCounter(int moduleWeight)
        {
            var result = 0;
            while (moduleWeight > 0)
            {
                result += moduleWeight;
                moduleWeight = moduleWeight / 3 - 2;
            }
            return result;
        }
    }
}
