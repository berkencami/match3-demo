using System;
using System.Collections.Generic;
using Zenject;

public class GoalManager : Manager
{
    [Inject] private LevelManager levelManager;
   
    private readonly List<Goal> currentGoals=new List<Goal>();
    public Action<BlockType> OnCheckGoals;

    protected override void Awake()
    {
        base.Awake();
        OnCheckGoals += DecreaseGoals;
    }

    protected override void OnDestroy()
    {
        base.Awake();
        OnCheckGoals -= DecreaseGoals;
    }

    protected override void LevelInstantiateProcess()
    {
        base.LevelInstantiateProcess();
        CreateGoals();
    }
    
    private void CreateGoals()
    {
        currentGoals.Clear();
        foreach (Goal goal in levelManager.currentLevel.goals)
        {
            Goal newGoal = new Goal()
            {
                count = goal.count,
                type = goal.type,
                sprite = goal.sprite
            };
            currentGoals.Add(newGoal);
        }
    }

    private void DecreaseGoals(BlockType blockType)
    {
        Goal goal = currentGoals.Find(x => x.type == blockType);
        if (goal == null) return;
        goal.count--;
        if (goal.count == 0)
        {
            currentGoals.Remove(goal);
        }
    }

    public bool GoalsCompleted()
    {
        return currentGoals.Count == 0;
    }

    public List<Goal> GetGoals()
    {
        return currentGoals;
    }

}
