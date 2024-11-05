using System.Collections.Generic;
using _Dev.Scripts.Block;
using _Dev.Scripts.Data;
using _Dev.Scripts.Goal;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class UIManager : Manager
    {
        [Inject] private MoveManager _moveManager;
        [Inject] private GoalManager _goalManager;
    
        [Header("Game canvases")]
        [SerializeField] private GameObject _menuCanvas;
        [SerializeField] private GameObject _inGameCanvas;
    
        [Header("Game Panels")] 
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _failPanel;
    
        [Header("Move Ui")] 
        [SerializeField] private TextMeshProUGUI _moveText;
    
        [Header("Goal Ui")]
        [SerializeField] private Transform _goalContainer; 
        [SerializeField] private GoalUI _goalUI;
        private readonly List<GoalUI> _goalUis = new List<GoalUI>();
    
        [Header("PlayButton")]
        [SerializeField] private TextMeshProUGUI _playButtonText;
    
        protected override void Awake()
        {
            base.Awake();
            _gameManager.OnStateChange += SetState;
            _moveManager.OnDecreaseMove += UpdateMoveText;
            _goalManager.OnCheckGoals += UpdateGoalUI;
            SetState(GameState.Init);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _moveManager.OnDecreaseMove -= UpdateMoveText;
            _goalManager.OnCheckGoals -= UpdateGoalUI;
        }

        protected override void GameStartProcess()
        {
            base.GameStartProcess();
            _playButtonText.text = "Level "+(PlayerPrefs.GetInt("currentLevelIndex", 0)+1);
        }

        protected override void PostLevelInstantiateProcess()
        {
            base.PostLevelInstantiateProcess();
            UpdateMoveText();
            InitializeGoalUI();
        }

        private void SetState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Init:
                    _menuCanvas.SetActive(true);
                    _inGameCanvas.SetActive(false);
                    _winPanel.SetActive(false);
                    _failPanel.SetActive(false);
                    break;
            
                case GameState.Load:
                    FXManager.Instance.StopSound();
                    _winPanel.SetActive(false);
                    _failPanel.SetActive(false);
                    break;
            
                case GameState.InGame:
                    _menuCanvas.SetActive(false);
                    _inGameCanvas.SetActive(true);
                    break;
            
                case GameState.Success:
                    _winPanel.SetActive(true);
                    break;
            
                case GameState.Fail:
                    _failPanel.SetActive(true);
                    break;
            
            }
        }

        private void UpdateMoveText()
        {
            _moveText.text = _moveManager.GetMove().ToString();
        }

        private void InitializeGoalUI()
        {
            foreach (var goalUi in _goalUis)
            {
                Destroy(goalUi.gameObject);
            }
        
            _goalUis.Clear();
        
            foreach (Goal.Goal goal in _goalManager.GetGoals())
            {
                GoalUI newGoalUi = Instantiate(_goalUI,_goalContainer).GetComponent<GoalUI>();
                newGoalUi.InitializeGoal(goal.Sprite,goal.Count,goal.Type);
                _goalUis.Add(newGoalUi);
            }
        }

        private void UpdateGoalUI(BlockType type)
        {
            GoalUI goal = _goalUis.Find(x => x.BlockType == type);
            if (goal != null)
            {
                goal.DecreaseGoal();
            }
        }
    }
}
