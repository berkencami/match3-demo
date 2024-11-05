using System.Collections.Generic;
using System.Threading.Tasks;
using _Dev.Scripts.Data;
using _Dev.Scripts.Managers;
using Lean.Pool;
using UnityEngine;

namespace _Dev.Scripts.Block.Rocket
{
    public enum RocketType
    {
        Horizontal,
        Vertical
    }

    public class Rocket : BlockBase
    {
        [SerializeField] private RocketType _rocketType;
        [SerializeField] private RocketPart _partOne;
        [SerializeField] private RocketPart _partTwo;
        private Vector3 _partOneInitPos;
        private Vector3 _partTwoInitPos;
    
        protected override void Awake()
        {
            base.Awake();
            _partOneInitPos = _partOne.transform.localPosition;
            _partTwoInitPos = _partTwo.transform.localPosition;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _partOne.transform.localPosition = _partOneInitPos;
            _partTwo.transform.localPosition = _partTwoInitPos;
        }

        public override async Task Blast()
        {
            if (Blasted) return;
            Blasted = true;
            FXManager.Instance.PlaySoundFX(SoundType.Rocket);
            await Task.Yield();
            await ExecuteBooster(_gridCell.MatchManager);
            _gridCell = null;
        }

        private async Task ExecuteBooster(MatchManager matchManager)
        {
            var tasks = new List<Task>();
            if (_rocketType == RocketType.Horizontal)
            {
                tasks.Add(_partOne.Execute(Vector2.right, _rocketType));
                tasks.Add(_partTwo.Execute(Vector2.left, _rocketType));
            }
            else
            {
                tasks.Add(_partOne.Execute(Vector2.up, _rocketType));
                tasks.Add(_partTwo.Execute(Vector2.down, _rocketType));
            }

            await Task.WhenAll(tasks);
            matchManager.OnMatch?.Invoke();
            LeanPool.Despawn(gameObject);
        }
    }
}