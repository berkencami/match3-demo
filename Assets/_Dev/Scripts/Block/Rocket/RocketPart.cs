using System.Threading.Tasks;
using _Dev.Scripts.Data;
using _Dev.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Dev.Scripts.Block.Rocket
{
    public class RocketPart : MonoBehaviour
    {
        private bool _canBlast;
        private Transform _rocketTransform;

        private void OnEnable()
        {
            _canBlast = false;
        }

        private void Awake()
        {
            _rocketTransform = GetComponent<Transform>();
        }

        public async Task Execute(Vector2 direction,RocketType rocketType)
        {  
            ParticleSystem particleInstance = FXManager.Instance
                .PlayParticle(ParticleType.RocketTrail, Vector3.zero, Quaternion.identity, _rocketTransform)
                .GetComponent<ParticleSystem>();
            particleInstance.Clear();
        
            _canBlast = true;
            if (rocketType == RocketType.Horizontal)
            {
                await _rocketTransform.DOMoveX(direction.x * 10, .537f).SetEase(Ease.Linear).AsyncWaitForCompletion();
           
            }
            else
            {
                await _rocketTransform.DOMoveY(direction.y * 10, .537f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }
            particleInstance.transform.SetParent(null);

        }
    
        private async void OnTriggerEnter2D(Collider2D other)
        {
            if(!_canBlast) return;
            if (!other.TryGetComponent(out GridCell.GridCell gridCell)) return;
            if(gridCell.Block==null) return;
            if (gridCell.Block.BlockType == BlockType.Obstacle)
            {
                gridCell.BlastObstacle();
            }
            else
            { 
                await gridCell.BlastWithRocket();
            }
        }
    }
}