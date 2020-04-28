using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
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
public class BoardManager : MonoBehaviour
{

    private Transform boardHolder;

    public GameObject Exit;
    public GameObject FloorTile;
    public GameObject WallTileHorizontal;
    public GameObject WallTileVertical;
    public GameObject WallUpLeftCornerTile;
    public GameObject WallUpRightCornerTile;
    public GameObject WallDownLeftCornerTile;
    public GameObject WallDownRightCornerTile;

    public Size RoomsSize;
    public int CountOfRooms;
    public int MapWidth = 70;
    public int MapHeight = 40;
    public List<Room> Rooms;
    public void SetUpLevel(int level) 
    {
        BoardSetUp();
        Generate(15);
        Rooms.ForEach(r => DrawRoom(r));
    }

    private void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        Rooms = new List<Room>();
        
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
            Instantiate(tile.Body, tile.Location, Quaternion.identity).transform.SetParent(boardHolder);
    }

    private void Generate(int roomsCount)
    {
        while (roomsCount-- > 0)
        {
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

            Debug.Log($"Counts of Rooms: {roomsCount}");
            Rooms.Add(room);

            for (int i = 1; i < Rooms.Count; i++)
            {
                var prev = Rooms[i - 1];
                var currt = Rooms[i];

                CreateHorizontalPath((int)prev.GetCenter().x, (int)currt.GetCenter().x, (int)prev.GetCenter().y);
                CreateVerticalPath((int)currt.GetCenter().y, (int)prev.GetCenter().y, (int)currt.GetCenter().x);
            }
        }
    }

    private void CreateHorizontalPath(int xStart, int xEnd, int y)
    {
        int min = Math.Min(xStart, xEnd);
        int max = Math.Max(xStart, xEnd);

        for (int x = min; x < max + 1; x++)
        {
            Instantiate(TileManager.FloorTile, new Vector3(x, y, 0), Quaternion.identity);
        }
    }

    private void CreateVerticalPath(int yStart, int yEnd, int x)
    {
        int min = Math.Min(yStart, yEnd);
        int max = Math.Max(yStart, yEnd);

        for (int y = min; y < max + 1; y++)
        {
            Instantiate(TileManager.FloorTile, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
