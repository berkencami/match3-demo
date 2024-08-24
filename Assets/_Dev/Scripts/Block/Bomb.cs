using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

public class Bomb : BlockBase
{
   public override async Task Blast()
   {
      if(blasted) return;
      FXManager.instance.PlayParticle(particleType, transform.position, Quaternion.Euler(-90, 0, 0));
      FXManager.instance.PlaySoundFX(SoundType.Bomb);
      gridCell = null;
      await ExecuteBooster();
   }

   private async Task ExecuteBooster()
   {
      blasted = true;
      Collider2D[] grids = Physics2D.OverlapCircleAll(transform.position, 1);
      List<Task> tasks = new List<Task>();
      foreach (Collider2D collider in grids)
      {
         if (collider.TryGetComponent(out GridCell targetGrid))
         {
            if (targetGrid.block != null && !targetGrid.block.blasted)
            {
               tasks.Add(targetGrid.block.Blast());
               targetGrid.block = null;
            }
           
         }
         
      }
      LeanPool.Despawn(gameObject);
      await Task.WhenAll(tasks);
     
   }
}
