using System.Collections.Generic;
using _Dev.Scripts.Goal;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level Editor/Level", order = 0)]
    public class Level : ScriptableObject
    {
        public ItemPalette itemPalette;
        public int row;
        public int column;
        public string data;
        public int moves;
        public List<Goal> goals;
    }
}