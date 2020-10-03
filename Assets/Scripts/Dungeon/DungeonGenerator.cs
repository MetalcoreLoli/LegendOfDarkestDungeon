using Assets.Scripts.Dungeon.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dungeon
{
    public class DungeonGenerator
    {
        #region Dependances
        private readonly DungeonFactory factory;
        private readonly IDungeonGeneratorSetup setup;
        #endregion

        private List<Room> rooms;
        private List<Vector3> hRoomDoorsCoords;
        private List<Vector3> vRoomDoorsCoords;

        public List<Room> Rooms { get => rooms; private set => rooms = value; }
        public List<Vector3> HRoomDoorsCoords { get => hRoomDoorsCoords; private set => hRoomDoorsCoords = value; }
        public List<Vector3> VRoomDoorsCoords { get => vRoomDoorsCoords; private set => vRoomDoorsCoords = value; }

        public event EventHandler<DungeonTileGenerationEventArg> OnPathTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnTourchesTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnTrapsTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnHorizontalPathTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnVerticalPathTileGeneration;
        public event EventHandler<DungeonRoomGenerationEventArg> OnRoomGeneration;

        public DungeonGenerator(DungeonFactory factory, IDungeonGeneratorSetup setup)
        {
            this.factory = factory;
            this.setup = setup;
            Rooms = new List<Room>();
            HRoomDoorsCoords = new List<Vector3>();
            VRoomDoorsCoords = new List<Vector3>();
        }

        private Room GenerateRoom()
        {
            int width = 0, height = 0;
            int x = 0, y = 0;

            GenerateRoomSize(ref width, ref height);
            GenerateRoomCoords(ref x, ref y, width, height);

            Room room = factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            while (Rooms.FirstOrDefault(r => r.IsIntersectedWith(room)) != null)
            {
                GenerateRoomSize(ref width, ref height);
                GenerateRoomCoords(ref x, ref y, width, height);

                room = factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            }
            OnRoomGeneration?.Invoke(this, new DungeonRoomGenerationEventArg(room));
            return room;
        }

        private void GenerateRoomSize(ref int width, ref int height)
        {
            GenerateRandomValue(ref width, setup.RoomMin, setup.RoomMax);
            GenerateRandomValue(ref height, setup.RoomMin, setup.RoomMax);
        }

        private void GenerateRoomCoords(ref int x, ref int y, int width, int height)
        {
            GenerateRandomValue(ref x, 0, setup.MapWidth - width);
            GenerateRandomValue(ref y, 0, setup.MapHeight - height);
        }

        private void GenerateRandomValue(ref int value, int min, int max)
        {
            value = UnityEngine.Random.Range(min, max);
        }

        public IEnumerable<Room> GenerateRooms()
        {
            int count = setup.CountOfRooms;
            while (count-- > 0)
            {
                Room room = GenerateRoom();
                Rooms.Add(room);
            }
            return Rooms;
        }

        private void GenerateTunels()
        {
            for (int i = 1; i < setup.CountOfRooms; i++)
            {
                var prev = Rooms[i - 1];
                var currt = Rooms[i];

                var pathType = UnityEngine.Random.Range(0, 2);
                if (pathType == 0)
                {
                    GenerateXPath(factory, prev, currt);
                }
                else
                {
                    GenerateYPath(factory, prev, currt);
                }
            }
        }

        private void GenerateYPath(DungeonFactory factory, Room prev, Room currt)
        {
            int height = (prev.Height % 2 == 0) ? prev.Height - 1 : prev.Height;
            if (prev.GetCenter().y > currt.GetCenter().y)
            {
                PlaceYPathTiles(
                   factory.DungeonInfo.WallUpRightCornerTile,
                   factory.DungeonInfo.WallUpLeftCornerTile,
                   prev.GetCenter().x,
                   prev.GetCenter().y - (height / 2)
                   );
            }
            else
            {
                PlaceYPathTiles(
                    factory.DungeonInfo.WallDownLeftCornerTile,
                    factory.DungeonInfo.WallDownRightCornerTile,
                    prev.GetCenter().x,
                    prev.GetCenter().y + (height / 2)
                    );
            }
            CreateHorizontalPath(factory, (int)currt.GetCenter().x, (int)prev.GetCenter().x, (int)currt.GetCenter().y);
            CreateVerticalPath(factory, (int)prev.GetCenter().y, (int)currt.GetCenter().y, (int)prev.GetCenter().x);
        }

        private void GenerateXPath(DungeonFactory factory, Room prev, Room currt)
        {
            int width = (prev.Width % 2 == 0) ? prev.Width - 1 : prev.Width;
            if (prev.GetCenter().x > currt.GetCenter().x)
            {
                PlaceXPathTiles(
                    factory.DungeonInfo.WallDownLeftCornerTile,
                    factory.DungeonInfo.WallUpRightCornerTile,
                    prev.GetCenter().x - (width / 2),
                    prev.GetCenter().y);
            }
            else
            {
                PlaceXPathTiles(
                    factory.DungeonInfo.WallDownRightCornerTile,
                    factory.DungeonInfo.WallUpLeftCornerTile,
                    prev.GetCenter().x + (width / 2),
                    prev.GetCenter().y);
            }
            CreateHorizontalPath(factory, (int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
            CreateVerticalPath(factory, (int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);
        }

        private void PlaceXPathTiles(GameObject upEnterTile, GameObject downEnterTile, float x, float y)
        {
            var upEnter = new Vector3(x, y + 1);
            var downEnter = new Vector3(x, y - 1);
            PlacePathTiles(HRoomDoorsCoords, upEnterTile, upEnter, x, y);
            PlacePathTiles(HRoomDoorsCoords, downEnterTile, downEnter, x, y);
        }

        private void PlaceYPathTiles(GameObject upEnterTile, GameObject downEnterTile, float x, float y)
        {
            var upEnter = new Vector3(x + 1, y);
            var downEnter = new Vector3(x - 1, y);
            PlacePathTiles(VRoomDoorsCoords, upEnterTile, upEnter, x, y);
            PlacePathTiles(VRoomDoorsCoords, downEnterTile, downEnter, x, y);
        }

        private void PlacePathTiles(
                    IList<Vector3> coords,
                    GameObject tile,
                    Vector3 coord,
                    float x,
                    float y)
        {
            OnPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile, coord));
            coords.Add(new Vector3(x, y));
        }


        private void CreateHorizontalPath(DungeonFactory factory, int xStart, int xEnd, int y)
        {
            foreach (var vec in factory.MakeHorizontalPath(xStart, xEnd, y))
            {
                OnHorizontalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(factory.DungeonInfo.FloorTile, vec, new Vector3(vec.x, y)));
            }
        }

        private void CreateVerticalPath(DungeonFactory factory, int yStart, int yEnd, int x)
        {
            foreach (var vec in factory.MakeVecticalPath(yStart, yEnd, x))
            {
                OnVerticalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(factory.DungeonInfo.FloorTile, vec, new Vector3(x, vec.y)));
            }
        }
        private void PlaceTourches()
        {
            //UpWallCoords.AddRange(Rooms.SelectMany(room => room.UpWallCoord));
            //int count = Tourches.Max;
            //foreach (var vec in factory.MakeTrapsIn(count, UpWallCoords.ToArray()))
            //    AddGameObjectToMap(Instantiate(TourchTile, vec, Quaternion.identity));
        }

        private void PlaceTraps()
        {
            //InnerRoomCoords.AddRange(Rooms.Skip(1).SelectMany(room => room.InnerCoords));
            var places = Rooms.Skip(1).SelectMany(r => r.InnerCoords).ToArray();
            int count = setup.CountOfTraps;
            foreach (var vec in factory.MakeTrapsIn(count, places))
            {
                OnTrapsTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(factory.DungeonInfo.Trap, vec));
            }
        }
        public IEnumerable<Room> Generate ()
        {
            GenerateRooms();
            PlaceTourches();
            GenerateTunels();
            PlaceTraps();
            return Rooms;
        }
    }
}