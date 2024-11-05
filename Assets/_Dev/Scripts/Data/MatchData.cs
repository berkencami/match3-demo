using System.Collections.Generic;
using _Dev.Scripts.Block;

namespace _Dev.Scripts.Data
{
    public readonly struct MatchData
    {
        public readonly int Size;
        public readonly BlockType BlockType;
        public readonly List<GridCell.GridCell> GridCells;
        
        public MatchData(int matchSize, List<GridCell.GridCell> cells, BlockType matchType)
        {
            Size = matchSize;
            BlockType = matchType;
            GridCells = cells;
        }
    }
}