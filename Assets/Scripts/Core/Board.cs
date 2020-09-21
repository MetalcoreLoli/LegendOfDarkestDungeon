using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.Scripts.Dices;
using Assets.Scripts.Dungeon.Factory;
using Assets.Scripts.Core.Data;

[Serializable]
public struct Size
{
    public int Min;
    public int Max;

    public Size(int min, int max)
    {
        Min = min;
        Max = max;
    }

}
public class Board : MonoBehaviour, IData<GameObject, int>
{
    private Transform boardHolder;

    public GameObject TourchTile;
    public GameObject BloodFountain;

    public GameObject[] Traps;

    public GameObject EnemyPrefab;

    public Size RoomsSize;
    public Size Tourches;

    public int CountOfRooms = 19;
    public int CountOfTraps;

    public int MapWidth = 70;
    public int MapHeight = 40;

    public int CountOfEnemies = DiceManager.RollUndSumFromString("3d12");

    public List<Room> Rooms;

    [HideInInspector] public List<GameObject> map;

    private List<Vector3> UpWallCoords;
    private List<Vector3> InnerRoomCoords;
    private List<Vector3> HRoomDoorsCoords;
    private List<Vector3> VRoomDoorsCoords;

    private List<GameObject> enemis;
    public LayerMask blockingLayer;

    public void SetUpLevel(int level) 
    {
        Debug.Log("Start board");
        BoardSetUp();
        if (level % 6 == 0)
            Generate(CountOfRooms, DungeonFactoryManager.instance.BlueDungeonFactory);
        else
            Generate(CountOfRooms, DungeonFactoryManager.instance.DefaultDungeonFactory);

    }

    private void BoardSetUp()
    {
        boardHolder         = new GameObject("Board").transform;
        Rooms               = new List<Room>();
        map                 = new List<GameObject>();
        enemis              = new List<GameObject>();
        UpWallCoords        = new List<Vector3>();
        InnerRoomCoords     = new List<Vector3>();
        HRoomDoorsCoords    = new List<Vector3>();
        VRoomDoorsCoords    = new List<Vector3>();
    
    }

    private void DrawRoom(Room room) 
    {
        foreach (var tile in room.Body)
        {
            GameObject obj = Instantiate(tile.Body, tile.Location, Quaternion.identity);
            AddGameObjectToMap(obj);
        }
    }

    private void Generate(int roomsCount, DungeonFactory factory)
    {
        GenerateRooms(roomsCount, factory);

        foreach (var room in Rooms)
            DrawRoom(room);

        GenerateTunels(Rooms.Count, factory);
        
        PlaceTourches(factory);
        PlaceTraps(factory);

        PlaceEnemies(factory);
        PlaceDoors(factory);


        CorrectAngels(factory);
        CorrectWalls(factory);
        //CorrectAngels(factory);
        //PlaceFountains();

        var lastRoomCenter = Rooms.Last().GetCenter();
        Instantiate(factory.DungeonInfo.Exit, lastRoomCenter, Quaternion.identity);
    }

