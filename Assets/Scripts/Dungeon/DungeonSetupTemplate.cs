using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    [CreateAssetMenu(fileName = "DungeonSetupTemplate", menuName = "Create Dungeon Setup Template", order = 57)]

    public class DungeonSetupTemplate : ScriptableObject, IDungeonGeneratorSetup
    {

        [SerializeField] private int roomMin;
        [SerializeField] private int roomMax;
        [SerializeField] private int tourchesMin;
        [SerializeField] private int tourchesMax;
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        [SerializeField] private int countOfRooms;
        [SerializeField] private int countOfTraps;
        [SerializeField] private int countOfEnemies;

        public int RoomMin { get => roomMin; set => roomMin = value; }
        public int RoomMax { get => roomMax; set => roomMax = value; }
        public int TourchesMin { get => tourchesMin; set => tourchesMin = value; }
        public int TourchesMax { get => tourchesMax; set => tourchesMax = value; }
        public int MapWidth { get => mapWidth; set => mapWidth = value; }
        public int MapHeight { get => mapHeight; set => mapHeight = value; }
        public int CountOfRooms { get => countOfRooms; set => countOfRooms = value; }
        public int CountOfTraps { get => countOfTraps; set => countOfTraps = value; }
        public int CountOfEnemies { get => countOfEnemies; set => countOfEnemies = value; }
    }
}
