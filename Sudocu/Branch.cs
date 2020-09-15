using System;
using System.Collections.Generic;
using System.Text;

namespace Sudocu
{
    public class Branch
    {
        private List<Action<Grid>> Actions = new List<Action<Grid>>();
        private List<Action<Grid>> ReverseActions = new List<Action<Grid>>();

        public Branch()
        {

        }

        public void Apply(Grid grid)
        {
            foreach (var action in Actions) action.Invoke(grid);
        }

        public void Reverse(Grid grid)
        {
            foreach (var action in ReverseActions) action.Invoke(grid);
        }

        private Branch(List<Action<Grid>> actions, List<Action<Grid>> reverseActions)
        {
            Actions.AddRange(actions);
            ReverseActions.AddRange(reverseActions);
        }

        public void Add(Square square, int value)
        {
            var oldValue = square.Value;
            var x = square.X;
            var y = square.Y;

            Actions.Add((grid) =>
            {
                grid.SetSquare(x, y, value);
            });
            ReverseActions.Add((grid) =>
            {
                grid.SetSquare(x, y, oldValue);
            });
        }

        public Branch Clone()
        {
            return new Branch(Actions, ReverseActions);
        }
    }
}
