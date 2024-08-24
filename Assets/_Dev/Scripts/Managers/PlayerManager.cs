using System;
using Zenject;

public class PlayerManager : Manager
{
    [Inject] private GoalManager goalManager;
    [Inject] private MoveManager moveManager;
    [Inject] private InputManager inputManager;
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
        if (goalManager.GoalsCompleted())
        {
            gameManager.OnLevelEnd?.Invoke(true);
            inputManager.OnTouch?.Invoke(false);
            return;
        }

        if (moveManager.GetAnyMove()) return;
        gameManager.OnLevelEnd?.Invoke(false);
        inputManager.OnTouch?.Invoke(false);
    }
    
}
