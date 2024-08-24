using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelEditor
{
    [Serializable]
    public class Item
    {
        public BlockType type;
        public GameObject prefab;
        public Texture icon;
    }
}
