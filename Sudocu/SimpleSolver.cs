using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudocu
{
    public static class SimpleSolver
    {
        public static SudokuResult Solve(string puzzle)
        {
            puzzle = puzzle.Replace('.', '0');
            var puzzleArray = puzzle.ToCharArray().Select(e => e - '0').ToArray();
            var grid = new List<SudokuSquare>();
            int row = 0;
            int col = 0;
            // loop through every square
            for (int i = 0; i < puzzle.Length; i++)
            {
                var square = new SudokuSquare(col, row, puzzleArray[i]);
                grid.Add(square);
                col++;
                if (col > 8)
                {
                    row++;
                    col = 0;
                }
            }

            var gridArray = grid.ToArray();

            if (TrySolve(gridArray))
            {
                return new SudokuResult
                {
                    Solved = true,
                    Puzzle = string.Join("", gridArray.Select(e => e.Value))
                };
            };

            return new SudokuResult();
        }

        private static bool TrySolve(SudokuSquare[] puzzle)
        {
            int indexOfTestSquare = -1;
            var values = Array.Empty<int>();
            var valuesLength = 10;

            for (int i = 0; i < puzzle.Length; i++)
            {
                var square = puzzle[i];
                if (square.Value != 0) continue;

                //var invalids = new HashSet<int>();

                var allValues = new int[9]; 
                Array.Copy(Constants.AllowedValues, allValues, 9);

                foreach(var s in puzzle)
                {
                    if(s.Y == square.Y || s.X == square.X || s.SubGrid == square.SubGrid)
                    {
                        if (s.Value == 0) continue;
                        allValues[s.Value - 1] = 0;
                        //invalids.Add(s.Value);
                    }
                }

                var validValues = allValues.Where(e => e != 0).ToArray(); //Constants.AllowedValues.Where(e => !invalids.Contains(e)).ToArray();

                if (indexOfTestSquare < 0 || validValues.Length < valuesLength)
                {
                    indexOfTestSquare = i;
                    values = validValues;
                    valuesLength = values.Length;
                }
            }

            if (indexOfTestSquare < 0) return true;

            foreach(var val in values)
            {
                var square = puzzle[indexOfTestSquare];
                square.Value = val;
                if(TrySolve(puzzle))
                {
                    return true;
                }
                square.Value = 0;
            }

            return false;
        }

        private class SudokuSquare
        {
            public SudokuSquare(int x, int y, int value)
            {
                X = x;
                Y = y;
                var across = (int)Math.Floor((double)X / 3);
                var down = (int)Math.Floor((double)Y / 3) * 3;

                SubGrid = across + down;
                Value = value;
            }

            public int X { get; }
            public int Y { get; }
            public int SubGrid { get; }
            public int Value { get; set; }
        }
    }

    public class SudokuResult
    {
        public bool Solved { get; set; }
        public string Puzzle { get; set; }
    }
}
