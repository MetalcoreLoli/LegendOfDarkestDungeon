using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public Int32 Width { get; set; }
    public Int32 Height { get; set; }

    public Vector3 Location { get; set; }
    public Tile[] Body { get; private set; }

    public Room(int width, int height, Vector3 location)
    {
        Width = width;
        Height = height;
        Location = location;
        Body = Create(Width, Height);

    }

    private Tile[] Create(int width, int height)
    {
        Tile[] temp = new Tile[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int idx = x + width * y;

                temp[idx] = new Tile(new Vector3(x, y, 0) + Location, TileManager.FloorTile);

                if (x == 0 && y == 0)
                    temp[idx].Body = TileManager.WallDownLeftCornerTile;

                if (x == width - 1 && y == 0)
                    temp[idx].Body = TileManager.WallDownRightCornerTile;

                if (x == 0 && y == height - 1)
                    temp[idx].Body = TileManager.WallUpLeftCornerTile;

                if (x == width - 1 && y == height - 1)
                    temp[idx].Body = TileManager.WallUpRightCornerTile;

                if ((x == width - 1 && y > 0 && y < height - 1))
                    temp[idx].Body = TileManager.WallTileVertical;

                if ((y == height - 1 && x > 0 && x < width - 1))
                    temp[idx].Body = TileManager.WallTileHorizontal;

                if (x > 0 && x < width - 1 && y == 0)
                    temp[idx].Body = TileManager.WallTileHorizontal;

                if (y > 0 && y < height - 1 && x == 0)
                    temp[idx].Body = TileManager.WallTileVertical;

            }
        }

        return temp;
    }

    public bool IsIntersectedWith(Room room)
    {

        return room.Body.Intersect(Body).Count() != 0;
        //return room.Location.x > (Width + Location.x) && room.Location.y > (Height + Location.y) && room.Location.x < Location.x && room.Location.y < Location.y || 
        //    (room.Location.x + room.Width) < Location.x && (room.Location.y + Height) < Location.y && room.Location.x > Location.x && room.Location.y > Location.y;
    }

    public Vector3 GetCenter()
    { 
        int idx = (Width / 2) + Width * (Height / 2);
        return Body[idx].Location;
    }
}
