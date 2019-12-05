using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day4
    {
        public static string A(string input)
        {
            var range = input.Split('-');
            var lower = int.Parse(range[0]);
            var higher = int.Parse(range[1]);
            var attempt = range[0];

            var counter = 0;
            
            for(int i = lower; i <= higher; i++)
            {
                if (VerifyIncreasingSequence(i.ToString()) && GetTwoAdjecentCharacters(i.ToString()).Count > 0)
                    counter++;
            }
            return counter.ToString();
        }

        public static string B(string input)
        {
            var range = input.Split('-');
            var lower = int.Parse(range[0]);
            var higher = int.Parse(range[1]);

            var counter = 0;

            for (int i = lower; i <= higher; i++)
            {
                if (!VerifyIncreasingSequence(i.ToString()))
                    continue;

                var twos = GetTwoAdjecentCharacters(i.ToString());
                if (!twos.Any())
                    continue;

                var threes = GetThreeAdjecentCharacters(i.ToString());
                if (twos.Where(x => !threes.Contains(x)).Count() > 0)
                    counter++;
            }
            return counter.ToString();
        }

        private static bool VerifyIncreasingSequence(string input)
        {
            for (var i = 1; i < input.Length; i++)
            {
                if (input[i] < input[i - 1])
                    return false;
            }
            return true;
        }

        private static HashSet<char> GetTwoAdjecentCharacters(string input)
        {
            var result = new HashSet<char>();
            for(var i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                    result.Add(input[i]);
            }
            return result;
        }

        private static HashSet<char> GetThreeAdjecentCharacters(string input)
        {
            var result = new HashSet<char>();
            for (var i = 0; i < input.Length - 2; i++)
            {
                if (input[i] == input[i + 1] && input[i] == input[i + 2])
                    result.Add(input[i]);
            }
            return result;
        }
    }
}
