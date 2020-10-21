using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon.Factory
{
    public class BlueDungeonFactory : DungeonFactory
    {
        public override Vector3[] MakeDoorAt(Vector3[] places)
        {
            throw new NotImplementedException();
        }

        private Vector3 GetFrom(List<Vector3> vector3s)
        {
            var index = UnityEngine.Random.Range(0, vector3s.Count);
            var vec = vector3s[index];
            vector3s.Remove(vec);
            return vec;
        }

        public override Vector3[] MakeEnemisIn(int count, Vector3[] places)
        {
            List<Vector3> vecs = new List<Vector3>();
            while (count-- > 0)
                vecs.Add(GetFrom(places.ToList()));

            return vecs.ToArray();
        }

        public override Room MakeRoom(Vector2 roomSize, Vector3 location)
        {
            var room = new Room((int)roomSize.x, (int)roomSize.y, location);

            for (int x1 = 0; x1 < room.Width; x1++)
            {
                for (int y1 = 0; y1 < room.Height; y1++)
                {
                    int idx = x1 + room.Width * y1;
                    room.Body[idx].Template = DungeonInfo.FloorTile;

                    if (x1 == 0 && y1 == 0)
                        room.Body[idx].Template = DungeonInfo.WallDownRightCornerTile;

                    if (x1 == room.Width - 1 && y1 == 0)
                        room.Body[idx].Template = DungeonInfo.WallDownLeftCornerTile;

                    if (x1 == 0 && y1 == room.Height - 1)
                        room.Body[idx].Template = DungeonInfo.WallUpLeftCornerTile;

                    if (x1 == room.Width - 1 && y1 == room.Height - 1)
                        room.Body[idx].Template = DungeonInfo.WallUpRightCornerTile;

                    if ((x1 == room.Width - 1 && y1 > 0 && y1 < room.Height - 1))
                        room.Body[idx].Template = DungeonInfo.WallTileVertical;

                    if ((y1 == room.Height - 1 && x1 > 0 && x1 < room.Width - 1))
                    {
                        room.Body[idx].Template = DungeonInfo.WallTileHorizontal;
                        room.UpWallCoord.Add(new Vector3(x1, y1) + room.Location);
                    }

                    if (x1 > 0 && x1 < room.Width - 1 && y1 == 0)
                        room.Body[idx].Template = DungeonInfo.WallTileHorizontal;

                    if (y1 > 0 && y1 < room.Height - 1 && x1 == 0)
                        room.Body[idx].Template = DungeonInfo.WallTileVertical;
                }
            }
            for (int x1 = 1; x1 < room.Width - 1; x1++)
            {
                for (int y1 = 1; y1 < room.Height - 1; y1++)
                {
                    int idx = x1 + room.Width * y1;
                    room.Body[idx].Template = DungeonInfo.FloorTile;

                    if (x1 == 1 && y1 == 1)
                        room.Body[idx].Template = DungeonInfo.FloorTileDownLeftCorner;

                    if (x1 == room.Width - 2 && y1 == 1)
                        room.Body[idx].Template = DungeonInfo.FloorTileDownRightCorner;

                    if (x1 == 1 && y1 == room.Height - 2)
                        room.Body[idx].Template = DungeonInfo.FloorTileUpLeftCorner;

                    if (x1 == room.Width - 2 && y1 == room.Height - 2)
                        room.Body[idx].Template = DungeonInfo.FloorTileUpRightCorner;

                    if ((x1 == room.Width - 2 && y1 > 1 && y1 < room.Height - 2))
                        room.Body[idx].Template = DungeonInfo.FloorRightTile;

                    if ((y1 == room.Height - 2 && x1 > 1 && x1 < room.Width - 2))
                        room.Body[idx].Template = DungeonInfo.FloorTileUp;

                    if (x1 > 1 && x1 < room.Width - 2 && y1 == 1)
                        room.Body[idx].Template = DungeonInfo.FloorTileDown;

                    if (y1 > 1 && y1 < room.Height - 2 && x1 == 1)
                        room.Body[idx].Template = DungeonInfo.FloorLeftTile;
                }
            }

            return room;
        }

        public override Vector3[] MakeTourchesAt(int count, Vector3[] places)
        {
            List<Vector3> vecs = new List<Vector3>();
            while (count-- > 0)
                vecs.Add(GetFrom(places.ToList()));

            return vecs.ToArray();
        }

        public override Vector3[] MakeTrapsIn(int count, Vector3[] places)
        {
            List<Vector3> vecs = new List<Vector3>();
            while (count-- > 0)
                vecs.Add(GetFrom(places.ToList()));

            return vecs.ToArray();
        }

        public override Vector3[] MakeHorizontalPath(int xStart, int xEnd, int y)
        {
            int min = Math.Min(xStart, xEnd);
            int max = Math.Max(xStart, xEnd);
            List<Vector3> vec = new List<Vector3>();
            for (int x = min; x < max + 1; x++)
                vec.Add(new Vector3(x, y));

            return vec.ToArray();
        }

        public override Vector3[] MakeVecticalPath(int yStart, int yEnd, int x)
        {
            int min = Math.Min(yStart, yEnd);
            int max = Math.Max(yStart, yEnd);

            List<Vector3> vec = new List<Vector3>();

            for (int y = min; y < max + 1; y++)
                vec.Add(new Vector3(x, y));

            return vec.ToArray();
        }

        protected override GameObject MakeGameObjectAt(Vector3 place, GameObject prefab)
        {
            return Instantiate(prefab, place, Quaternion.identity);
        }
    }
}