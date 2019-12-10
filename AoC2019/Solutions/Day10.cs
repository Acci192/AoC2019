using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AoC2019.Solutions
{
    public class Day10
    {
        public static string A(string input)
        {
            var rows = input.Split('\n').Select(x => x.Trim()).ToList();
            var astroids = GetAstroids(rows);

            foreach (var astroidA in astroids)
            {
                var angles = new HashSet<Vector>();
                foreach (var astroidB in astroids)
                {
                    if (astroidA == astroidB)
                        continue;

                    UpdateAstroidInfo(astroidA, astroidB);
                    angles.Add(astroidB.AngleFromLaser);
                }

                astroidA.NumOfSeenAstroid = angles.Count;
            }

            return astroids.Max(x => x.NumOfSeenAstroid).ToString();
        }

        

        public static string B(string input)
        {
            var rows = input.Split('\n').Select(x => x.Trim()).ToList();

            var astroids = GetAstroids(rows);

            foreach (var astroidA in astroids)
            {
                var unitVectors = new HashSet<Vector>();
                foreach (var astroidB in astroids)
                {
                    if (astroidA == astroidB)
                        continue;

                    UpdateAstroidInfo(astroidA, astroidB);
                    unitVectors.Add(astroidB.AngleFromLaser);
                }

                astroidA.NumOfSeenAstroid = unitVectors.Count;
            }

            var laser = astroids.OrderByDescending(x => x.NumOfSeenAstroid).First();
            astroids.Remove(laser);

            var anglesFromLaser = new HashSet<Vector>();
            foreach (var astroid in astroids)
            {
                UpdateAstroidInfo(laser, astroid);
                anglesFromLaser.Add(astroid.AngleFromLaser);
            }

            var angles = anglesFromLaser.Where(vector => vector.X >= 0 && vector.Y <= 0).OrderBy(v => v.X).ToList();
            angles.AddRange(anglesFromLaser.Where(vector => vector.X >= 0 && vector.Y > 0).OrderByDescending(v => v.X));
            angles.AddRange(anglesFromLaser.Where(vector => vector.X < 0 && vector.Y >= 0).OrderByDescending(v => v.X));
            angles.AddRange(anglesFromLaser.Where(vector => vector.X < 0 && vector.Y < 0).OrderBy(v => v.X));

            astroids = astroids.OrderBy(x => x.DistanceFromLaser).ToList();
            var count = 0;
            while(count < 200)
            {
                foreach(var angle in angles)
                {
                    var closestAstroid = astroids.FirstOrDefault(x => x.AngleFromLaser == angle);
                    if(closestAstroid != null)
                    {
                        astroids.Remove(closestAstroid);
                        count++;
                        if(count == 200)
                        {
                            return (closestAstroid.Position.X * 100 + closestAstroid.Position.Y).ToString();
                        }
                    }
                }
            }
            
            return string.Empty;
        }

        private static void UpdateAstroidInfo(Astroid laser, Astroid astroid)
        {
            var vectorAB = new Vector(astroid.Position.X - laser.Position.X, astroid.Position.Y - laser.Position.Y);
            astroid.DistanceFromLaser = Math.Sqrt((Math.Pow(vectorAB.X, 2) + Math.Pow(vectorAB.Y, 2)));

            vectorAB.Normalize();
            astroid.AngleFromLaser = vectorAB;
        }

        private static List<Astroid> GetAstroids(List<string> rows)
        {
            var astroids = new List<Astroid>();
            for (var h = 0; h < rows.Count; h++)
            {
                for (var w = 0; w < rows[0].Length; w++)
                {
                    if (rows[h][w] == '#')
                        astroids.Add(new Astroid { Position = new Vector(w, h), NumOfSeenAstroid = 0 });
                }
            }

            return astroids;
        }

        private class Astroid
        {
            public Vector Position { get; set; }
            public int NumOfSeenAstroid { get; set; }
            public double DistanceFromLaser { get; set; }
            public Vector AngleFromLaser { get; set; }
        }
    }
}
