using System.Collections.Generic;
using UnityEngine;
using LevelEditor;
using Zenject;

public class LevelManager : Manager
{
    [Inject] public FactoryInstaller.GridFactory gridFactory;
    [Inject] public BoardManager boardManager;

    [SerializeField] private List<Level> levels;
    private int currentLevelIndex;
    public Level currentLevel { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);
    }

    protected override void PreLevelInstantiateProcess()
    {
        base.PreLevelInstantiateProcess();
        currentLevel = GetLevel();
    }

    protected override void LevelInstantiateProcess()
    {
        base.LevelInstantiateProcess();
        LevelLoader.LoadLevel(currentLevel, gridFactory, boardManager);
    }

    protected override void LevelEndProcess(bool status)
    {
        base.LevelEndProcess(status);
        if (!status) return;
        currentLevelIndex++;
        PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
    }

    private Level GetLevel()
    {
        int levelIndex = currentLevelIndex;
        levelIndex %= levels.Count;
        return levels[levelIndex];
    }
}