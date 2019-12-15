using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day14
    {
        public static long UsedOre = 0;
        public static string A(string input)
        {
            UsedOre = 0;
            var rows = input.Replace("\r", "").Split('\n');

            var nodes = new Dictionary<string, int>();
            var connections = new List<Connection>();

            foreach(var row in rows)
            {
                var split = row.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                var froms = split[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                var toInfo = split[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var toNode = toInfo[1];
                var toAmount = int.Parse(toInfo[0]);
                nodes[toNode] = 0;
                foreach(var from in froms)
                {
                    var fromInfo = from.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = fromInfo[1];
                    var fromAmount = int.Parse(fromInfo[0]);
                    nodes[fromNode] = 0;
                    connections.Add(new Connection(fromNode, toNode, fromAmount, toAmount));
                }
            }

            GetAmountAtNode("FUEL", 1, nodes, connections);
            
            return UsedOre.ToString();
        }

        private static void GetAmountAtNode(string node, int amount, Dictionary<string, int> nodes, List<Connection> connections)
        {
            if (node == "ORE")
            {
                UsedOre += amount;
                return;
            }
            if (nodes[node] >= amount)
            {
                nodes[node] -= amount;
                return;
            }
            var neededAmount = amount - nodes[node]; 
            var neededConnections = connections.Where(x => x.To == node).ToList();
            foreach(var c in neededConnections)
            {
                var numberOfCycles = (int)Math.Ceiling((double)neededAmount / c.ToAmount);
                var needed = c.FromAmount * numberOfCycles;
                GetAmountAtNode(c.From, needed, nodes, connections);
                var wasted = c.ToAmount * numberOfCycles - neededAmount;
                nodes[node] = wasted;
            }
        }

        public static string B(string input)
        {
            UsedOre = 0;
            var rows = input.Replace("\r", "").Split('\n');
            var recipes = new List<Recipe>();

            foreach (var row in rows)
            {
                var split = row.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                var froms = split[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                var toInfo = split[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var toNode = toInfo[1];
                var toAmount = int.Parse(toInfo[0]);

                var recipe = new Recipe(toNode, toAmount);

                foreach (var from in froms)
                {
                    var fromInfo = from.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = fromInfo[1];
                    var fromAmount = int.Parse(fromInfo[0]);
                    recipe.Ingredients.Add((fromNode, fromAmount));
                }
                recipes.Add(recipe);
            }


            var result = NeededOre(1, recipes);
            return result.ToString();
        }

        public static int NeededOre(int amountOfFuel, List<Recipe> recipes)
        {
            var neededAmount = recipes.Select(x => x.Type).ToDictionary(x => x, x => 0);

            var leftOvers = recipes.Select(x => x.Type).ToDictionary(x => x, x => 0);

            neededAmount["FUEL"] = amountOfFuel;

            var usedOre = 0;
            while(neededAmount.Any(x => x.Value > 0))
            {
                var neededRecipe = neededAmount.FirstOrDefault(x => x.Value > 0);
                usedOre += UpdateRequirements(neededRecipe.Key, neededRecipe.Value, recipes, neededAmount, leftOvers);
            }
            return usedOre;
        }

        public static int UpdateRequirements(string type, int amount, List<Recipe> recipes, Dictionary<string, int> neededAmounts, Dictionary<string, int> leftOvers)
        {
            if (type == "ORE")
                return amount;
            
            if (leftOvers[type] >= amount)
            {
                neededAmounts[type] -= amount;
                leftOvers[type] -= amount;
                return 0;
            }
            else if (leftOvers[type] > 0)
            {
                amount = amount - leftOvers[type];
                leftOvers[type] = 0;
            }
            
            var recipe = recipes.First(x => x.Type == type);
            var numOfReps = (int)Math.Ceiling((double)amount / recipe.Amount);
            leftOvers[type] = (recipe.Amount - amount - leftOvers[type]) % recipe.Amount;
            neededAmounts[type] = 0;
            foreach (var i in recipe.Ingredients)
            {
                if (i.Item1 == "ORE")
                    return i.Item2;
                    
                neededAmounts[i.Item1] = i.Item2 * numOfReps;
            }
            
            return 0;
        }

        public class Recipe
        {
            public string Type { get; set; }
            public int Amount { get; set; }
            public List<(string, int)> Ingredients { get; set; } = new List<(string, int)>();

            public Recipe(string type, int amount)
            {
                Type = type;
                Amount = amount;
            }

            public override string ToString()
            {
                return $"Type: {Type}, Amount: {Amount}, NumIngredients: {Ingredients.Count}";
            }
        }

        public class Connection
        {
            public string From { get; set; }
            public string To { get; set; }
            public int FromAmount { get; set; }
            public int ToAmount { get; set; }

            public Connection(string from, string to, int fromAmount, int toAmount)
            {
                From = from;
                To = to;
                FromAmount = fromAmount;
                ToAmount = toAmount;
            }

            public override string ToString()
            {
                return $"From: {From}, FromAmount: {FromAmount}, To: {To}, ToAmount: {ToAmount}";
            }
        }
    }
}
