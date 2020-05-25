using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.Scripts.Dices;
using Assets.Scripts.Dungeon.Factory;

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
public class BoardManager : MonoBehaviour
{

    private Transform boardHolder;

    public GameObject Exit;
    public GameObject WallTileHorizontal;
    public GameObject WallTileVertical;
    public GameObject WallUpLeftCornerTile;
    public GameObject WallUpRightCornerTile;
    public GameObject WallDownLeftCornerTile;
    public GameObject WallDownRightCornerTile;

    public GameObject FloorPathHorizontalTile;
    public GameObject FloorPathRightTile;
    public GameObject FloorPathLeftTile;
    public GameObject FloorPathVerticalTile;
    public GameObject FloorPathUpTile;
    public GameObject FloorPathDownTile;

    public GameObject FloorTile;
    public GameObject FloorLeftTile;
    public GameObject FloorRightTile;
    public GameObject FloorTileUp;
    public GameObject FloorTileUpRightCorner;
    public GameObject FloorTileUpLeftCorner;
    public GameObject FloorTileDown;
    public GameObject FloorTileDownRightCorner;
    public GameObject FloorTileDownLeftCorner;

    public GameObject ClosedDoorVertical;
    public GameObject ClosedDoorHorizontal;

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

    public List<GameObject> map;

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
        if (level <= 10)
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

        TileManager.Exit                    = Exit;
        TileManager.FloorTile               = FloorTile;
        TileManager.WallTileHorizontal      = WallTileHorizontal;
        TileManager.WallTileVertical        = WallTileVertical;
        TileManager.WallUpLeftCornerTile    = WallUpLeftCornerTile;
        TileManager.WallUpRightCornerTile   = WallUpRightCornerTile;
        TileManager.WallDownLeftCornerTile  = WallDownLeftCornerTile;
        TileManager.WallDownRightCornerTile = WallDownRightCornerTile;
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
        while (roomsCount-- > 0)
        {
           // Debug.Log($"Room  with number {roomsCount} is generating");
            int width   = Random.Range(RoomsSize.Min, RoomsSize.Max);
            int height  = Random.Range(RoomsSize.Min, RoomsSize.Max);
            int x       = Random.Range(0, MapWidth - width);
            int y       = Random.Range(0, MapHeight - height);

            Room room = factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            
            while (Rooms.FirstOrDefault(r => r.IsIntersectedWith(room)) != null)
            {
                width   = Random.Range(RoomsSize.Min, RoomsSize.Max);
                height  = Random.Range(RoomsSize.Min, RoomsSize.Max);
                x       = Random.Range(0, MapWidth - width);
                y       = Random.Range(0, MapHeight - height);
                room    = factory.MakeRoom(new Vector2(width, height), new Vector3(x, y));
            }

            Rooms.Add(room);
            DrawRoom(room);
        }