    private void GenerateTunels(int roomsCount, DungeonFactory factory)
    {
        for (int i = 1; i < roomsCount; i++)
        {
            var prev = Rooms[i - 1];
            var currt = Rooms[i];

            var pathType = Random.Range(0, 2);
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
        PlacePathTiles(VRoomDoorsCoords, upEnterTile, downEnterTile, upEnter, downEnter, x, y);
    }

    private void PlaceYPathTiles(GameObject upEnterTile, GameObject downEnterTile, float x, float y)
    {
        var upEnter = new Vector3(x + 1, y);
        var downEnter = new Vector3(x - 1, y);
        PlacePathTiles(HRoomDoorsCoords, upEnterTile, downEnterTile, upEnter, downEnter, x, y);
    }

    private void PlacePathTiles(
        IList<Vector3> coords, 
        GameObject upEnterTile, 
        GameObject downEnterTile,
        Vector3 upEnter,
        Vector3 downEnter,
        float x,
        float y)
    {
        ReplaceWith(upEnterTile, upEnter);
        ReplaceWith(downEnterTile, downEnter);
        coords.Add(new Vector3(x, y));
    }

    private void GenerateRoomSize(ref int width, ref int height)
    {
        GenerateRandomValue(ref width, RoomsSize.Min, RoomsSize.Max);
        GenerateRandomValue(ref height, RoomsSize.Min, RoomsSize.Max);
    }

    private void GenerateRoomCoords(ref int x, ref int y, int width, int height)
    {
        GenerateRandomValue(ref x, 0, MapWidth - width);
        GenerateRandomValue(ref y, 0, MapHeight - height);
    }

    private void GenerateRandomValue(ref int value, int min, int max)
    {
        value = Random.Range(min, max);
    }

    private int GenerateRooms(int roomsCount, DungeonFactory factory)
    {
        while (roomsCount-- > 0)
        {
            // Debug.Log($"Room  with number {roomsCount} is generating");
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

            Rooms.Add(room);
        }

        return roomsCount;
    }

    private void CorrectWalls(DungeonFactory factory)
    {
        var walls = map.Where(gb => gb.CompareTag("Wall"));

        var toReplace = new List<(GameObject, Vector3)>();

        foreach (GameObject wall in walls)
        {
            var wallScript = wall.GetComponent<Wall>();

            //if (wallScript.HitDownLeftRightWithTag("Wall"))
            //    toReplace.Add((factory.DungeonInfo.WallDownLeftRightTile, wall.transform.position));
            if (wallScript.HitLeftRightWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallTileHorizontal, wall.transform.position));
            else if (wallScript.HitUpDownWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallTileVertical, wall.transform.position));
            //else if (wallScript.HitLeftWithTag("Wall") && !wallScript.HitRightWithTag("Wall") && !wallScript.IsAngelHere())
            //    toReplace.Add((factory.DungeonInfo.WallTileHorizontal, wall.transform.position));
            else
                continue;
        }

