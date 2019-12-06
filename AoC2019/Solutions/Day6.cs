using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day6
    {
        public static string A(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var centerOfMass = GetCenterOfMass(rows);
            return centerOfMass.CountTotalHeight().ToString();
        }

        public static string B(string input)
        {
            var rows = input.Replace("\r", "").Split('\n').ToList();
            var from = GetCenterOfMass(rows).FindPlanet("YOU").Parent;
            var counter = 0;

            while(!from.OrbitingPlanets.Any(x => x.Name == "SAN"))
            {
                bool foundPath = false;
                foreach (var planet in from.OrbitingPlanets)
                {
                    if (planet.FindPlanet("SAN") != null)
                    {
                        from = planet;
                        counter++;
                        foundPath = true;
                        break;
                    }
                }
                if (!foundPath)
                {
                    from = from.Parent;
                    counter++;
                }
            }
            return counter.ToString();
        }

        private static Planet GetCenterOfMass(List<string> orbits)
        {
            var planets = new List<Planet>();
            var existingPlanets = new HashSet<string>();
            foreach (var orbit in orbits)
            {
                var parse = orbit.Split(')');
                var inner = parse[0];
                var outer = parse[1];
                Planet innerPlanet = null;
                Planet outerPlanet = null;

                if (!existingPlanets.Contains(inner))
                {
                    innerPlanet = new Planet { Name = inner };
                    existingPlanets.Add(inner);
                    planets.Add(innerPlanet);
                }
                else
                {
                    foreach (var planet in planets)
                    {
                        innerPlanet = planet.FindPlanet(inner);
                        if (innerPlanet != null)
                            break;
                    }
                }

                if (!existingPlanets.Contains(outer))
                {
                    outerPlanet = new Planet { Name = outer };
                    existingPlanets.Add(outer);
                }
                else
                {
                    outerPlanet = planets.FirstOrDefault(x => x.Name == outer);
                    if (outerPlanet != null)
                    {
                        planets.Remove(outerPlanet);
                    }
                    else
                    {
                        foreach (var planet in planets)
                        {
                            outerPlanet = planet.FindAndRemovePlanet(outer);
                            if (outerPlanet != null)
                                break;
                        }
                    }
                }

                outerPlanet.UpdateHeight(innerPlanet.Height);
                outerPlanet.Parent = innerPlanet;
                innerPlanet.OrbitingPlanets.Add(outerPlanet);
            }

            return planets.First();
        }

        public class Planet
        {
            public string Name { get; set; }
            public List<Planet> OrbitingPlanets { get; set; } = new List<Planet>();
            public Planet Parent { get; set; }
            public int Height { get; set; }

            internal Planet FindPlanet(string name)
            {
                if (Name == name)
                    return this;
                Planet result = null;
                foreach(var planet in OrbitingPlanets)
                {
                    result = planet.FindPlanet(name);
                    if (result != null)
                        break;
                }
                return result;
            }

            internal Planet FindAndRemovePlanet(string name)
            {
                var result = OrbitingPlanets.FirstOrDefault(x => x.Name == name);
                if (result != null)
                {
                    OrbitingPlanets.Remove(result);
                    return result;
                }
                    
                foreach (var planet in OrbitingPlanets)
                {
                    result = planet.FindAndRemovePlanet(name);
                    if (result != null)
                        break;
                }
                return result;
            }

            internal void UpdateHeight(int height)
            {
                Height = height + 1;
                foreach(var planet in OrbitingPlanets)
                {
                    planet.UpdateHeight(Height);
                }
            }

            internal int CountTotalHeight()
            {
                if (!OrbitingPlanets.Any())
                    return Height;

                var counter = 0;
                foreach(var planet in OrbitingPlanets)
                {
                    counter += planet.CountTotalHeight();
                }
                return counter + Height;
            }
        }
    }
}
