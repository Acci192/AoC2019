using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC2019.Solutions
{
    public class Day12
    {
        public static Regex InputParser = new Regex(@"^<x=(-?\d*), y=(-?\d*), z=(-?\d*)>$");
        public static string A(string input)
        {
            var moons = input.Replace("\r", "").Split('\n').Select(x => InputParser.Match(x)).Select(x => new Moon(int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value), int.Parse(x.Groups[3].Value))).ToList();

            for (var i = 0; i < 1000; i++)
            {
                moons.ForEach(x => moons.ForEach(y => x.UpdateVelocity(y)));
                moons.ForEach(x => x.UpdatePosition());
            }
            return moons.Select(moon => (Math.Abs(moon.PosX) + Math.Abs(moon.PosY) + Math.Abs(moon.PosZ)) * (Math.Abs(moon.VelX) + Math.Abs(moon.VelY) + Math.Abs(moon.VelZ))).Sum().ToString();
        }

        public static string B(string input)
        {
            var moons = input.Replace("\r", "").Split('\n').Select(x => InputParser.Match(x)).Select(x => new Moon(int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value), int.Parse(x.Groups[3].Value))).ToList();
            var repeatingXAxis = 0;
            var repeatingYAxis = 0;
            var repeatingZAxis = 0;

            var steps = 0;
            while(repeatingXAxis == 0 || repeatingYAxis == 0 || repeatingZAxis == 0)
            {
                moons.ForEach(x => moons.ForEach(y => x.UpdateVelocity(y)));
                moons.ForEach(x => x.UpdatePosition());

                steps++;
                repeatingXAxis = repeatingXAxis != 0 ? repeatingXAxis : moons.TrueForAll(x => x.VelX == 0) ? steps * 2 : 0;
                repeatingYAxis = repeatingYAxis != 0 ? repeatingYAxis : moons.TrueForAll(x => x.VelY == 0) ? steps * 2 : 0;
                repeatingZAxis = repeatingZAxis != 0 ? repeatingZAxis : moons.TrueForAll(x => x.VelZ == 0) ? steps * 2 : 0;
            }
            var result = LCM(LCM(repeatingXAxis, repeatingYAxis), repeatingZAxis);
            return result.ToString();
        }

        private static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return b != 0 ? GCD(b, a % b) : a;
        }

        private static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return a * b / GCD(a, b);
        }

        public class Moon
        {
            public int PosX { get; set; }
            public int VelX { get; set; }
            public int PosY { get; set; }
            public int VelY { get; set; }
            public int PosZ { get; set; }
            public int VelZ { get; set; }

            public Moon(int posX, int posY, int posZ)
            {
                PosX = posX;
                PosY = posY;
                PosZ = posZ;
            }

            public void UpdateVelocity(Moon other)
            {
                VelX = PosX == other.PosX ? VelX : PosX > other.PosX ? VelX -=1 : VelX +=1;
                VelY = PosY == other.PosY ? VelY : PosY > other.PosY ? VelY -= 1 : VelY += 1;
                VelZ = PosZ == other.PosZ ? VelZ : PosZ > other.PosZ ? VelZ -= 1 : VelZ += 1;
            }

            public void UpdatePosition()
            {
                PosX += VelX;
                PosY += VelY;
                PosZ += VelZ;
            }
        }
    }
}
