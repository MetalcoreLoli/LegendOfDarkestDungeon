using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Dungeon
{
    public class DungeonTileGenerationEventArg : EventArgs
    {
        public DungeonTileGenerationEventArg(GameObject tile, Vector3 tileCoords)
        {
            Tile = tile;
            TileCoords = tileCoords;
        }

        public DungeonTileGenerationEventArg(GameObject tile, Vector3 tileCoords, Room room) : this(tile, tileCoords)
        {
            Room = room;
        }

        public DungeonTileGenerationEventArg(GameObject tile, Vector3 tileCoords, Vector3 tileRoomCoords) : this(tile, tileCoords)
        {
            TileRoomCoords = tileRoomCoords;
        }

        public DungeonTileGenerationEventArg(GameObject tile, Vector3 tileCoords, Vector3 tileRoomCoords, Room room) : this(tile, tileCoords, tileRoomCoords)
        {
            Room = room;
        }

        public GameObject Tile { get; }
        public Vector3 TileCoords { get; }
        public Vector3 TileRoomCoords { get; }
        public Room Room { get; }

    }
}