using System;

namespace Assets.Scripts.Dungeon
{
    public class DungeonRoomGenerationEventArg : EventArgs
    {
        public Room Room { get; }

        public DungeonRoomGenerationEventArg(Room room)
        {
            Room = room;
        }
    }
}