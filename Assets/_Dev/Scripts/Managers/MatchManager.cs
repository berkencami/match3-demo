using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Dev.Scripts.Block;
using _Dev.Scripts.Data;
using UnityEngine;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class MatchManager : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;
        [Inject] private BlockFactory _blockFactory;
        [Inject] private MoveManager _moveManager;

        public Action OnMatch;

        [Header("Match refs")]
        [SerializeField] private int _minMatchSize = 2;
        [SerializeField] private int _matchSizeForVerticalRocket = 5;
        [SerializeField] private int _matchSizeForHorizontalRocket = 7;

        private readonly List<GridCell.GridCell> _matchedCells = new List<GridCell.GridCell>();
        private int _matchSize = 0;

        private void Awake()
        {
            _inputManager.OnClickOnCell += OnClickOnCell;
        }

        private void OnDestroy()
        {
            _inputManager.OnClickOnCell -= OnClickOnCell;
        }

        private async void OnClickOnCell(GridCell.GridCell cell)
        {
            var matchData = SearchMatch(cell);

            if (IsBooster(cell.Block))
            {
                _moveManager.OnDecreaseMove?.Invoke();
                if (cell.Block.BlockType == BlockType.Bomb)
                {
                    await cell.BlastCell();
                    OnMatch?.Invoke();
                    return;
                }

                await cell.BlastCell();
                return;
            }

            if (matchData.Size < _minMatchSize) return;

            _moveManager.OnDecreaseMove?.Invoke();
            await Match(matchData, cell);
        }

        private async Task Match(MatchData matchData, GridCell.GridCell gridCell)
        {
            var tasks = new List<Task>();
            if (_matchSizeForVerticalRocket <= matchData.Size)
            {
                FXManager.Instance.PlaySoundFX(SoundType.Merge);
                foreach (var matchCell in matchData.GridCells)
                {
                    tasks.Add(matchCell.MergeCell(gridCell.transform));
                }

                await Task.WhenAll(tasks);
                gridCell.SetBlock(GetBooster(matchData.Size, gridCell));
                await Task.Yield();
                OnMatch?.Invoke();
            }
            else
            {
                FXManager.Instance.PlaySoundFX(SoundType.Match);
                foreach (var matchCell in matchData.GridCells)
                {
                    tasks.Add(matchCell.BlastCell());
                }

                await Task.WhenAll(tasks);
                OnMatch?.Invoke();
            }
        }

        private BlockBase GetBooster(int _matchSize, GridCell.GridCell grid)
        {
            if (_matchSizeForVerticalRocket == _matchSize)
            {
                return _blockFactory.SpawnVerticalRocket(grid);
            }

            return _matchSizeForHorizontalRocket == _matchSize
                ? _blockFactory.SpawnHorizontalRocket(grid)
                : _blockFactory.SpawnBomb(grid);
        }

        private MatchData SearchMatch(GridCell.GridCell clickedCell)
        {
            _matchSize = 0;
            _matchedCells.Clear();

            var match = CheckNeighbours(clickedCell);
            return match;
        }

        private MatchData CheckNeighbours(GridCell.GridCell cell)
        {
            MatchData match = new(0, _matchedCells, cell.Block.BlockType);

            var neighbours = cell.Neighbours;

            foreach (var neighbourCell in neighbours)
            {
                if (neighbourCell.Block == null) continue;
                if (cell.Block.BlockType == BlockType.Obstacle) continue;

                bool isSameItem = neighbourCell.Block.BlockType == cell.Block.BlockType;

                bool isAlreadyTraversed = _matchedCells.Contains(neighbourCell);

                if (!isSameItem || isAlreadyTraversed) continue;

                _matchSize++;
                _matchedCells.Add(neighbourCell);
                CheckNeighbours(neighbourCell);
            }

            return new MatchData(_matchSize, _matchedCells, match.BlockType);
        }


        private bool IsBooster(BlockBase blockBase)
        {
            return blockBase.BlockType == BlockType.Bomb || blockBase.BlockType == BlockType.VerticalRocket ||
                   blockBase.BlockType == BlockType.HorizontalRocket;
        }
    }
}