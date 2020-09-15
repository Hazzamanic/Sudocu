using System;
using System.Collections.Generic;
using System.Text;

namespace Sudocu
{
    public class BranchResult
    {
        private readonly Grid _grid;

        private BranchResult()
        {

        }
        private BranchResult(bool impossible)
        {
            IsImpossible = true;
        }

        private BranchResult(Grid grid)
        {
            _grid = grid;
            IsSolved = true;
        }

        public bool IsSolved { get; }
        public bool IsImpossible { get; }
        public Grid Grid => IsSolved ?
            _grid :
            throw new InvalidOperationException("Cannot access grid if it failed");

        public static BranchResult Impossible => new BranchResult(true);
        public static BranchResult Unknown => new BranchResult();
        public static BranchResult Solved(Grid grid) => new BranchResult(grid);
    }
}
