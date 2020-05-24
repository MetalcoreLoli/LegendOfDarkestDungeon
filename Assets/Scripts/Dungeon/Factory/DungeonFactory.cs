using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Factory
{
    public abstract class DungeonFactory : MonoBehaviour
    {
        [SerializeField]private DungeonInfo dungeonInfo;
        public DungeonInfo DungeonInfo { get => dungeonInfo; set => dungeonInfo = value; }
        public Vector2 Size { get; set; }
        public abstract Room MakeRoom(Vector2 roomSize, Vector3 location);
        public abstract Vector3[] MakeHorizontalPath(int xStart, int xEnd, int y);
        public abstract Vector3[] MakeVecticalPath(int yStart, int yEnd, int x);
        public abstract Vector3[] MakeTourchesAt(int count, Vector3[] places);
        public abstract Vector3[] MakeEnemisIn(int count, Vector3[] places);
        public abstract Vector3[] MakeTrapsIn(int count, Vector3[] places);
        public abstract Vector3[] MakeDoorAt(Vector3[] places);
        protected abstract GameObject MakeGameObjectAt(Vector3 place, GameObject prefab);
    }
}
