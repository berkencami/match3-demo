using Lean.Pool;
using UnityEngine;
using Zenject;

public class BlockFactory : MonoBehaviour
{
    [Inject] private LevelManager levelManager;
    [SerializeField] private MatchBlock[] matchBlocks;
    
    [SerializeField] private BlockBase bomb;
    [SerializeField] private BlockBase verticalRocket;
    [SerializeField] private BlockBase horizontalRocket;
    
    public MatchBlock RandomMatchBlock(int yCoord,Vector3 spawnPos)
    {
        int i = Random.Range(0, matchBlocks.Length);
        Vector3 upVector = Vector2.up * (levelManager.currentLevel.row - (yCoord + 1));
        spawnPos += upVector;
        return LeanPool.Spawn(matchBlocks[i], spawnPos + Vector3.up, Quaternion.identity);
    }

    public BlockBase SpawnBomb(GridCell grid)
    {
        BlockBase bombInstance = LeanPool.Spawn(bomb, grid.transform.position, Quaternion.identity);
        bombInstance.SetGrid(grid);
        return bombInstance;
    }
    
    public BlockBase SpawnVerticalRocket(GridCell grid)
    {
        BlockBase rocketInstance = LeanPool.Spawn(verticalRocket, grid.transform.position, Quaternion.identity);
        rocketInstance.SetGrid(grid);
        return rocketInstance;
    }
    
    public BlockBase SpawnHorizontalRocket(GridCell grid)
    {
        BlockBase rocketInstance = LeanPool.Spawn(horizontalRocket, grid.transform.position, Quaternion.identity);
        rocketInstance.SetGrid(grid);
        return rocketInstance;
    }
}
