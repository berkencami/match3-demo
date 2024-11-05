using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Dev.Scripts.Block;
using _Dev.Scripts.GridCell;
using _Dev.Scripts.Managers;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Zenject;

public class BoardManager : Manager
{
    [Inject] private LevelManager _levelManager;
    [Inject] private MatchManager _matchManager;
    [Inject] private BlockFactory _blockFactory;
    [Inject] private PlayerManager _playerManager;

    [SerializeField] private GameObject _board;
    private GridCell[] _fillingCells;
    private GridCell[,] _gridCells;
    public GridCell[,] GridCells => _gridCells;

    protected override void Awake()
    {
        base.Awake();
        _matchManager.OnMatch += OnMatch;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _matchManager.OnMatch -= OnMatch;
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
        _board.gameObject.SetActive(true);
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
        _gridCells = new GridCell[_levelManager.CurrentLevel.row, _levelManager.CurrentLevel.column];
    }

    private void InitializeGridCells()
    {
        foreach (var gridCell in GridCells)
        {
            if (gridCell != null)
            {
                gridCell.SetNeighbours();
            }
        }
    }

    private void FindFillingCells()
    {
        var cellList = new List<GridCell>();
        var count = _levelManager.CurrentLevel.column;
        for (var i = 0; i < count; i++)
        {
            cellList.Add(GridCells[i, 0] == null ? GridCells[i, 1] : GridCells[i, 0]);
        }

        _fillingCells = cellList.ToArray();
    }

    private async void OnMatch()
    {
        await FallBlocks();
        await FillBoard();
    }

    private async Task FillBoard()
    {
        await Task.Yield();
        var emptyGrids = FindFilledGrids();
        var spawnedBlocks = new List<BlockBase>();
        foreach (var cell in emptyGrids)
        {
            var newBlock = _blockFactory.RandomMatchBlock(cell.Coordinates.y,
                _fillingCells.First(x => x.Coordinates.x == cell.Coordinates.x).transform.position);
            newBlock.SetGrid(cell);
            spawnedBlocks.Add(newBlock);
        }

        var tasks = spawnedBlocks.Select(block => block.Fall()).ToList();

        await Task.WhenAll(tasks);
        _playerManager.OnCheckGame?.Invoke();
    }

    private async Task FallBlocks()
    {
        var gridCells = GetEmptyGrids();
        var blastedCellsTopNeighbours = GetTopNeighbours(gridCells);
        blastedCellsTopNeighbours.Reverse();
        var tasks = new List<Task>();
        foreach (var gridCell in blastedCellsTopNeighbours)
        {
            if (gridCell.Block == null) continue;

            var block = gridCell.Block;
            block.SetGrid(GetEmptyGrid(gridCell));
            tasks.Add(block.Fall());
        }

        await Task.WhenAll(tasks);
    }

    private List<GridCell> GetTopNeighbours(List<GridCell> gridCells)
    {
        var blastedCellsTopNeighbours = new List<GridCell>();
        foreach (var gridCell in gridCells)
        {
            for (var i = 0; i < gridCell.Coordinates.y; i++)
            {
                if (!blastedCellsTopNeighbours.Contains(_gridCells[gridCell.Coordinates.x, i]) &&
                    !gridCells.Contains(_gridCells[gridCell.Coordinates.x, i]) &&
                    _gridCells[gridCell.Coordinates.x, i].Block.CanFall)
                {
                    blastedCellsTopNeighbours.Add(_gridCells[gridCell.Coordinates.x, i]);
                }
            }
        }

        return blastedCellsTopNeighbours;
    }

    private GridCell GetEmptyGrid(GridCell gridCell)
    {
        GridCell newGrid = null;
        var row = _levelManager.CurrentLevel.row;
        var yCoord = gridCell.Coordinates.y + 1;
        for (var i = yCoord; i < row; i++)
        {
            if (GridCells[gridCell.Coordinates.x, i].Block != null)
            {
                if (!GridCells[gridCell.Coordinates.x, i].Block.CanFall)
                {
                    if (newGrid == null)
                    {
                        newGrid = gridCell;
                    }

                    break;
                }
            }

            if (GridCells[gridCell.Coordinates.x, i].Block != null || gridCell.Block == null) continue;
            newGrid = GridCells[gridCell.Coordinates.x, i];
        }

        if (newGrid == null) return newGrid;
        var blockBase = gridCell.Block;
        gridCell.SetBlock(null);
        newGrid.SetBlock(blockBase);
        return newGrid;
    }

    private List<GridCell> GetEmptyGrids()
    {
        var emptyGrids = new List<GridCell>();
        foreach (var gridCell in GridCells)
        {
            if (gridCell.Block != null) continue;
            if (!emptyGrids.Contains(gridCell))
            {
                emptyGrids.Add(gridCell);
            }
        }

        return emptyGrids;
    }

    private List<GridCell> FindFilledGrids()
    {
        var emptyGrids = new List<GridCell>();

        for (var i = 0; i < _levelManager.CurrentLevel.row; i++)
        {
            for (var j = 0; j < _levelManager.CurrentLevel.column; j++)
            {
                if (GridCells[i, j].Block != null)
                {
                    break;
                }

                if (!emptyGrids.Contains(GridCells[i, j]))
                {
                    emptyGrids.Add(GridCells[i, j]);
                }
            }
        }

        return emptyGrids;
    }

    private void DestroyGrids()
    {
        if (GridCells == null) return;

        foreach (var gridCell in GridCells)
        {
            if (gridCell != null)
            {
                Destroy(gridCell.gameObject);
            }
        }
    }
}