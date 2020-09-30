namespace Assets.Scripts.Spells
{
    public interface ISpell
    {
        SpellInfo Info { get; set; }

        void Cast();
    }
}