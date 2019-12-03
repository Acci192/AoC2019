using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Solutions
{
    public class Day3
    {
        public static string A(string input)
        {
            var wires = input.Split('\n').Select(x => x.Split(',')).ToList();
            var occopiedSpace = new HashSet<Tuple<int, int>>();

            var shortestPath = int.MaxValue;
            for (var i = 0; i < 2; i++)
            {
                var curPosX = 0;
                var curPosY = 0;
                for (var j = 0; j < wires[i].Count(); j++)
                {
                    var move = wires[i][j];
                    var direction = move.First();
                    var amount = int.Parse(string.Join("", move.Skip(1).ToList()));
                    for (var k = 0; k < amount; k++)
                    {
                        switch (direction)
                        {
                            case 'R':
                                curPosX++;
                                break;
                            case 'L':
                                curPosX--;
                                break;
                            case 'U':
                                curPosY--;
                                break;
                            case 'D':
                                curPosY++;
                                break;
                        }
                        if(i == 0)
                        {
                            occopiedSpace.Add(new Tuple<int, int>(curPosX, curPosY));
                        }
                        else if(occopiedSpace.Contains(new Tuple<int, int>(curPosX, curPosY)))
                        {
                                var dist = Math.Abs(curPosX) + Math.Abs(curPosY);
                                if (dist < shortestPath)
                                    shortestPath = dist;
                        }
                    }
                }
            }
            return shortestPath.ToString();
        }

        public static string B(string input)
        {
            var rows = input.Split('\n').Select(x => x.Split(',')).ToList();
            var occopiedSpace = new Dictionary<Tuple<int, int>, int>();
            var grid = new char[250, 250];

            var shortestPath = int.MaxValue;
            for (var i = 0; i < 2; i++)
            {
                var curPosX = 0;
                var curPosY = 0;
                var counter = 0;
                for (var j = 0; j < rows[i].Count(); j++)
                {
                    var move = rows[i][j];
                    var direction = move.First();
                    var amount = int.Parse(string.Join("", move.Skip(1).ToList()));
                    for (var k = 0; k < amount; k++)
                    {
                        counter++;
                        switch (direction)
                        {
                            case 'R':
                                curPosX++;
                                break;
                            case 'L':
                                curPosX--;
                                break;
                            case 'U':
                                curPosY--;
                                break;
                            case 'D':
                                curPosY++;
                                break;
                        }
                        if (i == 0)
                        {
                            if(!occopiedSpace.ContainsKey(new Tuple<int, int>(curPosX, curPosY)))
                                occopiedSpace.Add(new Tuple<int, int>(curPosX, curPosY), counter);
                        }
                        else if (occopiedSpace.ContainsKey(new Tuple<int, int>(curPosX, curPosY)))
                        {
                            var dist = occopiedSpace[new Tuple<int, int>(curPosX, curPosY)] + counter;
                            if (dist < shortestPath)
                                shortestPath = dist;
                        }
                    }
                }
            }
            return shortestPath.ToString();
        }
    }
}
