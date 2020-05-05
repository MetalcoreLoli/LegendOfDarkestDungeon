using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Stats
{
    public abstract class ActorCharacteristics : ICharacteristics, ICharcteristicsMod
    {
        public int IntelligenceMod  { get; set; }
        public int CharismaMod      { get; set; }
        public int DexterityMod     { get; set; }
        public int StrengthMod      { get; set; }
        public int LuckyMod         { get; set; }
        public int Hp               { get; set; }
        public int MaxHp            { get; set; }
        public int Mp               { get; set; }
        public int MaxMp            { get; set; }
        public int Intelligence     { get; set; }
        public int Charisma         { get; set; }
        public int Dexterity        { get; set; }
        public int Strength         { get; set; }
        public int Lucky            { get; set; }

        protected virtual int CalculateModificators(int value) 
            => (value - 10 != 0) ? (value - 10) / 2 : 0;
    
        

    }
}
