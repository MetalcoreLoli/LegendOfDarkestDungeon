namespace Assets.Scripts.Stats
{
    public interface ICharcteristicsMod
    {
        int IntelligenceMod { get; set; }
        int CharismaMod { get; set; }
        int DexterityMod { get; set; }
        int StrengthMod { get; set; }
        int LuckyMod { get; set; }
    }
}