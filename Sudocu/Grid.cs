using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudocu
{
    public class Grid
    {
        public List<Square> Squares { get; set; } = new List<Square>();

        public void SetSquare(int x, int y, int value)
        {
            Squares.Single(e => e.X == x && e.Y == y).Value = value;
        }

        public IEnumerable<int> GetValidValuesForSquare(Square square)
        {
            //if (square.Value != 0) return new List<int> { square.Value };

            var squares = Squares.Where(e => e.Y == square.Y || e.X == square.X || e.Subgrid == square.Subgrid)
                .Select(e => e.Value)
                .Distinct().Where(e => e != 0 && !square.Impossibles.Contains(e));

            //return squares.ToList();

            return Constants.AllowedValues.Where(e => !squares.Contains(e)).ToList();
        }

        public void AddSquare(Square square)
        {
            Squares.Add(square);
        }

        public Square GetSquare(int x, int y)
        {
            return Squares.First(e => e.X == x && e.Y == y);
        }

        public bool IsSolved => Squares.All(e => e.Value != 0);
        public int Depth { get; set; } = 0;

        public Grid Clone()
        {
            var grid = new Grid();
            foreach(var square in Squares)
            {
                var s = new Square(square.X, square.Y, square.Value, grid);
                foreach (var i in square.Impossibles) s.AddNotPossible(i);
                grid.AddSquare(s);
            }
            grid.Depth = this.Depth;

            return grid;
        }

        public override string ToString()
        {
            return string.Join("", Squares.Select(e => e.Value == 0 ? "#" : e.Value.ToString()));
        }
    }
}
