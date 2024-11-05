using System.Linq;
using _Dev.Scripts.Block;
using _Dev.Scripts.GridCell;
using _Dev.Scripts.Installers;
using UnityEngine;
using Lean.Pool;

namespace LevelEditor
{
    public static class LevelLoader
    {
        public static void LoadLevel(Level level,FactoryInstaller.GridFactory gridFactory, BoardManager boardManager)
        {
            LevelData[] data = JsonExtension.GetFromJson<LevelData>(level.data);
            foreach (LevelData dataItem in data)
            {
                LevelData ıtem = dataItem;
                Item dataPaletteItem = level.itemPalette.items.FirstOrDefault(fd => fd.type == ıtem.itemType);
                if (dataPaletteItem != null)
                {
                    Vector3 position = new Vector3(dataItem.row, dataItem.column, 0.0f);
                    Vector3 offset = Vector3.zero;
                    offset += new Vector3(-(float)(level.row - 1) / 2, (float)(level.column - 1) / 2, 0.0f);
                    BlockBase block=InstantiateItem(dataPaletteItem.prefab, position, offset).GetComponent<BlockBase>();
                    GridCell grid=InstantiateGrid(position, offset,gridFactory,dataItem,boardManager);
                    grid.SetBlock(block);
                    block.SetGrid(grid);
                    block.SetSpriteLayer();
                }
            }
        }

        private static GameObject InstantiateItem(GameObject prefab, Vector3 position, Vector3 offset)
        {
            GameObject instance =LeanPool.Spawn(prefab);
            float x = position.x + offset.x;
            float y = -position.y + offset.y;
            float z = offset.z;
            instance.transform.position = new Vector3(x, y, z);
            instance.transform.localScale = Vector3.one;

            return instance;
        }

        private static GridCell InstantiateGrid(Vector3 position, Vector3 offset,FactoryInstaller.GridFactory gridFactory,LevelData levelData,BoardManager boardManager)
        {
            GridCell instance = gridFactory.Create();
            float x = position.x + offset.x;
            float y = -position.y + offset.y;
            float z = offset.z;
            instance.transform.position = new Vector3(x, y, z);
            instance.transform.localScale = Vector3.one;
            instance.SetCoords(levelData.row,levelData.column);
            boardManager.GridCells[levelData.row, levelData.column] = instance;
            return instance;
        }
    }
    
}