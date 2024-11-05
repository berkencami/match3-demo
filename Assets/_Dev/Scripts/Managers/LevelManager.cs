using System.Collections.Generic;
using _Dev.Scripts.Installers;
using LevelEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class LevelManager : Manager
    {
        [Inject] public FactoryInstaller.GridFactory GridFactory;
        [Inject] public BoardManager BoardManager;

        [FormerlySerializedAs("levels")] [SerializeField] private List<Level> _levels;
        private int _currentLevelIndex;
        public Level CurrentLevel { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);
        }

        protected override void PreLevelInstantiateProcess()
        {
            base.PreLevelInstantiateProcess();
            CurrentLevel = GetLevel();
        }

        protected override void LevelInstantiateProcess()
        {
            base.LevelInstantiateProcess();
            LevelLoader.LoadLevel(CurrentLevel, GridFactory, BoardManager);
        }

        protected override void LevelEndProcess(bool status)
        {
            base.LevelEndProcess(status);
            if (!status) return;
            _currentLevelIndex++;
            PlayerPrefs.SetInt("currentLevelIndex", _currentLevelIndex);
        }

        private Level GetLevel()
        {
            int levelIndex = _currentLevelIndex;
            levelIndex %= _levels.Count;
            return _levels[levelIndex];
        }
    }
}