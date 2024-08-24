using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    public bool canBlast;
    private Transform rocketTransform;

    private void OnEnable()
    {
        canBlast = false;
    }

    private void Awake()
    {
        rocketTransform = GetComponent<Transform>();
    }

    public async Task Execute(Vector2 direction,RocketType rocketType)
    {  
        ParticleSystem particleInstance = FXManager.instance
            .PlayParticle(ParticleType.RocketTrail, Vector3.zero, Quaternion.identity, rocketTransform)
            .GetComponent<ParticleSystem>();
        particleInstance.Clear();
        
        canBlast = true;
        if (rocketType == RocketType.Horizontal)
        {
            await rocketTransform.DOMoveX(direction.x * 10, .537f).SetEase(Ease.Linear).AsyncWaitForCompletion();
           
        }
        else
        {
            await rocketTransform.DOMoveY(direction.y * 10, .537f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }
        particleInstance.transform.SetParent(null);

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!canBlast) return;
        if (other.TryGetComponent(out GridCell gridCell))
        {
            if(gridCell.block==null) return;
            if (gridCell.block.blockType == BlockType.Obstacle)
            {
                gridCell.BlastObstacle();
            }
            else
            {
                gridCell.BlastWithRocket();
            }
           
        }
    }
}