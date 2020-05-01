using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Tile
{ 
    public Vector3 Location { get; set; }
    public GameObject Body { get; set; }

    public Tile(Vector3 location, GameObject body)
    {
        Location = location;
        Body = body;
    }

}
