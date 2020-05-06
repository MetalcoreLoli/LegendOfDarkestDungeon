using Assets.Scripts.Dices;
using System;

namespace Assets.Scripts.Stats
{
    [Serializable]
    public class ActorCharacteristics : ICharacteristics, ICharcteristicsMod
    {
        #region Private Members 

        private int intell;
        private int charisma;
        private int dexterity;
        private int strength;
        private int lucky;

        #endregion

        #region Public Members

        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }

        #region Modificators
        public int IntelligenceMod { get; set; }
        public int CharismaMod { get; set; }
        public int DexterityMod { get; set; }
        public int StrengthMod { get; set; }
        public int LuckyMod { get; set; }
        #endregion

        #region Stats
        public int Intelligence 
        { 
            get => intell;
            set 
            {
                intell = value;
                IntelligenceMod = CalculateModificators(intell);
            } 
        }
        public int Charisma
        { 
            get => charisma;
            set
            {
                charisma = value;
                CharismaMod = CalculateModificators(charisma);
            }
        }
        public int Dexterity 
        { 
            get => dexterity;
            set
            {
                dexterity = value;
                DexterityMod = CalculateModificators(dexterity);
            }
        }
        public int Strength 
        { 
            get => strength;
            set
            {
                strength = value;
                LuckyMod = CalculateModificators(strength);
            }
        }
        public int Lucky 
        { 
            get => lucky;
            set 
            {
                lucky = value;
                CalculateModificators(lucky);
            }
        }
        #endregion

        #endregion


        protected virtual int CalculateModificators(int value) 
            => (value - 10 != 0) ? (value - 10) / 2 : 0;


        public ActorCharacteristics(int maxHp, int maxMp)
        {
            RollStats();
            Hp      = MaxHp = maxHp;
            maxMp   += IntelligenceMod;
            Mp      = MaxMp = maxMp;
            
        }

        public void RollStats()
        {
            Strength        = DiceManager.RollUndSumFromString("4d6");
            Charisma        = DiceManager.RollUndSumFromString("4d6");
            Dexterity       = DiceManager.RollUndSumFromString("4d6");
            Lucky           = DiceManager.RollUndSumFromString("4d6");
            Intelligence    = DiceManager.RollUndSumFromString("4d6");
        }
    }
}
