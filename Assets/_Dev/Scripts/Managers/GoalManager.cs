using System;
using System.Collections.Generic;
using System.Linq;
using _Dev.Scripts.Block;
using Zenject;

namespace _Dev.Scripts.Managers
{
    public class GoalManager : Manager
    {
        [Inject] private LevelManager _levelManager;
   
        private readonly List<Goal.Goal> _currentGoals=new List<Goal.Goal>();
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
            _currentGoals.Clear();
            foreach (var newGoal in _levelManager.CurrentLevel.goals.Select(goal => new Goal.Goal()
                     {
                         Count = goal.Count,
                         Type = goal.Type,
                         Sprite = goal.Sprite
                     }))
            {
                _currentGoals.Add(newGoal);
            }
        }

        private void DecreaseGoals(BlockType blockType)
        {
            var goal = _currentGoals.Find(x => x.Type == blockType);
            if (goal == null) return;
            goal.Count--;
            if (goal.Count == 0)
            {
                _currentGoals.Remove(goal);
            }
        }

        public bool GoalsCompleted()
        {
            return _currentGoals.Count == 0;
        }

        public List<Goal.Goal> GetGoals()
        {
            return _currentGoals;
        }

    }
}
