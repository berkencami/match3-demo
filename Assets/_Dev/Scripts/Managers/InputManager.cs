using System;
using UnityEngine;

namespace _Dev.Scripts.Managers
{
   public class InputManager : Manager
   {
      public  Action<GridCell.GridCell> OnClickOnCell;
      public  Action<bool> OnTouch;
      private Camera _camera;
      private bool _canTouch=true;
   
      protected override void Awake()
      {
         base.Awake();
         _camera = Camera.main;
         OnTouch += SetInput;
      }

      protected override void LevelInstantiateProcess()
      {
         base.LevelInstantiateProcess();
         SetInput(true);
      }

      protected override void LevelEndProcess(bool status)
      {
         base.LevelEndProcess(status);
         SetInput(false);
      }

      protected override void OnDestroy()
      {
         base.OnDestroy();
         OnTouch-= SetInput;
      }

      private void Update()
      {
         if (!Input.GetMouseButtonDown(0)) return;
         Touch(Input.mousePosition);
      }
   
      private void Touch(Vector3 pos)
      {
         if(!_canTouch) return;
         var hit = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(pos)) as BoxCollider2D;
         if (hit == null || !hit.TryGetComponent(out GridCell.GridCell gridCell)) return;
         if(gridCell.Block==null) return;
         OnClickOnCell?.Invoke(gridCell);
      }

      private void SetInput(bool status)
      {
         _canTouch = status;
      }
   }
}
