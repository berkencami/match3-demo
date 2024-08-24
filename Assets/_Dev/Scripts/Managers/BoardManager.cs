using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Zenject;

public class BoardManager : Manager
{
    [Inject] private LevelManager levelManager;
    [Inject] private MatchManager matchManager;
    [Inject] private BlockFactory blockFactory;
    [Inject] private PlayerManager playerManager;

    public GridCell[] fillingCells;
    [SerializeField] private GameObject board;
    
    public GridCell[,] gridCells;

    protected override void Awake()
    {
        base.Awake();
        matchManager.OnMatch += OnMatch;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        matchManager.OnMatch -= OnMatch;
    }

    protected override void PreLevelInstantiateProcess()
    {
        base.PreLevelInstantiateProcess();
        DestroyGrids();
        LeanPool.DespawnAll();
        DOTween.KillAll();
        CreateCells();
    }

    protected override void LevelInstantiateProcess()
    {
        base.LevelInstantiateProcess();
        board.gameObject.SetActive(true);
        InitializeGridCells();
        FindFillingCells();
    }

    protected override void LevelEndProcess(bool status)
    {
        base.LevelEndProcess(status);
        DestroyGrids();
    }

    private void CreateCells()
    {
        gridCells = new GridCell[levelManager.currentLevel.row, levelManager.currentLevel.column];
    }

    private void InitializeGridCells()
    {
        foreach (GridCell gridCell in gridCells)
        {
            if (gridCell != null)
            {
                gridCell.SetNeighbours();
            }
        }
    }

    private void FindFillingCells()
    {
        List<GridCell> cellList = new List<GridCell>();
        int count = levelManager.currentLevel.column;
        for (int i = 0; i < count; i++)
        {
            cellList.Add(gridCells[i, 0] == null ? gridCells[i, 1] : gridCells[i, 0]);
        }

        fillingCells = cellList.ToArray();
    }
    
    private async void OnMatch()
    {
        await FallBlocks();
        await FillBoard();
    }

    private async Task FillBoard()
    {
        await Task.Yield();
        List<GridCell> emptyGrids = FindFilledGrids();
        List<BlockBase> spawnedBlocks = new List<BlockBase>();
        foreach (GridCell cell in emptyGrids)
        {
            MatchBlock newBlock = blockFactory.RandomMatchBlock(cell.yCoord,
                fillingCells.First(x => x.xCoord == cell.xCoord).transform.position);
            newBlock.SetGrid(cell);
            spawnedBlocks.Add(newBlock);
        }

        List<Task> tasks = spawnedBlocks.Select(block => block.Fall()).ToList();

        await Task.WhenAll(tasks);
        playerManager.OnCheckGame?.Invoke();
    }

    private async Task FallBlocks()
    {
        List<GridCell> gridCells = GetEmptyGrids();
        List<GridCell> blastedCellsTopNeighbours = GetTopNeighbours(gridCells);
        blastedCellsTopNeighbours.Reverse();
        List<Task> tasks = new List<Task>();
        foreach (GridCell gridCell in blastedCellsTopNeighbours)
        {
            if (gridCell.block == null) continue;
     
            BlockBase block = gridCell.block;
            block.SetGrid(GetEmptyGrid(gridCell));
            tasks.Add(block.Fall());
        }

        await Task.WhenAll(tasks);
    }

    private List<GridCell> GetTopNeighbours(List<GridCell> gridCells)
    {
        List<GridCell> blastedCellsTopNeighbours = new List<GridCell>();
        foreach (var gridCell in gridCells)
        {
            for (int i = 0; i < gridCell.yCoord; i++)
            {
                if (!blastedCellsTopNeighbours.Contains(this.gridCells[gridCell.xCoord, i]) && !gridCells.Contains(this.gridCells[gridCell.xCoord, i]) && this.gridCells[gridCell.xCoord, i].block.Fallable)
                {
                    blastedCellsTopNeighbours.Add(this.gridCells[gridCell.xCoord, i]);
                }
            }
        }

        return blastedCellsTopNeighbours;
    }

    private GridCell GetEmptyGrid(GridCell gridCell)
    {
        GridCell newGrid = null;
        int row = levelManager.currentLevel.row;
        int yCoord = gridCell.yCoord+1;
        for (int i = yCoord; i < row; i++)
        {
            if (gridCells[gridCell.xCoord, i].block != null)
            {
                if (!gridCells[gridCell.xCoord, i].block.Fallable)
                {
                    if (newGrid == null)
                    {
                        newGrid = gridCell;
                    }
                  
                    break;
                }
            }
            if (gridCells[gridCell.xCoord, i].block != null || gridCell.block == null) continue;
            newGrid = gridCells[gridCell.xCoord, i];
        }

        if (newGrid == null) return newGrid;
        BlockBase blockBase = gridCell.block;
        gridCell.block = null;
        newGrid.block = blockBase;
        return newGrid;
        
    }

    private List<GridCell> GetEmptyGrids()
    {
        List<GridCell> emptyGrids = new List<GridCell>();
        foreach (GridCell gridCell in gridCells)
        {
            if (gridCell.block != null) continue;
            if (!emptyGrids.Contains(gridCell))
            {
                emptyGrids.Add(gridCell);
            }
        }

        return emptyGrids;
    }

    private List<GridCell> FindFilledGrids()
    {
        List<GridCell> emptyGrids = new List<GridCell>();

        for (int i = 0; i < levelManager.currentLevel.row; i++)
        {
            for (int j = 0; j < levelManager.currentLevel.column; j++)
            {
                if (gridCells[i, j].block != null)
                {
                    break;
                }
                if (!emptyGrids.Contains(gridCells[i, j]))
                {
                    emptyGrids.Add(gridCells[i, j]);
                }
            }
        }

        return emptyGrids;
    }

    private void DestroyGrids()
    {
        if(gridCells==null) return;
        
        foreach (GridCell gridCell in gridCells)
        {
            if (gridCell != null)
            {
                Destroy(gridCell.gameObject);
            }
           
        }
    }
}