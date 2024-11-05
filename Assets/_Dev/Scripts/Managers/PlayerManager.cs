using System;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class PlayerManager : Manager
    {
        [Inject] private GoalManager _goalManager;
        [Inject] private MoveManager _moveManager;
        [Inject] private InputManager _inputManager;
        public Action OnCheckGame;

        protected override void Awake()
        {
            base.Awake();
            OnCheckGame += CheckGame;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnCheckGame -= CheckGame;
        }

        private void CheckGame()
        {
            if (_goalManager.GoalsCompleted())
            {
                _gameManager.OnLevelEnd?.Invoke(true);
                _inputManager.OnTouch?.Invoke(false);
                return;
            }

            if (_moveManager.GetAnyMove()) return;
            _gameManager.OnLevelEnd?.Invoke(false);
            _inputManager.OnTouch?.Invoke(false);
        }
    
    }
}