        foreach (var wall in toReplace)
            ReplaceWith(wall.Item1, wall.Item2);
    }

    private void CorrectAngels(DungeonFactory factory)
    {
        var walls = map.Where(gb => gb.CompareTag("Wall"));

        var toReplace = new List<(GameObject, Vector3)>();

        foreach (GameObject wall in walls)
        {
            var wallScript = wall.GetComponent<Wall>();

            if (wallScript.HitDownLeftWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallUpRightCornerTile, wall.transform.position));
            else if (wallScript.HitDownRightWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallUpLeftCornerTile, wall.transform.position));
            else if (wallScript.HitUpLeftWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallDownLeftCornerTile, wall.transform.position));
            else if (wallScript.HitUpRightWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallDownRightCornerTile, wall.transform.position));
            else
                continue;
        }

        foreach (var wall in toReplace)
            ReplaceWith(wall.Item1, wall.Item2);
    }

    private void CreateHorizontalPath(DungeonFactory factory, int xStart, int xEnd, int y)
    {
        foreach (var vec in factory.MakeHorizontalPath(xStart, xEnd, y))
        {
            ReplaceWith(factory.DungeonInfo.FloorTile, vec);

            var upWallVector    = new Vector3(vec.x, y + 1, 0);
            var downWallVector  = new Vector3(vec.x, y - 1, 0);
            
            if (map.FirstOrDefault(obj => obj.transform.position == upWallVector) == null)
                AddGameObjectToMap(Instantiate(factory.DungeonInfo.WallTileHorizontal, upWallVector, Quaternion.identity));

            if (map.FirstOrDefault(obj => obj.transform.position == downWallVector) == null)
                AddGameObjectToMap(Instantiate(factory.DungeonInfo.WallTileHorizontal, downWallVector, Quaternion.identity));
        }
    }

    private void CreateVerticalPath(DungeonFactory factory, int yStart, int yEnd, int x)
    {
        foreach (var vec in factory.MakeVecticalPath(yStart, yEnd, x))
        { 
            ReplaceWith(factory.DungeonInfo.FloorTile, vec);
            var leftWall    = new Vector3(x + 1, vec.y, 0);
            var rightWall   = new Vector3(x - 1, vec.y, 0);

            if (map.FirstOrDefault(obj => obj.transform.position == leftWall) == null)
                AddGameObjectToMap(Instantiate(factory.DungeonInfo.WallTileVertical, leftWall, Quaternion.identity));

            if (map.FirstOrDefault(obj => obj.transform.position == rightWall) == null)
                AddGameObjectToMap(Instantiate(factory.DungeonInfo.WallTileVertical, rightWall, Quaternion.identity));
        }
    }

    private void AddGameObjectToMap(GameObject obj)
    {
        map.Add(obj);
        obj.transform.SetParent(boardHolder);
    }

    private void ReplaceWith(GameObject tile, Vector3 vector3)
    {
        var gameObj = map.FirstOrDefault(obj => obj.transform.position == vector3);
        map.Remove(gameObj);
        DestroyImmediate(gameObj);
        AddGameObjectToMap(Instantiate(tile, vector3, Quaternion.identity));
    }

    private Vector3 GetRandVectorFrom(List<Vector3> vector3s)
    {
        var vec = vector3s[Random.Range(0, vector3s.ToList().Count)];
        vector3s.Remove(vec);
        return vec;
    }

    private void PlaceTourches(DungeonFactory factory)
    {
        UpWallCoords.AddRange(Rooms.SelectMany(room => room.UpWallCoord));
        int count = Tourches.Max;
        foreach (var vec in factory.MakeTrapsIn(count, UpWallCoords.ToArray()))
            AddGameObjectToMap(Instantiate(TourchTile, vec, Quaternion.identity));
    }

    private void PlaceTraps(DungeonFactory factory)
    {
        InnerRoomCoords.AddRange(Rooms.Skip(1).SelectMany(room => room.InnerCoords));
        int count = CountOfTraps;
        foreach (var vec in factory.MakeTrapsIn(count, UpWallCoords.ToArray()))
        {
            AddGameObjectToMap(Instantiate(Traps[0], GetRandVectorFrom(InnerRoomCoords), Quaternion.identity));
        }
    }

    private void PlaceEnemies(DungeonFactory factory)
    {
        int count = CountOfEnemies;
        foreach (var vec in factory.MakeEnemisIn(count, InnerRoomCoords.ToArray()))
        {
            var enemy = Instantiate(EnemyPrefab, vec, Quaternion.identity);
            enemy.transform.SetParent(boardHolder);
            GameManager._instance.Enemies.Add(enemy.GetComponent<Enemy>());
        }
      
    }

    private void PlaceFountains()
    {
        AddGameObjectToMap(Instantiate(BloodFountain, GetRandVectorFrom(InnerRoomCoords), Quaternion.identity));    
    }

    private void PlaceDoors(DungeonFactory factory)
    {
        int h_count = HRoomDoorsCoords.Count;
        int v_count = VRoomDoorsCoords.Count;
        while (h_count-- > 0)
        {
            var vec = GetRandVectorFrom(HRoomDoorsCoords);
            RaycastHit2D hit_left   = Physics2D.Raycast(vec, Vector2.left,     1);
            RaycastHit2D hit_right  = Physics2D.Raycast(vec, Vector2.right,    1);
            if (hit_left.transform != null && hit_right.transform != null)
            {
                var collider_left   = hit_left.collider;
                var collider_right  = hit_right.collider;
                if (collider_right.gameObject.tag == "Wall" && collider_left.gameObject.tag == "Wall")
                {
                    var gb = map.FirstOrDefault(g => g.transform.position == vec && g.tag == "Wall");
                    if (gb == null)
                        AddGameObjectToMap(Instantiate(factory.DungeonInfo.ClosedDoorVertical, vec, Quaternion.identity));
                }
                else
                    continue;
            }
            else
                continue;
        }

        while (v_count-- > 0)
        {
            var vec = GetRandVectorFrom(VRoomDoorsCoords);
            RaycastHit2D hit_up     = Physics2D.Raycast(vec, Vector2.up,    1);
            RaycastHit2D hit_down   = Physics2D.Raycast(vec, Vector2.down,  1);
            if (hit_up.transform != null && hit_down.transform != null)
            {
                var collider_left   = hit_up.collider;
                var collider_right  = hit_down.collider;
                if (collider_right.gameObject.tag == "Wall" && collider_left.gameObject.tag == "Wall")
                    AddGameObjectToMap(Instantiate(factory.DungeonInfo.ClosedDoorHorizontal, vec, Quaternion.identity));

            }
        }
    }

    public void SpawnObject(Vector3 pos, GameObject obj)
    {
        AddGameObjectToMap(Instantiate(obj, pos, Quaternion.identity));
    }
    
    private void OnDestroy()
    {
        Debug.Log("Was Destroied");
        //Destroy(boardHolder);
        //foreach (var item in map)
        //{
        //    Destroy(item);
        //}
    }

    public Dictionary<GameObject, int> GetData()
    {
        var dict = new Dictionary<GameObject, int>();
        foreach (var item in map)
            dict.Add(item, 0);
     
        return dict;
    }

    public void LoadData(Dictionary<GameObject, int> data)
    {
        foreach (var item in data.Keys)
        {
            AddGameObjectToMap(Instantiate(item, item.transform.position, Quaternion.identity));
        }
    }
}