using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI text;
   [SerializeField] private float refreshTime = 0.5f;
   private float frameCounter;
   private float timeCounter;
   private float fps;

   private void Update()
   {
      if (timeCounter < refreshTime)
      {
         timeCounter += Time.deltaTime;
         frameCounter++;
      }
      else
      {
         fps = frameCounter / timeCounter;
         timeCounter = 0;
         frameCounter = 0;
      }

      text.text = "FPS: " + fps.ToString("0.#");
   }
}
