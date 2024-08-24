using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class UIManager : Manager
{
    [Inject] private MoveManager moveManager;
    [Inject] private GoalManager goalManager;
   
    [Header("Game canvases")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject inGameCanvas;

    [Header("Game Panels")] 
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    
    [Header("Move Ui")] 
    [SerializeField] private TextMeshProUGUI moveText;
    
    [Header("Goal Ui")]
    [SerializeField] private Transform goalContainer;
    [SerializeField] private GoalUI goalUI;
    private List<GoalUI> goalUis = new List<GoalUI>();

    [Header("PlayButton")]
    [SerializeField] private TextMeshProUGUI playButtonText;


    protected override void Awake()
    {
        base.Awake();
        gameManager.OnStateChange += SetState;
        moveManager.OnDecreaseMove += UpdateMoveText;
        goalManager.OnCheckGoals += UpdateGoalUI;
        SetState(GameState.Init);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        moveManager.OnDecreaseMove -= UpdateMoveText;
        goalManager.OnCheckGoals -= UpdateGoalUI;
    }

    protected override void GameStartProcess()
    {
        base.GameStartProcess();
        playButtonText.text = "Level "+(PlayerPrefs.GetInt("currentLevelIndex", 0)+1);
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
                menuCanvas.SetActive(true);
                inGameCanvas.SetActive(false);
                winPanel.SetActive(false);
                failPanel.SetActive(false);
                break;
            
            case GameState.Load:
                FXManager.instance.StopSound();
                winPanel.SetActive(false);
                failPanel.SetActive(false);
                break;
            
            case GameState.InGame:
                menuCanvas.SetActive(false);
                inGameCanvas.SetActive(true);
                break;
            
            case GameState.Success:
                winPanel.SetActive(true);
                break;
            
            case GameState.Fail:
                failPanel.SetActive(true);
                break;
            
        }
    }

    private void UpdateMoveText()
    {
        moveText.text = moveManager.GetMove().ToString();
    }

    private void InitializeGoalUI()
    {
        foreach (var goalUi in goalUis)
        {
            Destroy(goalUi.gameObject);
        }
        
        goalUis.Clear();
        
        foreach (Goal goal in goalManager.GetGoals())
        {
            GoalUI newGoalUi = Instantiate(goalUI,goalContainer).GetComponent<GoalUI>();
            newGoalUi.InitializeGoal(goal.sprite,goal.count,goal.type);
            goalUis.Add(newGoalUi);
        }
    }

    private void UpdateGoalUI(BlockType type)
    {
        GoalUI goal = goalUis.Find(x => x.blockType == type);
        if (goal != null)
        {
            goal.DecreaseGoal();
        }
    }
}
