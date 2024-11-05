using System.Threading.Tasks;
using _Dev.Scripts.Data;
using _Dev.Scripts.Managers;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace _Dev.Scripts.Block
{
    public class BlockBase : MonoBehaviour
    {
        [Header("Block Refs")] private bool _blasted;
        [SerializeField] private BlockType _blockType;
        [SerializeField] private ParticleType _particleType;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _canFall;

        protected GridCell.GridCell _gridCell;

        [Header("Fall Refs")] [SerializeField] private float _fallDuration = 0.3f;

        public bool CanFall => _canFall;
        public BlockType BlockType => _blockType;
        protected ParticleType ParticleType => _particleType;

        public bool Blasted
        {
            get => _blasted;
            protected set => _blasted = value;
        }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected virtual void OnEnable()
        {
            _blasted = false;
        }

        private void OnDisable()
        {
            _gridCell = null;
        }

        public void SetGrid(GridCell.GridCell newGrid)
        {
            if (newGrid == _gridCell)
            {
                return;
            }

            var oldCell = _gridCell;
            _gridCell = newGrid;

            if (oldCell != null && oldCell.Block == this)
            {
                oldCell.SetBlock(null);
            }

            if (newGrid != null)
            {
                newGrid.SetBlock(this);
            }
        }

        public void SetSpriteLayer()
        {
            _spriteRenderer.sortingOrder = (int)(transform.position.y * 10);
        }

        public virtual async Task Blast()
        {
            if (_blasted) return;
            _blasted = true;
            _gridCell.GoalManager.OnCheckGoals?.Invoke(_blockType);
            FXManager.Instance.PlayParticle(_particleType, transform.position, Quaternion.Euler(-90, 0, 0));
            _gridCell = null;
            LeanPool.Despawn(gameObject);
            await Task.Yield();
        }

        public async Task MergeBlock(Transform mergePoint)
        {
            _gridCell.GoalManager.OnCheckGoals?.Invoke(_blockType);
            await transform.DOMove(mergePoint.position, _fallDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                _gridCell = null;
                LeanPool.Despawn(gameObject);
            }).AsyncWaitForCompletion();
        }

        public async Task Fall()
        {
            if (!_canFall) return;
            if (_gridCell == null) return;

            DOTween.Kill(transform, true);
            await transform.DOMove(_gridCell.transform.position, _fallDuration)
                .OnUpdate(SetSpriteLayer).AsyncWaitForCompletion();
        }
    }
}