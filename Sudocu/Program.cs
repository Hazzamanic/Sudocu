using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sudocu
{
    class Program
    {
        static int depth = 1;

        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Enter your sudoku");
                var s = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(s)) break;

                var stopWatch = new Stopwatch();
                int i = 0;
                stopWatch.Restart();
                Console.WriteLine("===========");
                Console.WriteLine($"Solving grid {i}: {s}");

                var res = SimpleSolver.Solve(s);

                stopWatch.Stop();
                if (res.Solved)
                {
                    Console.WriteLine("Solved!");
                    RenderGrid(res.Puzzle);
                }

                //var result = Solver.Solve(s);
                //if (result.IsSolved)
                //{
                //    RenderGrid(result.Grid);
                //}
                //else
                //{
                //    Console.WriteLine("Sudoku impossible!");
                //}
                Console.WriteLine("Time taken: " + stopWatch.ElapsedMilliseconds / 1000 + " seconds");
            }

            
        }

        public static void RenderGrid(string puzzle)
        {
            string render = "";
            int count = 0;
            var lines = 0;
            foreach (var square in puzzle)
            {

                render += square; 
                count++;
                if (count == 3 || count == 6 || count == 9)
                {
                    render += "|";
                }
                if (count > 8)
                {
                    Console.WriteLine(render);
                    render = "";
                    count = 0;
                    lines++;
                    if (lines % 3 == 0)
                        Console.WriteLine("--- --- ---");
                }
            }

        }

        public static void RenderGrid(Grid grid, bool possibles = false)
        {
            string render = "";
            int count = 0;
            var lines = 0;
            foreach (var square in grid.Squares)
            {
                var possibleCount = grid.GetValidValuesForSquare(square);

                render += possibles ? square.Value == 0 ? possibleCount.Count().ToString() : "#" : square.Value.ToString(); //possibleCount.Count();
                count++;
                if (count == 3 || count == 6 || count == 9)
                {
                    render += "|";
                }
                if (count > 8)
                {
                    Console.WriteLine(render);
                    render = "";
                    count = 0;
                    lines++;
                    if (lines % 3 == 0)
                        Console.WriteLine("--- --- ---");
                }
            }
        }

        //public static BranchStatus SolveGrid(Grid grid, int i, bool setTo1)
        //{
        //    foreach (var square in grid.Squares)
        //    {
        //        // if the square has a value carry on
        //        if (square.Value != 0) continue;
        //        var validValues = grid.GetValidValuesForSquare(square);
        //        var count = validValues.Count();
        //        // no possible values so it is an impossible grid
        //        if (count == 0)
        //        {
        //            return BranchStatus.Impossible;
        //        }
        //        // perfect! set the value and retry the grid
        //        if (count == 1)
        //        {
        //            square.Value = validValues.First();
        //            continue;
        //        }

        //        if (setTo1) continue;

        //        if (grid.Depth < i)
        //        {
        //            foreach (var val in validValues)
        //            {
        //                var grid2 = grid.Clone();
        //                grid2.Depth++;
        //                var s2 = grid2.GetSquare(square.X, square.Y);
        //                s2.Value = val;
        //                var after1Status = SolveGrid(grid2, i, true);
        //                if (after1Status == BranchStatus.Impossible)
        //                {
        //                    square.Impossibles.Add(val);
        //                };
        //                var tryAgainStatus = SolveGrid(grid2, i, false);
        //                if (after1Status == BranchStatus.Impossible)
        //                {
        //                    square.Impossibles.Add(val);
        //                    continue;
        //                };


        //                if (grid2.IsSolved)
        //                {
        //                    grid = grid2;
        //                    return BranchStatus.Success;
        //                }
        //            }
        //        }
        //    }

        //    Console.WriteLine("===========");
        //    RenderGrid(grid);


        //    if (grid.IsSolved)
        //    {
        //        return BranchStatus.Success;
        //    }

        //    if (setTo1) return BranchStatus.Unknown;

        //    return BranchStatus.Unknown;
        //}

        //public static BranchStatus TrySolve(Grid grid, Guid guid, bool depthSearch)
        //{
        //    var branchesForSquares = new List<Tuple<Square, int, IEnumerable<int>>>();
        //    foreach (var square in grid.Squares)
        //    {
        //        // if the square has a value carry on
        //        if (square.Value != 0) continue;
        //        var validValues = grid.GetValidValuesForSquare(square);
        //        var count = validValues.Count();
        //        // no possible values so it is an impossible grid
        //        if (count == 0)
        //        {
        //            return BranchStatus.Impossible;
        //        }
        //        // perfect! set the value and retry the grid
        //        if (count == 1)
        //        {
        //            square.Value = validValues.First();
        //            square.Guid = guid;
        //            TrySolve(grid, guid, depthSearch);
        //            continue;
        //        }

        //        if (depthSearch) continue;

        //        var branches = new List<Branch>();
        //        // we need to branch to try multiple values
        //        branchesForSquares.Add(new Tuple<Square, int, IEnumerable<int>>(square, count, validValues));
        //        //Console.WriteLine($"valid values = {count}");
        //        foreach (var val in validValues)
        //        {
        //            var branch = new Branch();
        //            branch.Add(square, val);
        //        }
        //    }

        //    if (grid.IsSolved) return BranchStatus.Success;

        //    if (!depthSearch)
        //    {
        //        bool restart = false;
        //        var orderedBranches = branchesForSquares.OrderBy(e => e.Item2).ToArray();
        //        var branchPosition = 0;
        //        foreach (var branch in orderedBranches)
        //        {
        //            if (branch.Item1.Value != 0) continue;
        //            var gridBranches = new List<Branch>();
        //            gridBranches.Add(new Branch());

        //            for (int i = 0; i < depth; i++)
        //            {
        //                var index = branchPosition + (i + 1);
        //                if (index > orderedBranches.Length - 1) index = 0 + (i + 1);
        //                var nextBranch = orderedBranches[index];
        //                foreach (var val in nextBranch.Item3)
        //                {
        //                    var gridBranch = new Branch();
        //                    gridBranch.Add(nextBranch.Item1, val);
        //                    gridBranches.Add(gridBranch);
        //                }

        //            }

        //            var possibles = new HashSet<int>();
        //            foreach (var gb in gridBranches)
        //            {
        //                gb.Apply(grid);
        //                foreach (var val in branch.Item3)
        //                {
        //                    var newGuid = Guid.NewGuid();
        //                    branch.Item1.Guid = newGuid;
        //                    branch.Item1.Value = val;
        //                    //var c = grid.Clone();
        //                    //var s2 = c.GetSquare(branch.Item1.X, branch.Item1.Y);
        //                    //s2.Value = val;
        //                    var result = TrySolve(grid, Guid.NewGuid(), true);

        //                    if (result == BranchStatus.Success)
        //                    {
        //                        return BranchStatus.Success;
        //                    }
        //                    else if (result == BranchStatus.Unknown)
        //                    {
        //                        possibles.Add(val);
        //                    }
        //                    else if (result == BranchStatus.Impossible)
        //                    {
        //                        //branch.Item1.AddNotPossible(val);
        //                    }

        //                    foreach (var s in grid.Squares.Where(e => e.Guid == newGuid))
        //                    {
        //                        s.Value = 0;
        //                    }


        //                }
        //                gb.Reverse(grid);
        //                if (possibles.Count() == 1)
        //                {
        //                    var val = possibles.First();
        //                    //Console.WriteLine($"Setting square at x: {branch.Item1.X} and y: {branch.Item1.Y} to {val}");
        //                    branch.Item1.Value = val;
        //                    branch.Item1.Guid = Guid.NewGuid();
        //                    //grid = grid.Clone();
        //                    restart = true;
        //                    break;
        //                }
        //                else
        //                {

        //                }

        //            }
        //            branchPosition++;
        //        }

        //        if (restart) TrySolve(grid, Guid.NewGuid(), false);


        //        if (!grid.IsSolved)
        //        {
        //            depth++;
        //            TrySolve(grid, Guid.NewGuid(), false);
        //        }

        //        return BranchStatus.Success;
        //    }



        //    return BranchStatus.Unknown;
        //}

    }
}