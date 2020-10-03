using Assets.Scripts.Core.Data;
using Assets.Scripts.Dices;
using Assets.Scripts.Dungeon;
using Assets.Scripts.Dungeon.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    //public Size RoomsSize;
    //public Size Tourches;

    //public int CountOfRooms = 19;
    //public int CountOfTraps;

    //public int MapWidth = 70;
    //public int MapHeight = 40;

    //public int CountOfEnemies = DiceManager.RollUndSumFromString("3d12");

    public List<Room> Rooms;

    [HideInInspector] public List<GameObject> map;

    private List<Vector3> UpWallCoords;
    private List<Vector3> InnerRoomCoords;
    private List<Vector3> HRoomDoorsCoords;
    private List<Vector3> VRoomDoorsCoords;

    private List<GameObject> enemis;
    public LayerMask blockingLayer;

    [SerializeField] private DungeonSetupTemplate dungeonSetupTemplate;

    private DungeonGenerator dungeonGenerator;
    private DungeonFactory factory;
    public void SetUpLevel(int level)
    {
        Debug.Log("Start board");
        BoardSetUp();

        factory = DungeonFactoryManager.instance.DefaultDungeonFactory;

        if (level % 2 == 0)
            factory= DungeonFactoryManager.instance.BlueDungeonFactory;
        else
            factory = DungeonFactoryManager.instance.DefaultDungeonFactory;

        Generate(factory);
    }

    private void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        Rooms = new List<Room>();
        map = new List<GameObject>();
        enemis = new List<GameObject>();
        UpWallCoords = new List<Vector3>();
        InnerRoomCoords = new List<Vector3>();
        HRoomDoorsCoords = new List<Vector3>();
        VRoomDoorsCoords = new List<Vector3>();
    }

    private void DrawRoom(Room room)
    {
        foreach (var tile in room.Body)
        {
            GameObject obj = Instantiate(tile.Body, tile.Location, Quaternion.identity);
            AddGameObjectToMap(obj);
        }
    }

    private void Generate(DungeonFactory factory)
    {
        dungeonGenerator = new DungeonGenerator(factory, dungeonSetupTemplate);
        dungeonGenerator.OnPathTileGeneration               += DungeonGenerator_OnPathTileGeneration;
        dungeonGenerator.OnHorizontalPathTileGeneration     += DungeonGenerator_OnHorizontalPathTileGeneration;
        dungeonGenerator.OnVerticalPathTileGeneration       += DungeonGenerator_OnVerticalPathTileGeneration;
        dungeonGenerator.OnRoomGeneration                   += DungeonGenerator_OnRoomGeneration;
        dungeonGenerator.OnTrapsTileGeneration              += DungeonGenerator_OnTrapsTileGeneration;
        dungeonGenerator.Generate();

        ////PlaceTourches(factory);
        ////PlaceTraps(factory);

        ////PlaceEnemies(factory);
        ////PlaceDoors(factory);

        CorrectAngels(factory);
        CorrectWalls(factory);
        //CorrectAngels(factory);
        //PlaceFountains();

        var lastRoomCenter = Rooms.Last().GetCenter();
        Instantiate(factory.DungeonInfo.Exit, lastRoomCenter, Quaternion.identity);
    }

    private void DungeonGenerator_OnTrapsTileGeneration(object sender, DungeonTileGenerationEventArg e)
    {
        AddGameObjectToMap(Instantiate(e.Tile, e.TileCoords, Quaternion.identity));
    }

    private void DungeonGenerator_OnHorizontalPathTileGeneration(object sender, DungeonTileGenerationEventArg e)
    {
        ReplaceWith(e.Tile, e.TileCoords);

        PlaceWallPair(
            factory.DungeonInfo.WallTileHorizontal,
            new Vector3(e.TileRoomCoords.x, e.TileRoomCoords.y + 1, 0),
            new Vector3(e.TileRoomCoords.x, e.TileRoomCoords.y - 1, 0));
    }

    private void PlaceWallPair(GameObject tile, Vector3 firstWallPos, Vector3 secondWallPos)
    {
        PlaceWall(tile, firstWallPos);
        PlaceWall(tile, secondWallPos);
    }

    private void PlaceWall(GameObject tile, Vector3 wallPos)
    {
        if (map.FirstOrDefault(obj => obj.transform.position == wallPos) == null)
            AddGameObjectToMap(Instantiate(tile, wallPos, Quaternion.identity));
    }

    private void DungeonGenerator_OnVerticalPathTileGeneration(object sender, DungeonTileGenerationEventArg e)
    {
        ReplaceWith(e.Tile, e.TileCoords);

        PlaceWallPair(
            factory.DungeonInfo.WallTileVertical,
            new Vector3(e.TileRoomCoords.x + 1, e.TileRoomCoords.y, 0),
            new Vector3(e.TileRoomCoords.x - 1, e.TileRoomCoords.y, 0));
    }

    private void DungeonGenerator_OnRoomGeneration(object sender, DungeonRoomGenerationEventArg e)
    {
        Rooms.Add(e.Room);
        InnerRoomCoords.AddRange(e.Room.InnerCoords);
        DrawRoom(e.Room);
    }

    private void DungeonGenerator_OnPathTileGeneration(object sender, DungeonTileGenerationEventArg e)
    {
        ReplaceWith(e.Tile, e.TileCoords);
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

    private void AddGameObjectToMap(GameObject obj)
    {
        map.Add(obj);
        obj.transform.SetParent(boardHolder);
    }

    private void ReplaceWith(GameObject tile, Vector3 vector3)
    {
        var gameObj = map.FirstOrDefault(obj => obj.transform.position == vector3);
        if (gameObj != null)
        {
            map.Remove(gameObj);
            DestroyImmediate(gameObj);
        }
        AddGameObjectToMap(Instantiate(tile, vector3, Quaternion.identity));
    }

    private Vector3 GetRandVectorFrom(List<Vector3> vector3s)
    {
        var vec = vector3s[Random.Range(0, vector3s.ToList().Count)];
        vector3s.Remove(vec);
        return vec;
    }

 

    //private void PlaceEnemies(DungeonFactory factory)
    //{
    //    int count = CountOfEnemies;
    //    foreach (var vec in factory.MakeEnemisIn(count, InnerRoomCoords.ToArray()))
    //    {
    //        var enemy = Instantiate(EnemyPrefab, vec, Quaternion.identity);
    //        enemy.transform.SetParent(boardHolder);
    //        GameManager.Instance.Enemies.Add(enemy.GetComponent<Enemy>());
    //    }
    //}

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
            RaycastHit2D hit_left = Physics2D.Raycast(vec, Vector2.left, 1);
            RaycastHit2D hit_right = Physics2D.Raycast(vec, Vector2.right, 1);
            if (hit_left.transform != null && hit_right.transform != null)
            {
                var collider_left = hit_left.collider;
                var collider_right = hit_right.collider;
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
            RaycastHit2D hit_up = Physics2D.Raycast(vec, Vector2.up, 1);
            RaycastHit2D hit_down = Physics2D.Raycast(vec, Vector2.down, 1);
            if (hit_up.transform != null && hit_down.transform != null)
            {
                var collider_left = hit_up.collider;
                var collider_right = hit_down.collider;
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