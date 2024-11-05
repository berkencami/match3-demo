using System.Collections.Generic;
using System.Threading.Tasks;
using _Dev.Scripts.Data;
using _Dev.Scripts.Managers;
using Lean.Pool;
using UnityEngine;

namespace _Dev.Scripts.Block
{
   public class Bomb : BlockBase
   {
      public override async Task Blast()
      {
         if(Blasted) return;
         FXManager.Instance.PlayParticle(ParticleType, transform.position, Quaternion.Euler(-90, 0, 0));
         FXManager.Instance.PlaySoundFX(SoundType.Bomb);
         _gridCell = null;
         await ExecuteBooster();
      }

      private async Task ExecuteBooster()
      {
         Blasted = true;
         Collider2D[] grids = Physics2D.OverlapCircleAll(transform.position, 1);
         List<Task> tasks = new List<Task>();
         foreach (Collider2D collider in grids)
         {
            if (collider.TryGetComponent(out GridCell.GridCell targetGrid))
            {
               if (targetGrid.Block != null && !targetGrid.Block.Blasted)
               {
                  tasks.Add(targetGrid.Block.Blast());
                  targetGrid.SetBlock(null);
               }
           
            }
         
         }
         LeanPool.Despawn(gameObject);
         await Task.WhenAll(tasks);
     
      }
   }
}
