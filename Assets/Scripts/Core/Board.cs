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

    [HideInInspector] public List<Tile> tiles;
    [HideInInspector] public List<GameObject> map;

    private List<GameObject> enemis;
    public LayerMask blockingLayer;

    [SerializeField] private DungeonSetupTemplate dungeonSetupTemplate;

    public DungeonGenerator dungeonGenerator;
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
        
        dungeonGenerator = new DungeonGenerator(factory, dungeonSetupTemplate);

        DrawDungeon(dungeonGenerator);
    }

    private void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        map = new List<GameObject>();
        tiles = new List<Tile>();
        enemis = new List<GameObject>();
    }

    private void DrawDungeon(DungeonGenerator generator)
    {
        generator.OnTrapsTileGeneration              += (s, e) => SpawnObject(e.TileCoords, e.Tile);
        generator.OnDoorTileGeneration               += (s, e) => SpawnObject(e.TileCoords, e.Tile);
        tiles = generator.Generate().ToList();

        foreach (var tile in tiles)
        {
            var gb = Instantiate(tile.Body, tile.Location, Quaternion.identity);
            map.Add(gb);
        }
        //CorrectWalls(generator.Factory);
        //CorrectAngels(generator.Factory); 
        //CorrectAngels(generator.Factory);
        //PlaceFountains();

        //Vector3 lastRoomCenter = generator.Rooms.Last().GetCenter();
        //SpawnObject(lastRoomCenter, factory.DungeonInfo.Exit);
    }



    private void CorrectWalls(DungeonFactory factory)
    {
        var walls = map.Where(gb => gb.CompareTag("Wall"));

        var toReplace = new List<(GameObject template, Vector3 position)>();

        foreach (GameObject wall in walls)
        {
            var wallScript = wall.GetComponent<Wall>();


            if (wallScript.HitLeftRightWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallTileHorizontal, wall.transform.position));
            else if (wallScript.HitUpDownWithTag("Wall"))
                toReplace.Add((factory.DungeonInfo.WallTileVertical, wall.transform.position));
            else
                continue;
        }

        foreach (var wall in toReplace)
            ReplaceWith(wall.template, wall.position);
    }

    private void CorrectAngels(DungeonFactory factory)
    {
        throw new NotImplementedException("TODO: Fix angels correct");
        {
            //var walls = map.Where(gb => gb.CompareTag("Wall"));
            var walls = from gb in map where gb.CompareTag("Wall") select gb;


            var toReplace = new List<(GameObject tile, Vector3 position)>();

            foreach (var (wall, wallScript) in from GameObject wall in walls
                                               let wallScript = wall.GetComponent<Wall>()
                                               select (wall, wallScript))
            {
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
                ReplaceWith(wall.tile, wall.position);
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
        if (gameObj != null)
        {
            map.Remove(gameObj);
            Destroy(gameObj);
        }
        SpawnObject(vector3, tile);
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
        //foreach (var item in map)
        //    dict.Add(item, 0);

        return dict;
    }

    public void LoadData(Dictionary<GameObject, int> data)
    {
        foreach (var item in data.Keys)
        {
           // AddGameObjectToMap(Instantiate(item, item.transform.position, Quaternion.identity));
        }
    }
}