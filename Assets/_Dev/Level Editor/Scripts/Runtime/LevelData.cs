using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] public int row;
        [SerializeField] public int column;
        [SerializeField] public BlockType itemType;
    }
}