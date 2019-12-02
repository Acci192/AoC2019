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
            return input.Split('\n').Select(x => int.Parse(x) / 3 - 2).Select(Fuel).Sum().ToString();
        }

        private static int Fuel(int weight)
        {
            return weight <= 0 ? 0 : Fuel(weight / 3 - 2) + weight;
        }
    }
}
