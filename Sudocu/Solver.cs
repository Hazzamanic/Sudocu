using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudocu
{
    public static class Solver
    {
        public static BranchResult Solve(string puzzle)
        {
            var puzzleArray = puzzle.ToCharArray().Select(e => e - '0').ToArray();
            var grid = new Grid();
            int row = 0;
            int col = 0;
            for (int i = 0; i < puzzleArray.Length; i++)
            {
                var square = new Square(col, row, puzzleArray[i], grid);
                grid.AddSquare(square);
                col++;
                if (col > 8)
                {
                    row++;
                    col = 0;
                }
            }

            return TrySolve(grid);
        }
        
        private static BranchResult TrySolve(Grid grid)
        {

            var lowest = 10;
            Square squareToTest = null;
            IEnumerable<int> values = null;
            foreach (var square in grid.Squares)
            {
                // if the square has a value carry on
                if (square.Value != 0) continue;
                var validValues = grid.GetValidValuesForSquare(square);
                var count = validValues.Count();
                // no possible values so it is an impossible grid
                //if (count == 0)
                //{
                //    return BranchResult.Impossible;
                //}
                //// perfect! set the value and retry the grid
                //if (count == 1)
                //{
                //    square.Value = validValues.First();
                //    var result = TrySolve(grid);
                //    if (result.IsSolved) return result;
                //    if (result.IsImpossible) return result;
                //    continue;
                //}

                if (count < lowest)
                {
                    squareToTest = square;
                    lowest = count;
                    values = validValues;
                }
            }

            if (grid.IsSolved) return BranchResult.Solved(grid);

            foreach (var val in values)
            {
                var clonedGrid = grid.Clone();
                var s2 = clonedGrid.GetSquare(squareToTest.X, squareToTest.Y);
                s2.Value = val;
                var result = TrySolve(clonedGrid);

                if (result.IsImpossible)
                {
                    grid.GetSquare(squareToTest.X, squareToTest.Y).AddNotPossible(val);
                    continue;
                }

                if (result.IsSolved)
                {
                    return BranchResult.Solved(result.Grid);
                }
            }


            return BranchResult.Impossible;
        }
    }
}
