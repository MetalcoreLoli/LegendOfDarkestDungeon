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

    public List<Vector3> UpWallCoord { get; set; }
    public List<Vector3> InnerCoords { get; set; }
    public Room(int width, int height, Vector3 location)
    {
        Width = width;
        Height = height;
        Location = location;
        UpWallCoord = new List<Vector3>();
        InnerCoords = new List<Vector3>();
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

                temp[idx] = new Tile(new Vector3(x, y, 0) + Location);

               

            }
        }

        for (int x = 1; x < width-1; x++)
            for (int y = 1; y < height-1; y++)
                InnerCoords.Add(new Vector3(x, y)+ Location);
                
        return temp;
    }

    public bool IsIntersectedWith(Room room)
    {

        //return room.Body.Intersect(Body).Count() != 0;

        foreach (var cell in room.Body)
        {
            if (Body.Select(t => t.Location).Contains(cell.Location))
                return true;
        }
        return false;
    }

    public Vector3 GetCenter()
    { 
        int idx = (Width / 2) + Width * (Height / 2);
        return Body[idx].Location;
    }
}
