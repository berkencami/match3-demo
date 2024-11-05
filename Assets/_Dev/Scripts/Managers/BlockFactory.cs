using _Dev.Scripts.Block;
using _Dev.Scripts.GridCell;
using _Dev.Scripts.Managers;
using Lean.Pool;
using UnityEngine;
using Zenject;

public class BlockFactory : MonoBehaviour
{
    [Inject] private LevelManager _levelManager;
    [SerializeField] private MatchBlock[] _matchBlocks;

    [SerializeField] private BlockBase _bomb;
    [SerializeField] private BlockBase _verticalRocket;
    [SerializeField] private BlockBase _horizontalRocket;

    public MatchBlock RandomMatchBlock(int yCoord, Vector3 spawnPos)
    {
        var i = Random.Range(0, _matchBlocks.Length);
        Vector3 upVector = Vector2.up * (_levelManager.CurrentLevel.row - (yCoord + 1));
        spawnPos += upVector;
        return LeanPool.Spawn(_matchBlocks[i], spawnPos + Vector3.up, Quaternion.identity);
    }

    public BlockBase SpawnBomb(GridCell grid)
    {
        var bombInstance = LeanPool.Spawn(_bomb, grid.transform.position, Quaternion.identity);
        bombInstance.SetGrid(grid);
        return bombInstance;
    }

    public BlockBase SpawnVerticalRocket(GridCell grid)
    {
        var rocketInstance = LeanPool.Spawn(_verticalRocket, grid.transform.position, Quaternion.identity);
        rocketInstance.SetGrid(grid);
        return rocketInstance;
    }

    public BlockBase SpawnHorizontalRocket(GridCell grid)
    {
        var rocketInstance = LeanPool.Spawn(_horizontalRocket, grid.transform.position, Quaternion.identity);
        rocketInstance.SetGrid(grid);
        return rocketInstance;
    }
}