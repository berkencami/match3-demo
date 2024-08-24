using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MatchManager : MonoBehaviour
{
    [Inject] private InputManager inputManager;
    [Inject] private BlockFactory blockFactory;
    [Inject] private MoveManager moveManager;
    
    public Action OnMatch;

    [Header("Match refs")] 
    
    [SerializeField] private int minMatchSize = 2;
    [SerializeField] private int matchSizeForVerticalRocket = 5;
    [SerializeField] private int matchSizeForHorizontalRocket = 7;
    [SerializeField] private int matchSizeForBomb = 7;

    private readonly List<GridCell> matchedCells = new List<GridCell>();
    private int matchSize = 0;

    private void Awake()
    {
        inputManager.OnClickOnCell += OnClickOnCell;
    }

    private void OnDestroy()
    {
        inputManager.OnClickOnCell -= OnClickOnCell;
    }

    private async void OnClickOnCell(GridCell cell)
    {
        MatchData matchData = SearchMatch(cell);

        if (IsAnyBooster(cell.block))
        {
            moveManager.OnDecreaseMove?.Invoke();
            if (cell.block.blockType == BlockType.Bomb)
            {
                await cell.BlastCell();
                OnMatch?.Invoke();
                return;
            }
            else
            {
                await cell.BlastCell();
                return;
            }
          
        }
        if (matchData.size < minMatchSize) return;

        moveManager.OnDecreaseMove?.Invoke();
        await Match(matchData,cell);

    }

    private async Task Match(MatchData matchData, GridCell gridCell)
    {
        List<Task> tasks = new List<Task>();
        if (matchSizeForVerticalRocket<=matchData.size)
        {
            FXManager.instance.PlaySoundFX(SoundType.Merge);
            foreach (GridCell matchCell in matchData.gridCells)
            { 
                tasks.Add(matchCell.MergeCell(gridCell.transform));
            }
            
            await Task.WhenAll(tasks);
            gridCell.block = GetBooster(matchData.size, gridCell);
            await Task.Yield();
            OnMatch?.Invoke();

        }
        else
        {
            FXManager.instance.PlaySoundFX(SoundType.Match);
            foreach (GridCell matchCell in matchData.gridCells)
            { 
                tasks.Add(matchCell.BlastCell());
            }
            await Task.WhenAll(tasks);
            OnMatch?.Invoke();
        }
        
    }

    private BlockBase GetBooster(int _matchSize, GridCell grid)
    {
        if (matchSizeForVerticalRocket == _matchSize)
        {
            return blockFactory.SpawnVerticalRocket(grid);
        }
        
        if (matchSizeForHorizontalRocket == _matchSize)
        {
            return blockFactory.SpawnHorizontalRocket(grid);
        }
        
        return blockFactory.SpawnBomb(grid);
    }

    private MatchData SearchMatch(GridCell clickedCell)
    {
        matchSize = 0;
        matchedCells.Clear();

        MatchData match = CheckNeighbours(clickedCell);
        return match;
    }

    private MatchData CheckNeighbours(GridCell cell)
    {
        MatchData match = new(0, matchedCells, cell.block.blockType);

        List<GridCell> neighbours = cell.neighbours;

        foreach (GridCell neighbourCell in neighbours)
        {
            if (neighbourCell.block == null) continue;
            if (cell.block.blockType == BlockType.Obstacle) continue;

            bool isSameItem = neighbourCell.block.blockType == cell.block.blockType ;

            bool isAlreadyTraversed = matchedCells.Contains(neighbourCell);

            if (!isSameItem || isAlreadyTraversed) continue;

            matchSize++;
            matchedCells.Add(neighbourCell);
            CheckNeighbours(neighbourCell);
        }

        return new MatchData(matchSize, matchedCells, match.blockType);
    }
    

    private bool IsAnyBooster(BlockBase blockBase)
    {
        return blockBase.blockType == BlockType.Bomb || blockBase.blockType == BlockType.VerticalRocket ||
               blockBase.blockType == BlockType.HorizontalRocket;
    }
}