using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Factory
{
    [CreateAssetMenu(fileName ="Dungeon", menuName ="new Dungeon", order = 53)]
    public class DungeonInfo : ScriptableObject
    {
        [SerializeField] private GameObject floorPathDownTile;
        [SerializeField] private GameObject exit;
        [SerializeField] private GameObject wallTileHorizontal;
        [SerializeField] private GameObject wallTileVertical;
        [SerializeField] private GameObject wallUpLeftCornerTile;
        [SerializeField] private GameObject wallUpRightCornerTile;
        [SerializeField] private GameObject wallDownLeftCornerTile;
        [SerializeField] private GameObject wallDownRightCornerTile;
        [SerializeField] private GameObject floorPathHorizontalTile;
        [SerializeField] private GameObject floorPathRightTile;
        [SerializeField] private GameObject floorPathLeftTile;
        [SerializeField] private GameObject floorPathVerticalTile;
        [SerializeField] private GameObject floorPathUpTile;
        [SerializeField] private GameObject floorTile;
        [SerializeField] private GameObject floorLeftTile;
        [SerializeField] private GameObject floorRightTile;
        [SerializeField] private GameObject floorTileUp;
        [SerializeField] private GameObject floorTileUpRightCorner;
        [SerializeField] private GameObject floorTileUpLeftCorner;
        [SerializeField] private GameObject floorTileDown;
        [SerializeField] private GameObject floorTileDownRightCorner;
        [SerializeField] private GameObject floorTileDownLeftCorner;
        [SerializeField] private GameObject closedDoorVertical;
        [SerializeField] private GameObject closedDoorHorizontal;

        public GameObject Exit { get => exit; set => exit = value; }
        public GameObject WallTileHorizontal { get => wallTileHorizontal; set => wallTileHorizontal = value; }
        public GameObject WallTileVertical { get => wallTileVertical; set => wallTileVertical = value; }
        public GameObject WallUpLeftCornerTile { get => wallUpLeftCornerTile; set => wallUpLeftCornerTile = value; }
        public GameObject WallUpRightCornerTile { get => wallUpRightCornerTile; set => wallUpRightCornerTile = value; }
        public GameObject WallDownLeftCornerTile { get => wallDownLeftCornerTile; set => wallDownLeftCornerTile = value; }
        public GameObject WallDownRightCornerTile { get => wallDownRightCornerTile; set => wallDownRightCornerTile = value; }

        public GameObject FloorPathHorizontalTile { get => floorPathHorizontalTile; set => floorPathHorizontalTile = value; }
        public GameObject FloorPathRightTile { get => floorPathRightTile; set => floorPathRightTile = value; }
        public GameObject FloorPathLeftTile { get => floorPathLeftTile; set => floorPathLeftTile = value; }
        public GameObject FloorPathVerticalTile { get => floorPathVerticalTile; set => floorPathVerticalTile = value; }
        public GameObject FloorPathUpTile { get => floorPathUpTile; set => floorPathUpTile = value; }
        public GameObject FloorPathDownTile { get => floorPathDownTile; set => floorPathDownTile = value; }

        public GameObject FloorTile { get => floorTile; set => floorTile = value; }
        public GameObject FloorLeftTile { get => floorLeftTile; set => floorLeftTile = value; }
        public GameObject FloorRightTile { get => floorRightTile; set => floorRightTile = value; }
        public GameObject FloorTileUp { get => floorTileUp; set => floorTileUp = value; }
        public GameObject FloorTileUpRightCorner { get => floorTileUpRightCorner; set => floorTileUpRightCorner = value; }
        public GameObject FloorTileUpLeftCorner { get => floorTileUpLeftCorner; set => floorTileUpLeftCorner = value; }
        public GameObject FloorTileDown { get => floorTileDown; set => floorTileDown = value; }
        public GameObject FloorTileDownRightCorner { get => floorTileDownRightCorner; set => floorTileDownRightCorner = value; }
        public GameObject FloorTileDownLeftCorner { get => floorTileDownLeftCorner; set => floorTileDownLeftCorner = value; }

        public GameObject ClosedDoorVertical { get => closedDoorVertical; set => closedDoorVertical = value; }
        public GameObject ClosedDoorHorizontal { get => closedDoorHorizontal; set => closedDoorHorizontal = value; }
    }
}
