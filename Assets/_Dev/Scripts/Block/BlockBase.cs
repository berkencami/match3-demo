using System.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    [Header("Block Refs")]
    public BlockType blockType;
    public ParticleType particleType;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool fallable;
    public bool Fallable => fallable;
    protected GridCell gridCell;

    [Header("Fall Refs")]
    [SerializeField] private float fallDuration = 0.3f;
    public bool blasted;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        blasted = false;
    }
    
    private void OnDisable()
    {
        gridCell = null;
    }

    public void SetGrid(GridCell newGrid)
    {
        if (newGrid == gridCell)
        {
            return;
        }

        GridCell oldCell = gridCell;
        gridCell = newGrid;

        if (oldCell != null && oldCell.block == this)
        {
            oldCell.block = null;
        }

        if (newGrid != null)
        {
            newGrid.block = this;
            
        }
    }
    
    public void SetSpriteLayer()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y*10);
    }

    public virtual async Task Blast()
    {
        if(blasted) return;
        blasted = true;
        gridCell.goalManager.OnCheckGoals?.Invoke(blockType);
        FXManager.instance.PlayParticle(particleType, transform.position, Quaternion.Euler(-90, 0, 0));
        gridCell = null;
        LeanPool.Despawn(gameObject);
        await Task.Yield();
    }

    public async Task MergeBlock(Transform mergePoint)
    {
        gridCell.goalManager.OnCheckGoals?.Invoke(blockType);
        await transform.DOMove(mergePoint.position, fallDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            gridCell = null;
            LeanPool.Despawn(gameObject);
        }).AsyncWaitForCompletion();

    }

    [Button]
    public async Task Fall()
    {
        if(!fallable) return;
        if(gridCell==null) return;
        
        DOTween.Kill(transform,true); 
        await transform.DOMove(gridCell.transform.position, fallDuration)
            .OnUpdate(SetSpriteLayer).AsyncWaitForCompletion();
    }
}