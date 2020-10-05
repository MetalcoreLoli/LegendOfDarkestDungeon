using Assets.Scripts.Dungeon.Factory;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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

        private List<Tile> map;

        public List<Room> Rooms { get => rooms; private set => rooms = value; }
        public List<Vector3> HRoomDoorsCoords { get => hRoomDoorsCoords; private set => hRoomDoorsCoords = value; }
        public List<Vector3> VRoomDoorsCoords { get => vRoomDoorsCoords; private set => vRoomDoorsCoords = value; }

        public DungeonFactory Factory => factory;

        public event EventHandler<DungeonTileGenerationEventArg> OnPathTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnTourchesTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnTrapsTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnHorizontalPathTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnVerticalPathTileGeneration;
        public event EventHandler<DungeonTileGenerationEventArg> OnDoorTileGeneration;
        public event EventHandler<DungeonRoomGenerationEventArg> OnRoomGeneration;

        public DungeonGenerator(DungeonFactory factory, IDungeonGeneratorSetup setup)
        {
            this.factory = factory;
            this.setup = setup;
            Setup();
        }

        private Room GenerateRoom()
        {
            int width = 0, height = 0;
            int x = 0, y = 0;

            GenerateRoomSize(ref width, ref height);
            GenerateRoomCoords(ref x, ref y, width, height);

            Room room = Factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            while (Rooms.FirstOrDefault(r => r.IsIntersectedWith(room)) != null)
            {
                GenerateRoomSize(ref width, ref height);
                GenerateRoomCoords(ref x, ref y, width, height);

                room = Factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            }
            map.AddRange(room.Body);
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
                    GenerateXPath(Factory, prev, currt);
                }
                else
                {
                    GenerateYPath(Factory, prev, currt);
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

            map.Add(new Tile(coord, tile));
            OnPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile, coord));
            coords.Add(new Vector3(x, y));
        }

        private void PlaceWallPair(GameObject tile, Vector3 firstWallPos, Vector3 secondWallPos)
        {
            PlaceWall(tile, firstWallPos);
            PlaceWall(tile, secondWallPos);
        }

        private void PlaceWall(GameObject tileTemplate, Vector3 wallPos)
        {
            Tile tile = new Tile(wallPos, tileTemplate);
            bool isOnMap = map.Contains(tile);
            if (!isOnMap) 
                map.Add(tile);
        }

        private void CreateHorizontalPath(DungeonFactory factory, int xStart, int xEnd, int y)
        {
            foreach (var vec in factory.MakeHorizontalPath(xStart, xEnd, y))
            {

                var tile = new Tile(vec, factory.DungeonInfo.FloorTile);
                ReplaceTileOnMapOrAdd(tile);
                PlaceWallPair(
                    factory.DungeonInfo.WallTileHorizontal,
                    new Vector3(vec.x, y + 1),
                    new Vector3(vec.x, y - 1)
                    );
                
                OnHorizontalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile.Body, tile.Location, new Vector3(vec.x, y)));
            }
        }

        private void CreateVerticalPath(DungeonFactory factory, int yStart, int yEnd, int x)
        {
            foreach (var vec in factory.MakeVecticalPath(yStart, yEnd, x))
            {
                var tile = new Tile(vec, factory.DungeonInfo.FloorTile);
                ReplaceTileOnMapOrAdd(tile);
                PlaceWallPair(
                    factory.DungeonInfo.WallTileVertical,
                    new Vector3(vec.x + 1, vec.y),
                    new Vector3(vec.x - 1, vec.y)
                    );
                OnVerticalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile.Body, tile.Location, new Vector3(x, vec.y)));
            }
        }

        private void ReplaceTileOnMapOrAdd(Tile tile)
        {
            Tile? t = map.FirstOrDefault(tt => tt.Location.Equals(tile.Location));
            if (t.HasValue)
            {
                map.Remove((Tile)t);
                map.Add(tile);
            }
            else
                map.Add(tile);
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
            foreach (var vec in Factory.MakeTrapsIn(count, places))
            {
                map.Add(new Tile(vec, Factory.DungeonInfo.Trap));
                OnTrapsTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(Factory.DungeonInfo.Trap, vec));
            }
        }

        private Vector3 GetRandVectorFrom(List<Vector3> vector3s)
        {
            var vec = vector3s[UnityEngine.Random.Range(0, vector3s.ToList().Count)];
            vector3s.Remove(vec);
            return vec;
        }
        private void PlaceDoors (IEnumerable<Vector3> coorsd, GameObject tile, Vector3 firstRayDirection, Vector3 secondRayDirection)
        {
            UInt16 count = (UInt16)coorsd.Count();

            while (count --> 0)
            {
                var vec = GetRandVectorFrom(coorsd.ToList());
                RaycastHit2D firstHit   = Physics2D.Raycast(vec, firstRayDirection, 1);
                RaycastHit2D secondHit  = Physics2D.Raycast(vec, secondRayDirection, 1);

                if (firstHit.transform != null && secondHit.transform != null)
                {
                    var fsthit = firstHit.transform;
                    var sndhit = secondHit.transform;
                    if (fsthit.gameObject.CompareTag("Wall") 
                        && sndhit.gameObject.CompareTag("Wall"))
                    {
                        map.Add(new Tile(vec, tile));
                        OnDoorTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile, vec));
                    }
                    else continue;
                }
                else continue;
            }
        }

        public void PlaceHorizontalDoors()
        {
            PlaceDoors(HRoomDoorsCoords, factory.DungeonInfo.ClosedDoorHorizontal, Vector2.left, Vector2.right);
        }

        public void PlaceVerticalDoors()
        {
            PlaceDoors(VRoomDoorsCoords, factory.DungeonInfo.ClosedDoorVertical, Vector2.up, Vector2.down);

        }

        private void Setup()
        {
            Rooms            = new List<Room>();
            HRoomDoorsCoords = new List<Vector3>();
            VRoomDoorsCoords = new List<Vector3>();
            map              = new List<Tile>();
        }
        public IEnumerable<Tile> Generate ()
        {
            Setup();
            GenerateRooms();
            PlaceTourches();
            GenerateTunels();
            PlaceTraps();
            return map;
        }
    }
}