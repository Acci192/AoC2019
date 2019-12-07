using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AoC2019.Solutions
{
    public class Day7
    {
        public static string A(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();

            var max = 0;

            for (var aPhase = 0; aPhase < 5; aPhase++)
            {
                for (var bPhase = 0; bPhase < 5; bPhase++)
                {
                    for (var cPhase = 0; cPhase < 5; cPhase++)
                    {
                        for (var dPhase = 0; dPhase < 5; dPhase++)
                        {
                            for (var ePhase = 0; ePhase < 5; ePhase++)
                            {
                                var uniquePhaseSetting = new HashSet<int>
                                {
                                    aPhase,
                                    bPhase,
                                    cPhase,
                                    dPhase,
                                    ePhase
                                };
                                if (uniquePhaseSetting.Count != 5)
                                    continue;

                                var outputA = new BlockingCollection<int> { bPhase };

                                var outputB = new BlockingCollection<int> { cPhase };

                                var outputC = new BlockingCollection<int> { dPhase };

                                var outputD = new BlockingCollection<int> { ePhase };

                                var outputE = new BlockingCollection<int> { aPhase, 0 };

                                var a = new IntCodeComputer(program, outputE, outputA);
                                var b = new IntCodeComputer(program, outputA, outputB);
                                var c = new IntCodeComputer(program, outputB, outputC);
                                var d = new IntCodeComputer(program, outputC, outputD);
                                var e = new IntCodeComputer(program, outputD, outputE);

                                var threadA = new Thread(new ThreadStart(a.Run));
                                var threadB = new Thread(new ThreadStart(b.Run));
                                var threadC = new Thread(new ThreadStart(c.Run));
                                var threadD = new Thread(new ThreadStart(d.Run));
                                var threadE = new Thread(new ThreadStart(e.Run));

                                threadA.Start();
                                threadB.Start();
                                threadC.Start();
                                threadD.Start();
                                threadE.Start();

                                threadE.Join();
                                var value = outputE.Take();
                                if (value > max)
                                {
                                    max = value;
                                }
                            }
                        }
                    }
                }
            }

            return max.ToString();
        }

        public static string B(string input)
        {
            var program = input.Split(',').Select(int.Parse).ToList();

            var max = 0;

            for (var aPhase = 5; aPhase < 10; aPhase++)
            {
                for (var bPhase = 5; bPhase < 10; bPhase++)
                {
                    for (var cPhase = 5; cPhase < 10; cPhase++)
                    {
                        for (var dPhase = 5; dPhase < 10; dPhase++)
                        {
                            for (var ePhase = 5; ePhase < 10; ePhase++)
                            {
                                var uniquePhaseSetting = new HashSet<int>
                                {
                                    aPhase,
                                    bPhase,
                                    cPhase,
                                    dPhase,
                                    ePhase
                                };
                                if (uniquePhaseSetting.Count != 5)
                                    continue;

                                var outputA = new BlockingCollection<int> { bPhase };
                                var outputB = new BlockingCollection<int> { cPhase };
                                var outputC = new BlockingCollection<int> { dPhase };
                                var outputD = new BlockingCollection<int> { ePhase };
                                var outputE = new BlockingCollection<int> { aPhase, 0};

                                var a = new IntCodeComputer(program, outputE, outputA);
                                var b = new IntCodeComputer(program, outputA, outputB);
                                var c = new IntCodeComputer(program, outputB, outputC);
                                var d = new IntCodeComputer(program, outputC, outputD);
                                var e = new IntCodeComputer(program, outputD, outputE);

                                var threadA = new Thread(new ThreadStart(a.Run));
                                var threadB = new Thread(new ThreadStart(b.Run));
                                var threadC = new Thread(new ThreadStart(c.Run));
                                var threadD = new Thread(new ThreadStart(d.Run));
                                var threadE = new Thread(new ThreadStart(e.Run));

                                threadA.Start();
                                threadB.Start();
                                threadC.Start();
                                threadD.Start();
                                threadE.Start();

                                threadE.Join();
                                var value = outputE.Take();
                                if (value > max)
                                {
                                    max = value;
                                }
                            }
                        }
                    }
                }
            }
            
            return max.ToString();
        }
    }
}
