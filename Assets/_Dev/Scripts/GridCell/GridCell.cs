using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class GridCell : MonoBehaviour
{
    [Inject] private LevelManager levelManager;
    [Inject] private BoardManager boardManager;
    [Inject] public MatchManager matchManager;
    [Inject] public GoalManager goalManager;

    public int xCoord;
    public int yCoord;
    public List<GridCell> neighbours = new List<GridCell>();
    
    public BlockBase block;

    [Inject]
    public void Construct(LevelManager _levelManager, MatchManager _matchManager, GoalManager _goalManager,BoardManager _boardManager)
    {
        levelManager = _levelManager;
        matchManager = _matchManager;
        goalManager = _goalManager;
        boardManager = _boardManager;
    }

    public void SetCoords(int x, int y)
    {
        xCoord = x;
        yCoord = y;
    }

    public void SetNeighbours()
    {
        neighbours = GetNeighbours(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
    }

    private List<GridCell> GetNeighbours(params Direction[] directions)
    {
        List<GridCell> neighbours = new List<GridCell>();

        foreach (Direction direction in directions)
        {
            GridCell neighbour = GetNeighbourWithDirection(direction);
            if (neighbour != null)
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    private GridCell GetNeighbourWithDirection(Direction direction)
    {
        int x = xCoord;
        int y = yCoord;
        switch (direction)
        {
            case Direction.Right:
                x += 1;
                break;
            case Direction.Left:
                x -= 1;
                break;
            case Direction.Up:
                y -= 1;
                break;
            case Direction.Down:
                y += 1;
                break;
        }

        if (x >= levelManager.currentLevel.row || x < 0 || y >= levelManager.currentLevel.column || y < 0)
        {
            return null;
        }

        return boardManager.gridCells[x, y];
    }

    private void CheckNeighbourObstacle()
    {
        foreach (GridCell neighbourGrid in neighbours)
        {
            if (neighbourGrid.block == null) continue;

            if (neighbourGrid.block.blockType == BlockType.Obstacle)
            {
                neighbourGrid.BlastObstacle();
            }
        }
    }

    public async void BlastObstacle()
    {
        BlockBase currentBlock = block;
        block = null;
        await currentBlock.Blast();
    }

    public async Task BlastWithRocket()
    {
        if(block==null) return;
        BlockBase currentBlock = block;
        block = null;
        await currentBlock.Blast();
    }

    public async Task BlastCell()
    {
        if(block==null) return;
      
        if (!IsAnyBooster(block))
        {
            CheckNeighbourObstacle();
        }
        
        BlockBase currentBlock = block;
        block = null;
        await currentBlock.Blast();
    }

    public async Task MergeCell(Transform mergePoint)
    {
        if(block==null) return;
        CheckNeighbourObstacle();
        BlockBase currentBlock = block;
        block = null;
        await currentBlock.MergeBlock(mergePoint);
    }
    
    private bool IsAnyBooster(BlockBase blockBase)
    {
        return blockBase.blockType == BlockType.Bomb || blockBase.blockType == BlockType.VerticalRocket ||
               blockBase.blockType == BlockType.HorizontalRocket;
    }
    
}