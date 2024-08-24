using System;
using Zenject;

public class MoveManager : Manager
{
   [Inject] private LevelManager levelManager;

   public Action OnDecreaseMove;
   private int moveCount;

   protected override void Awake()
   {
       base.Awake();
       OnDecreaseMove += DecreaseMoveCount;
   }

   protected override void OnDestroy()
   {
       base.OnDestroy();
       OnDecreaseMove -= DecreaseMoveCount;
   }

   protected override void PostLevelInstantiateProcess()
   {
       base.PostLevelInstantiateProcess();
       moveCount = levelManager.currentLevel.moves;
   }
   
   private void DecreaseMoveCount()
   {
       moveCount--;
   }

   public bool GetAnyMove()
   {
       return moveCount > 0;
   }

   public int GetMove()
   {
       return moveCount;
   }
}
