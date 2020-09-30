﻿using UnityEngine;

public struct Tile
{
    public Vector3 Location { get; set; }
    public GameObject Body { get; set; }

    public Tile(Vector3 location, GameObject body = null)
    {
        Location = location;
        Body = body;
    }
}