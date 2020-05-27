
using Assets.Scripts.Dices;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Stats
{
    [Serializable]
    public class ActorCharacteristics : ICharacteristics, ICharcteristicsMod
    {

        [SerializeField] private int intell;
        [SerializeField] private int charisma;
        [SerializeField] private int dexterity;
        [SerializeField] private int strength;
        [SerializeField] private int lucky;

        [SerializeField] private int _hp;
        [SerializeField] private int _mp;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _maxMp;

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }


        public int Mp
        {
            get => _mp;
            set => _mp = value;
        }
        public int MaxHp { get => _maxHp; set => _maxHp = value; }

        public int MaxMp { get => _maxMp; set => _maxMp = value; }

        public int IntelligenceMod { get; set; }
        public int CharismaMod { get; set; }
        public int DexterityMod { get; set; }
        public int StrengthMod { get; set; }
        public int LuckyMod { get; set; }

        public int Intelligence
        {
            get => intell;
            set
            {
                intell = value;
                IntelligenceMod = CalculateModificators(intell);
                MaxMp += IntelligenceMod;
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
                StrengthMod = CalculateModificators(strength);
                MaxHp += StrengthMod;
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



        protected virtual int CalculateModificators(int value)
             => (value - 10 != 0) ? (value - 10) / 2 : 0;


        public ActorCharacteristics(int maxHp, int maxMp)
        {
            Hp = MaxHp = maxHp;
            Mp = MaxMp = maxMp;
            RollStats();
            Hp = MaxHp;
            Mp = MaxMp;
        }

        public void RollStats()
        {
            Strength = DiceManager.RollUndSumFromString("4d6");
            Charisma = DiceManager.RollUndSumFromString("4d6");
            Dexterity = DiceManager.RollUndSumFromString("4d6");
            Lucky = DiceManager.RollUndSumFromString("4d6");
            Intelligence = DiceManager.RollUndSumFromString("4d6");
        }
    }
}
