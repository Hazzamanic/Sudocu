using System;
using System.Collections.Generic;
using System.Text;

namespace Sudocu
{
    public class Square
    {

        public Square(int x, int y, int value, Grid grid)
        {
            X = x;
            Y = y;
            Value = value;

            var across = (int)Math.Floor((double)X / 3);
            var down = (int)Math.Floor((double)Y / 3) * 3;

            Subgrid = across + down;
        }

        public void AddNotPossible(int val) => Impossibles.Add(val);

        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }
        public int Subgrid { get; set; }
        public Guid Guid { get; internal set; }
        public List<int> Impossibles { get; set; } = new List<int>();
    }
}
