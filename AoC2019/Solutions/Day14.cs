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
        // Don't Look. This needs major cleaning. 
        public static BigInteger UsedOre = 0;
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

            var ingredients = new List<Ingredient>();

            foreach (var row in rows)
            {
                var split = row.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                var froms = split[0].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                var toInfo = split[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var toNode = toInfo[1];
                var toAmount = int.Parse(toInfo[0]);
                var recipe = new Recipe(toAmount);
                foreach (var from in froms)
                {
                    var fromInfo = from.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var fromNode = fromInfo[1];
                    var fromAmount = int.Parse(fromInfo[0]);
                    recipe.RequiredIngredients[fromNode] = fromAmount;
                }

                ingredients.Add(new Ingredient(toNode, recipe));
            }
            var baseIngredients = ingredients.Where(x => x.Recipe.RequiredIngredients.Where(y => y.Key == "ORE").Count() == 1).ToList();
            baseIngredients.ForEach(x => x.CalculateDependencies(0, ingredients));

            
            ingredients = ingredients.OrderByDescending(x => x.Dependencies).ToList();

            ingredients.First(x => x.Name == "FUEL").RequiredAmount = 1;
            ingredients.ForEach(x => x.ProduceRequiredIngredients(ingredients));
            ingredients.ForEach(x => x.ClearRequired());
            

            var target = 1000000000000;
            var min = target / UsedOre;
            var max = target + new BigInteger(100000000);
            var increment = min / 2;
            
            while (min < max)
            {
                ingredients.ForEach(x => x.ClearRequired());
                UsedOre = 0;
                var mid = (min + max) / 2;

                ingredients.First(x => x.Name == "FUEL").RequiredAmount = mid;
                ingredients.ForEach(x => x.ProduceRequiredIngredients(ingredients));
                if (UsedOre > target)
                    max = mid;
                else if (UsedOre < target)
                {
                    if (mid == min)
                        break;
                    min = mid;
                }
                else
                {
                    min = mid;
                    break;
                }
            }
            return min.ToString();
        }

        public class Ingredient
        {
            public string Name { get; set; }
            public BigInteger RequiredAmount { get; set; }
            public int Dependencies { get; set; } = 0;
            public Recipe Recipe { get; set; }

            public Ingredient(string name, Recipe recipe)
            {
                Name = name;
                Recipe = recipe;
            }
            
            public void ClearRequired()
            {
                RequiredAmount = 0;
            }

            public void ProduceIngredient(BigInteger amount, List<Ingredient> ingredients)
            {
                var numReps = (int)Math.Ceiling((double)amount / Recipe.Output);
                
                foreach (var i in Recipe.RequiredIngredients)
                {
                    if (i.Key == "ORE")
                    {
                        UsedOre += i.Value * numReps;
                        return;
                    }
                        
                    var ingredient = ingredients.First(x => x.Name == i.Key);
                    ingredient.RequiredAmount += i.Value * numReps;
                }  
            }

            public void ProduceRequiredIngredients(List<Ingredient> ingredients)
            {
                var numReps = new BigInteger(0);
                if (RequiredAmount % Recipe.Output == 0)
                    numReps = RequiredAmount / Recipe.Output;
                else
                    numReps = RequiredAmount / Recipe.Output + 1;

                foreach (var i in Recipe.RequiredIngredients)
                {
                    if (i.Key == "ORE")
                    {
                        UsedOre += new BigInteger(i.Value) * numReps;
                        return;
                    }

                    var ingredient = ingredients.First(x => x.Name == i.Key);
                    ingredient.RequiredAmount += new BigInteger(i.Value) * numReps;
                }
            }

            public void CalculateDependencies(int stage, List<Ingredient> ingredients)
            {
                Dependencies = Math.Max(stage + 1, Dependencies);
                var baseIngredients = ingredients.Where(x => x.Recipe.RequiredIngredients.Where(y => y.Key == Name).Count() == 1).ToList();
                baseIngredients.ForEach(x => x.CalculateDependencies(Dependencies, ingredients));
            }
        }

        public class Recipe
        {
            public int Output { get; set; }
            public Dictionary<string, int> RequiredIngredients { get; set; } = new Dictionary<string, int>();

            public Recipe(int output)
            {
                Output = output;
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
