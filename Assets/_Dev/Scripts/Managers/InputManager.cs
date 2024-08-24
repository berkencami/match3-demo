using System;
using UnityEngine;

public class InputManager : Manager
{
   public  Action<GridCell> OnClickOnCell;
   public  Action<bool> OnTouch;
   private Camera camera;
   private bool canTouch=true;
   
   protected override void Awake()
   {
      base.Awake();
      camera = Camera.main;
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
      if(!canTouch) return;
      BoxCollider2D hit = Physics2D.OverlapPoint(camera.ScreenToWorldPoint(pos)) as BoxCollider2D;
      if (hit == null || !hit.TryGetComponent(out GridCell gridCell)) return;
      if(gridCell.block==null) return;
      OnClickOnCell?.Invoke(gridCell);
   }

   private void SetInput(bool status)
   {
      canTouch = status;
   }
}