        for (int i = 1; i < Rooms.Count; i++)
        {
            var prev = Rooms[i - 1];
            var currt = Rooms[i];

            var pathType = Random.Range(0, 2);
            if (pathType == 0)
            {
                
                int width = (prev.Width % 2 == 0) ? prev.Width - 1 : prev.Width;
                if (prev.GetCenter().x > currt.GetCenter().x)
                {
                    var upEnter     = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y + 1);
                    var downEnter   = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y - 1);
                    ReplaceWith(factory.DungeonInfo.WallDownRightCornerTile, upEnter);
                    ReplaceWith(factory.DungeonInfo.WallUpRightCornerTile, downEnter);
                    VRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y));
                }
                else
                {
                    var upEnter     = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y + 1);
                    var downEnter   = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y - 1);
                    ReplaceWith(factory.DungeonInfo.WallDownLeftCornerTile, upEnter);
                    ReplaceWith(factory.DungeonInfo.WallUpLeftCornerTile, downEnter);
                    VRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x + (width / 2), prev.GetCenter().y));
                }
                CreateHorizontalPath(factory, (int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
                CreateVerticalPath(factory, (int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);

            }
            else
            {
                int height = (prev.Height % 2 == 0) ? prev.Height - 1 : prev.Height;
                if (prev.GetCenter().y > currt.GetCenter().y)
                {
                    var upEnter     = new Vector3(prev.GetCenter().x + 1, prev.GetCenter().y - (prev.Height / 2));
                    var downEnter   = new Vector3(prev.GetCenter().x - 1, prev.GetCenter().y - (prev.Height / 2));
                    ReplaceWith(factory.DungeonInfo.WallUpRightCornerTile, downEnter);
                    ReplaceWith(factory.DungeonInfo.WallUpLeftCornerTile, upEnter);
                    HRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x, prev.GetCenter().y - (prev.Height / 2)));
                }
                else
                {
                    var upEnter     = new Vector3(prev.GetCenter().x + 1, prev.GetCenter().y + (height / 2));
                    var downEnter   = new Vector3(prev.GetCenter().x - 1, prev.GetCenter().y + (height / 2));
                    ReplaceWith(WallDownRightCornerTile,    downEnter);
                    ReplaceWith(WallDownLeftCornerTile,     upEnter);
                    HRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x, prev.GetCenter().y + (height / 2)));
                }
                CreateHorizontalPath(factory, (int)currt.GetCenter().x, (int)prev.GetCenter().x, (int)currt.GetCenter().y);
                CreateVerticalPath(factory, (int)prev.GetCenter().y, (int)currt.GetCenter().y, (int)prev.GetCenter().x);
            }


        }
        PlaceTourches(factory);
        PlaceTraps(factory);
        
        PlaceEnemies(factory);
        PlaceDoors();

        //PlaceFountains();

        var lastRoomCenter = Rooms.Last().GetCenter();
        Instantiate(factory.DungeonInfo.Exit, lastRoomCenter, Quaternion.identity);
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
        Destroy(gameObj);
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
            Instantiate(TourchTile, vec, Quaternion.identity).transform.SetParent(boardHolder);
    }

    private void PlaceTraps(DungeonFactory factory)
    {
        InnerRoomCoords.AddRange(Rooms.Skip(1).SelectMany(room => room.InnerCoords));
        int count = CountOfTraps;
        foreach (var vec in factory.MakeTrapsIn(count, UpWallCoords.ToArray()))
        {
            Instantiate(Traps[0], GetRandVectorFrom(InnerRoomCoords), Quaternion.identity).transform.SetParent(boardHolder);
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

    private void PlaceDoors()
    {
        int h_count = HRoomDoorsCoords.Count;
        int v_count = VRoomDoorsCoords.Count;
        while (h_count-- > 0)
        {
            var vec = GetRandVectorFrom(HRoomDoorsCoords);
            RaycastHit2D hit_left   = Physics2D.Linecast(vec, Vector2.left,     blockingLayer);
            RaycastHit2D hit_right  = Physics2D.Linecast(vec, Vector2.right,    blockingLayer);
            if (hit_left.transform != null && hit_right.transform != null)
            {
                var collider_left   = hit_left.collider;
                var collider_right  = hit_right.collider;
                if (collider_right.gameObject.tag == "Wall" && collider_left.gameObject.tag == "Wall")
                    AddGameObjectToMap(Instantiate(ClosedDoorVertical, vec, Quaternion.identity));
                else
                    continue;
            }
            else
                continue;
        }

        while (v_count-- > 0)
        {
            var vec = GetRandVectorFrom(VRoomDoorsCoords);
            RaycastHit2D hit_up     = Physics2D.Raycast(vec, Vector2.up,    blockingLayer);
            RaycastHit2D hit_down   = Physics2D.Raycast(vec, Vector2.down,  blockingLayer);
            if (hit_up.transform != null && hit_down.transform != null)
            {
                var collider_left   = hit_up.collider;
                var collider_right  = hit_down.collider;
                if (collider_right.gameObject.tag == "Wall" && collider_left.gameObject.tag == "Wall")
                    AddGameObjectToMap(Instantiate(ClosedDoorHorizontal, vec, Quaternion.identity));

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
}
