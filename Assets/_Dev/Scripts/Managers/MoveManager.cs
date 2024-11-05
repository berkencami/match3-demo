using System;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class MoveManager : Manager
    {
        [Inject] private LevelManager _levelManager;

        public Action OnDecreaseMove;
        private int _moveCount;

        protected override void Awake()
        {
            base.Awake();
            OnDecreaseMove += DecreaseMoveCount;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDecreaseMove -= DecreaseMoveCount;
        }

        protected override void PostLevelInstantiateProcess()
        {
            base.PostLevelInstantiateProcess();
            _moveCount = _levelManager.CurrentLevel.moves;
        }
   
        private void DecreaseMoveCount()
        {
            _moveCount--;
        }

        public bool GetAnyMove()
        {
            return _moveCount > 0;
        }

        public int GetMove()
        {
            return _moveCount;
        }
    }
}
