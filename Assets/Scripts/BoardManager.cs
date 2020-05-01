using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor.UIElements;
using UnityEditor;

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

    public Size RoomsSize;
    public int CountOfRooms;
    public int MapWidth = 70;
    public int MapHeight = 40;
    public List<Room> Rooms;

    private List<GameObject> map;
    public void SetUpLevel(int level) 
    {
        BoardSetUp();
        Generate(15);
        //Rooms.ForEach(r => DrawRoom(r));
    }

    private void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        Rooms = new List<Room>();
        map = new List<GameObject>();
        
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
            Debug.Log($"Room  with number {roomsCount} is generating");
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
                if (prev.GetCenter().x > currt.GetCenter().x)
                {
                    var upEnter = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y + 1);
                    var downEnter = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y - 1);
                    ReplaceWith(WallDownRightCornerTile, upEnter);
                    ReplaceWith(WallUpRightCornerTile, downEnter);
                }
                else
                {
                    int width = (prev.Width % 2 == 0) ? prev.Width - 1 : prev.Width;
                    var upEnter = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y + 1);
                    var downEnter = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y - 1);
                    ReplaceWith(WallDownLeftCornerTile, upEnter);
                    ReplaceWith(WallUpLeftCornerTile, downEnter);
                }
                CreateHorizontalPath((int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
                CreateVerticalPath((int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);
            }
            else
            {
                //if (prev.GetCenter().x > currt.GetCenter().x)
                //{
                //    var upEnter = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y + 1);
                //    var downEnter = new Vector3(prev.GetCenter().x - (prev.Width / 2), prev.GetCenter().y - 1);
                //    ReplaceWith(WallDownRightCornerTile, upEnter);
                //    ReplaceWith(WallUpRightCornerTile, downEnter);
                //}
                //else
                //{
                //    int width = (prev.Width % 2 == 0) ? prev.Width - 1 : prev.Width;
                //    var upEnter = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y + 1);
                //    var downEnter = new Vector3((int)prev.GetCenter().x + (width / 2), prev.GetCenter().y - 1);
                //    ReplaceWith(WallDownLeftCornerTile, upEnter);
                //    ReplaceWith(WallUpLeftCornerTile, downEnter);
                //}
                CreateHorizontalPath((int)currt.GetCenter().x, (int)prev.GetCenter().x, (int)currt.GetCenter().y);
                CreateVerticalPath((int)prev.GetCenter().y, (int)currt.GetCenter().y, (int)prev.GetCenter().x);
            }


        }

        var lastRoomCenter = Rooms.Last().GetCenter();
        Instantiate(Exit, lastRoomCenter, Quaternion.identity);

        //TODO: ADD PLAYER SPAWN HERE !!!
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
}
