using System;
using UnityEngine;

public struct Tile
{
    public Vector3 Location { get; set; }
    public GameObject Template { get; set; }

    public Tile(Vector3 location, GameObject body = null)
    {
        Location = location;
        Template = body;
    }

    public bool CompareTemplateTag(string tag) => Template.CompareTag(tag);

    public override bool Equals(object obj)
    {
        if (obj is Tile b)
        {
            return this.Location == b.Location;
        }
        throw new ArgumentException("obj is not Tile");
    }
}