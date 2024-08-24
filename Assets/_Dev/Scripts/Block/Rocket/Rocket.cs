using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

public enum RocketType
{
   Horizontal,
   Vertical
}

public class Rocket : BlockBase
{
   public RocketType rocketType;
   [SerializeField] private RocketPart partOne;
   [SerializeField] private RocketPart partTwo;
   private Vector3 partOneInitPos;
   private Vector3 partTwoInitPos;

   protected override void Awake()
   {
      base.Awake();
      partOneInitPos = partOne.transform.localPosition;
      partTwoInitPos = partTwo.transform.localPosition;
   }

   protected override void OnEnable()
   {
      base.OnEnable();
       partOne.transform.localPosition = partOneInitPos;
       partTwo.transform.localPosition = partTwoInitPos;
   }

   public override async Task Blast()
   {
      if(blasted) return;
      blasted = true;
      FXManager.instance.PlaySoundFX(SoundType.Bomb);
      await Task.Yield();
      await ExecuteBooster(gridCell.matchManager);
      gridCell = null;
   }

   private async Task ExecuteBooster(MatchManager matchManager)
   {
      List<Task> tasks = new List<Task>();
      if (rocketType == RocketType.Horizontal)
      {
         tasks.Add(partOne.Execute(Vector2.right,rocketType));
         tasks.Add(partTwo.Execute(Vector2.left,rocketType));
      }
      else
      {
         tasks.Add(partOne.Execute(Vector2.up,rocketType));
         tasks.Add(partTwo.Execute(Vector2.down,rocketType));
      }

      await Task.WhenAll(tasks);
      matchManager.OnMatch?.Invoke();
      LeanPool.Despawn(gameObject);
   }
}
