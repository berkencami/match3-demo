using System.Collections.Generic;
using System.Threading.Tasks;
using _Dev.Scripts.Block;
using _Dev.Scripts.Managers;
using _Dev.Scripts.Utility;
using UnityEngine;
using Zenject;

namespace _Dev.Scripts.GridCell
{
    public class GridCell : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private BoardManager _boardManager;
        [Inject] public MatchManager MatchManager;
        [Inject] public GoalManager GoalManager;

        private Vector2Int _coordinates;
        private List<GridCell> _neighbours = new List<GridCell>();
    
        private BlockBase _block;
    
        public BlockBase Block=>_block;
        public Vector2Int Coordinates => _coordinates;
        public List<GridCell> Neighbours => _neighbours;

        [Inject]
        public void Construct(LevelManager _levelManager, MatchManager _matchManager, GoalManager _goalManager,BoardManager _boardManager)
        {
            this._levelManager = _levelManager;
            MatchManager = _matchManager;
            GoalManager = _goalManager;
            this._boardManager = _boardManager;
        }

        public void SetCoords(int x, int y)
        {
            _coordinates = new Vector2Int(x, y);
        }

        public void SetBlock(BlockBase block)
        {
            _block = block;
        }

        public void SetNeighbours()
        {
            _neighbours = GetNeighbours(Direction.Up, Direction.Down, Direction.Left, Direction.Right);
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
            var x = _coordinates.x;
            var y = _coordinates.y;
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

            if (x >= _levelManager.CurrentLevel.row || x < 0 || y >= _levelManager.CurrentLevel.column || y < 0)
            {
                return null;
            }

            return _boardManager.GridCells[x, y];
        }

        private void CheckNeighbourObstacle()
        {
            foreach (var neighbourGrid in _neighbours)
            {
                if (neighbourGrid._block == null) continue;

                if (neighbourGrid._block.BlockType == BlockType.Obstacle)
                {
                    neighbourGrid.BlastObstacle();
                }
            }
        }

        public async void BlastObstacle()
        {
            var currentBlock = _block;
            _block = null;
            await currentBlock.Blast();
        }

        public async Task BlastWithRocket()
        {
            if(_block==null) return;
            BlockBase currentBlock = _block;
            _block = null;
            await currentBlock.Blast();
        }

        public async Task BlastCell()
        {
            if(_block==null) return;
      
            if (!IsBooster(_block))
            {
                CheckNeighbourObstacle();
            }
        
            var currentBlock = _block;
            _block = null;
            await currentBlock.Blast();
        }

        public async Task MergeCell(Transform mergePoint)
        {
            if(_block==null) return;
            CheckNeighbourObstacle();
            var currentBlock = _block;
            _block = null;
            await currentBlock.MergeBlock(mergePoint);
        }
    
        private bool IsBooster(BlockBase blockBase)
        {
            return blockBase.BlockType == BlockType.Bomb || blockBase.BlockType == BlockType.VerticalRocket ||
                   blockBase.BlockType == BlockType.HorizontalRocket;
        }
    
    }
}