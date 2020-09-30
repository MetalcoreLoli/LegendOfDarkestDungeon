namespace Assets.Scripts.Stats
{
    public interface ICharacteristics
    {
        int Hp { get; set; }
        int MaxHp { get; set; }
        int Mp { get; set; }
        int MaxMp { get; set; }
        int Intelligence { get; set; }
        int Charisma { get; set; }
        int Dexterity { get; set; }
        int Strength { get; set; }
        int Lucky { get; set; }
    }
}