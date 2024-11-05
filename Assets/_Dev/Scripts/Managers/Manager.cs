using UnityEngine;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class Manager : MonoBehaviour
    {
        [Inject] protected GameManager _gameManager;
  
        protected virtual void Awake()
        {
            _gameManager.OnGameStart += GameStartProcess;
            _gameManager.OnPrepareLevel += PreLevelInstantiateProcess;
            _gameManager.OnLevelInstantiate += LevelInstantiateProcess;
            _gameManager.PostLevelInstantiate += PostLevelInstantiateProcess;
            _gameManager.OnLevelEnd += LevelEndProcess;
        }

        protected virtual void OnDestroy()
        {
            _gameManager.OnGameStart -= GameStartProcess;
            _gameManager.OnPrepareLevel -= PreLevelInstantiateProcess;
            _gameManager.OnLevelInstantiate -= LevelInstantiateProcess;
            _gameManager.PostLevelInstantiate -= PostLevelInstantiateProcess;
            _gameManager.OnLevelEnd -= LevelEndProcess;
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
}
