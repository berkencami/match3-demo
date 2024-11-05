using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FPSCounter : MonoBehaviour
{
   [FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI _text;
   [FormerlySerializedAs("refreshTime")] [SerializeField] private float _refreshTime = 0.5f;
   private float _frameCounter;
   private float _timeCounter;
   private float _fps;

   private void Update()
   {
      if (_timeCounter < _refreshTime)
      {
         _timeCounter += Time.deltaTime;
         _frameCounter++;
      }
      else
      {
         _fps = _frameCounter / _timeCounter;
         _timeCounter = 0;
         _frameCounter = 0;
      }

      _text.text = "FPS: " + _fps.ToString("0.#");
   }
}
