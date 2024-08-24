using System.Collections.Generic;

public readonly struct MatchData
{
    public readonly int size;
    public readonly BlockType blockType;
    public readonly List<GridCell> gridCells;
        
    public MatchData(int matchSize, List<GridCell> cells, BlockType matchType)
    {
        size = matchSize;
        blockType = matchType;
        gridCells = cells;
    }
}