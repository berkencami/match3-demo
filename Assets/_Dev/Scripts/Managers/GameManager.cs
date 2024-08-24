using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action OnGameStart;
    public Action OnPrepareLevel;
    public Action OnLevelInstantiate;
    public Action PostLevelInstantiate;
    public Action<bool> OnLevelEnd;
    public Action<GameState> OnStateChange;

    public GameState gameState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        OnLevelEnd += LevelEnd;
    }

    private void OnDestroy()
    {
        OnLevelEnd -= LevelEnd;
    }

    private void Start()
    {
        OnGameStart?.Invoke();
    }

    private void SetState(GameState newState)
    {
        if (gameState == newState) return;
        gameState = newState;
        OnStateChange?.Invoke(gameState);
    }

    public async void PrepareLevel()
    {
        SetState(GameState.Load);
        OnPrepareLevel?.Invoke();
        await Task.Yield();
        OnLevelInstantiate?.Invoke();
        await Task.Yield();
        PostLevelInstantiate?.Invoke();
        SetState(GameState.InGame);
    }

    private void LevelEnd(bool levelStatus)
    {
        SetState(levelStatus ? GameState.Success : GameState.Fail);
    }

}