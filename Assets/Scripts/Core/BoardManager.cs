using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

using Random = UnityEngine.Random;
using Assets.Scripts.Dices;
using System.Security.Cryptography;

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

    public void SetUpLevel(int level) 
    {
        Debug.Log("Start board");
        BoardSetUp();
        //CountOfRooms = (int)Mathf.Log(level) + 10;
        Generate(CountOfRooms);
        //Rooms.ForEach(r => DrawRoom(r));
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

    private void Generate(int roomsCount)
    {
        while (roomsCount-- > 0)
        {
           // Debug.Log($"Room  with number {roomsCount} is generating");
            int width   = Random.Range(RoomsSize.Min, RoomsSize.Max);
            int height  = Random.Range(RoomsSize.Min, RoomsSize.Max);
            int x       = Random.Range(0, MapWidth - width);
            int y       = Random.Range(0, MapHeight - height);

            Room room = new Room(width, height, new Vector3(x, y, 0));
            
            while (Rooms.FirstOrDefault(r => r.IsIntersectedWith(room)) != null)
            {
                width   = Random.Range(RoomsSize.Min, RoomsSize.Max);
                height  = Random.Range(RoomsSize.Min, RoomsSize.Max);
                x       = Random.Range(0, MapWidth - width);
                y       = Random.Range(0, MapHeight - height);
                room = new Room(width, height, new Vector3(x, y, 0));
            }

         
            for (int x1 = 1; x1 < width-1; x1++)
            {
                for (int y1 = 1; y1 < height-1; y1++)
                {
                    int idx = x1 + width * y1;
                    room.Body[idx].Body = FloorTile;

                    if (x1 == 1 && y1 == 1)
                        room.Body[idx].Body = FloorTileDownLeftCorner;

                    if (x1 == width - 2 && y1 == 1)
                        room.Body[idx].Body = FloorTileDownRightCorner;

                    if (x1 == 1 && y1 == height - 2)
                        room.Body[idx].Body = FloorTileUpLeftCorner;

                    if (x1 == width - 2 && y1 == height - 2)
                        room.Body[idx].Body = FloorTileUpRightCorner;

                    if ((x1 == width - 2 && y1 > 1 && y1 < height - 2))
                        room.Body[idx].Body = FloorRightTile;

                    if ((y1 == height - 2 && x1 > 1 && x1 < width - 2))
                        room.Body[idx].Body = FloorTileUp;

                    if (x1 > 1 && x1 < width - 2 && y1 == 1)
                        room.Body[idx].Body = FloorTileDown;

                    if (y1 > 1 && y1 < height - 2 && x1 == 1)
                        room.Body[idx].Body = FloorLeftTile;

                }
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
                    ReplaceWith(WallDownRightCornerTile, upEnter);
                    ReplaceWith(WallUpRightCornerTile, downEnter);
                    VRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y));
                }
                else
                {
                    var upEnter     = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y + 1);
                    var downEnter   = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y - 1);
                    ReplaceWith(WallDownLeftCornerTile, upEnter);
                    ReplaceWith(WallUpLeftCornerTile, downEnter);
                    VRoomDoorsCoords.Add(new Vector3(prev.GetCenter().x + (width / 2), prev.GetCenter().y));
                }
                CreateHorizontalPath((int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
                CreateVerticalPath((int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);

            }
            else
            {
                int height = (prev.Height % 2 == 0) ? prev.Height - 1 : prev.Height;
                if (prev.GetCenter().y > currt.GetCenter().y)
                {
                    var upEnter     = new Vector3(prev.GetCenter().x + 1, prev.GetCenter().y - (prev.Height / 2));
                    var downEnter   = new Vector3(prev.GetCenter().x - 1, prev.GetCenter().y - (prev.Height / 2));
                    ReplaceWith(WallUpRightCornerTile, downEnter);
                    ReplaceWith(WallUpLeftCornerTile, upEnter);
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
                CreateHorizontalPath((int)currt.GetCenter().x, (int)prev.GetCenter().x, (int)currt.GetCenter().y);
                CreateVerticalPath((int)prev.GetCenter().y, (int)currt.GetCenter().y, (int)prev.GetCenter().x);
            }


        }
        PlaceTourches();
        PlaceDoors();

        PlaceTraps();
        PlaceEnemies();

        //PlaceFountains();

        var lastRoomCenter = Rooms.Last().GetCenter();
        Instantiate(Exit, lastRoomCenter, Quaternion.identity);
    }
    private void CreateHorizontalPath(int xStart, int xEnd, int y)
    {
        int min = Math.Min(xStart, xEnd);
        int max = Math.Max(xStart, xEnd);

        for (int x = min; x < max + 1; x++)
        {
            //FindUndDestory(new Vector3(x, y, 0));
            //if (x == max)
            //    AddGameObjectToMap(Instantiate(FloorPathRightTile, new Vector3(x, y, 0), Quaternion.identity));
            //else 
            //    AddGameObjectToMap(Instantiate(FloorPathHorizontalTile, new Vector3(x, y, 0), Quaternion.identity));
            ReplaceWith(FloorTile, new Vector3(x, y, 0));

            var upWallVector = new Vector3(x, y + 1, 0);
            var downWallVector = new Vector3(x, y - 1, 0);

            if (map.FirstOrDefault(obj => obj.transform.position == upWallVector) == null)
                AddGameObjectToMap(Instantiate(WallTileHorizontal, upWallVector, Quaternion.identity));

            if (map.FirstOrDefault(obj => obj.transform.position == downWallVector) == null)
                AddGameObjectToMap(Instantiate(WallTileHorizontal, downWallVector, Quaternion.identity));
        }
    }

    private void CreateVerticalPath(int yStart, int yEnd, int x)
    {
        int min = Math.Min(yStart, yEnd);
        int max = Math.Max(yStart, yEnd);

        for (int y = min; y < max + 1; y++)
        {
            //FindUndDestory(new Vector3(x, y, 0));
            //AddGameObjectToMap(Instantiate(FloorPathVerticalTile, new Vector3(x, y, 0), Quaternion.identity));
            ReplaceWith(FloorTile, new Vector3(x, y, 0));
            var leftWall = new Vector3(x + 1, y, 0);
            var rightWall = new Vector3(x - 1, y, 0);

            if (map.FirstOrDefault(obj => obj.transform.position == leftWall) == null)
                AddGameObjectToMap(Instantiate(WallTileVertical, leftWall, Quaternion.identity));

            if (map.FirstOrDefault(obj => obj.transform.position == rightWall) == null)
                AddGameObjectToMap(Instantiate(WallTileVertical, rightWall, Quaternion.identity));
        }
    }

    private void AddGameObjectToMap(GameObject obj)
    {
        map.Add(obj);
        obj.transform.SetParent(boardHolder);
    }

    /// <summary>
    /// Уничтожает GameObject на заданной позиции
    /// </summary>
    /// <param name="vector3"></param>
    private void FindUndDestory(Vector3 vector3)
    {
        foreach (GameObject obj in map)
        {
            if (obj.transform.position == vector3) Destroy(obj);
        }
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

    private void PlaceTourches()
    {
        UpWallCoords.AddRange(Rooms.SelectMany(room => room.UpWallCoord));
        int count = Tourches.Max;
        while (count-- > 0)
        {
            Instantiate(TourchTile, GetRandVectorFrom(UpWallCoords), Quaternion.identity).transform.SetParent(boardHolder);
        }
    }

    private void PlaceTraps()
    {
        InnerRoomCoords.AddRange(Rooms.Skip(1).SelectMany(room => room.InnerCoords));
        int count = CountOfTraps; 
        while (count-- > 0)
        {
            Instantiate(Traps[0], GetRandVectorFrom(InnerRoomCoords), Quaternion.identity).transform.SetParent(boardHolder);
        }
    }

    private void PlaceEnemies()
    {
        int count = CountOfEnemies;

        while (count-- > 0)
        {
            var enemy = Instantiate(EnemyPrefab, GetRandVectorFrom(InnerRoomCoords), Quaternion.identity);
            enemy.transform.SetParent(boardHolder);
        }
    }

    private void PlaceFountains()
    {
        AddGameObjectToMap(Instantiate(BloodFountain, GetRandVectorFrom(InnerRoomCoords), Quaternion.identity));    
    }

    private void PlaceDoors()
    {
        //foreach (var room in Rooms)
        //{
        //    RaycastHit2D hit_up     = Physics2D.Raycast(room.GetCenter(), Vector2.up);
        //    RaycastHit2D hit_down   = Physics2D.Raycast(room.GetCenter(), Vector2.down);
        //    if (hit_up.transform != null)
        //    {
        //    }
        //}

        int h_count = HRoomDoorsCoords.Count;
        int v_count = VRoomDoorsCoords.Count;
        while (h_count-- > 0)
        {
            var vec = GetRandVectorFrom(HRoomDoorsCoords);
            RaycastHit2D hit_left   = Physics2D.Raycast(vec, Vector2.left);
            RaycastHit2D hit_right  = Physics2D.Raycast(vec, Vector2.right);
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
            RaycastHit2D hit_up = Physics2D.Raycast(vec, Vector2.up);
            RaycastHit2D hit_down = Physics2D.Raycast(vec, Vector2.down);
            if (hit_up.transform != null && hit_down.transform != null)
            {
                var collider_left = hit_up.collider;
                var collider_right = hit_down.collider;
                if (collider_right.gameObject.tag == "Wall" && collider_left.gameObject.tag == "Wall")
                    AddGameObjectToMap(Instantiate(ClosedDoorHorizontal, vec, Quaternion.identity));

            }
        }
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
