namespace Assets.Scripts.Dungeon
{
    public interface IDungeonGeneratorSetup
    {
        int RoomMin { get; set; }
        int RoomMax { get; set; }
        int TourchesMin { get; set; }
        int TourchesMax { get; set; }
        int MapWidth { get; set; }
        int MapHeight { get; set; }
        int CountOfRooms { get; set; }
        int CountOfTraps { get; set; }
        int CountOfEnemies { get; set; }
    }
}