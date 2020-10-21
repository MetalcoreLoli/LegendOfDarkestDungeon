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
            CreateHorizontalPath(factory, (int)currt.GetCenter().x, (int)prev.GetCenter().x, (int)currt.GetCenter().y);
            CreateVerticalPath(factory, (int)prev.GetCenter().y, (int)currt.GetCenter().y, (int)prev.GetCenter().x);

            int height = (prev.Height % 2 == 0) ? prev.Height - 1 : prev.Height;
            if (prev.GetCenter().y > currt.GetCenter().y)
            {
                PlaceVecticalDoorTiles(
                   prev.GetCenter().x,
                   prev.GetCenter().y - (height / 2)
                   );
            }
            else
            {
                PlaceVecticalDoorTiles(
                    prev.GetCenter().x,
                    prev.GetCenter().y + (height / 2)
                    );
            }
        }

        private void GenerateXPath(DungeonFactory factory, Room prev, Room currt)
        {
            CreateHorizontalPath(factory, (int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
            CreateVerticalPath(factory, (int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);

            int width = (prev.Width % 2 == 0) ? prev.Width - 1 : prev.Width;
            if (prev.GetCenter().x > currt.GetCenter().x)
            {
                PlaceHorizontalPathTiles(
                    prev.GetCenter().x - (width / 2),
                    prev.GetCenter().y);
            }
            else
            {
                PlaceHorizontalPathTiles(
                    prev.GetCenter().x + (width / 2),
                    prev.GetCenter().y);
            }
        }

        private void PlaceVecticalDoorTiles(float x, float y)
        {
            if (CanBeDoorPlace(new Vector3(x, y), Vector3.up, Vector3.down))
                PlaceDoorTile(VRoomDoorsCoords, factory.DungeonInfo.ClosedDoorVertical, x, y);
        }

        private void PlaceHorizontalPathTiles(float x, float y)
        {
            if (CanBeDoorPlace(new Vector3(x, y), Vector3.left, Vector3.right))
                PlaceDoorTile(HRoomDoorsCoords, factory.DungeonInfo.ClosedDoorHorizontal, x, y);
        }

        private bool CanBeDoorPlace(Vector3 doorDirection, Vector3 firstRayDirection, Vector3 secondRayDirection)
        {
            Nullable<Tile> fstTile = map.Where(t => t.Template.CompareTag("Wall")).ToList().Find(t => t.Location.Equals(doorDirection + firstRayDirection));
            Nullable<Tile> sndTile = map.Where(t => t.Template.CompareTag("Wall")).ToList().Find(t => t.Location.Equals(doorDirection + secondRayDirection));
            return fstTile.HasValue && sndTile.HasValue;
        }


        /// <summary>
        ///  Placing door in x, y coord und add it to door coords list
        /// </summary>
        /// <param name="coords">list of doors coords</param>
        /// <param name="tileTemplate"> template of door tile</param>
        /// <param name="x"> is room enter coord</param>
        /// <param name="y"> is room enter coord</param>
        private void PlaceDoorTile(
                    IList<Vector3> coords,
                    GameObject tileTemplate,
                    float x,
                    float y)
        {
            var coord = new Vector3(x, y);
            map.Add(new Tile(coord, tileTemplate));
            OnPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tileTemplate, coord));
            coords.Add(coord);
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

                OnHorizontalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile.Template, tile.Location, new Vector3(vec.x, y)));
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
                    new Vector3(x + 1, vec.y),
                    new Vector3(x - 1, vec.y)
                    );
                OnVerticalPathTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(tile.Template, tile.Location, new Vector3(x, vec.y)));
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
            var places = Rooms.Skip(1).SelectMany(r => r.InnerCoords).ToArray();
            int count = setup.CountOfTraps;

            foreach (var vec in Factory.MakeTrapsIn(count, places))
            {
                map.Add(new Tile(vec, Factory.DungeonInfo.Trap));
                OnTrapsTileGeneration?.Invoke(this, new DungeonTileGenerationEventArg(Factory.DungeonInfo.Trap, vec));
            }
        }

        private void Setup()
        {
            Rooms = new List<Room>();
            HRoomDoorsCoords = new List<Vector3>();
            VRoomDoorsCoords = new List<Vector3>();
            map = new List<Tile>();
        }

        public IEnumerable<Tile> Generate()
        {
            Setup();
            GenerateRooms();
            PlaceTourches();
            GenerateTunels();
            WallCorrection();
            PlaceTraps();
            map.Add(
                new Tile(
                    Rooms.Last().GetCenter(),
                    factory.DungeonInfo.Exit
                    )
                );
            return map;
        }

        /*
               up
               #.#
               #.#
           #####.#####
      left ........... right
           #####.#####
               #.#
               #.#
              down         
         */
        private void WallCorrection()
        {
            foreach (Tile tile in map.Where(t => t.CompareTemplateTag("Wall")))
            {

                if (tile.CompareTemplateTag("Wall"))
                {

                }
            }
        }
    }
}
