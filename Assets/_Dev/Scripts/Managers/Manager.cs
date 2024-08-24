using UnityEngine;
using Zenject;

public class Manager : MonoBehaviour
{
    [Inject] protected GameManager gameManager;
  
    protected virtual void Awake()
    {
        gameManager.OnGameStart += GameStartProcess;
        gameManager.OnPrepareLevel += PreLevelInstantiateProcess;
        gameManager.OnLevelInstantiate += LevelInstantiateProcess;
        gameManager.PostLevelInstantiate += PostLevelInstantiateProcess;
        gameManager.OnLevelEnd += LevelEndProcess;
    }

    protected virtual void OnDestroy()
    {
        gameManager.OnGameStart -= GameStartProcess;
        gameManager.OnPrepareLevel -= PreLevelInstantiateProcess;
        gameManager.OnLevelInstantiate -= LevelInstantiateProcess;
        gameManager.PostLevelInstantiate -= PostLevelInstantiateProcess;
        gameManager.OnLevelEnd -= LevelEndProcess;
    }

    protected virtual void GameStartProcess()
    {
    }
    
    protected virtual void PreLevelInstantiateProcess()
    {
     
    }

    protected virtual void LevelInstantiateProcess()
    {
      
    }
    
    protected virtual void PostLevelInstantiateProcess()
    {
    }

    protected virtual void LevelEndProcess(bool status)
    {
        
    }
}
