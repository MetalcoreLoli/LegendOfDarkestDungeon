
using Assets.Scripts.Dices;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Builders;
using Assets.Scripts.Actors.Stats;

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
                LuckyMod = CalculateModificators(lucky);
            }
        }



        protected virtual int CalculateModificators(int value)
             => (value - 10 != 0) ? (value - 10) / 2 : 0;
        
        public ActorCharacteristics() { }

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



        public static ActorCharacteristics FromTemplate(ActorCharacteristicsTemplate template)
        {
            var builder = new ActorCharacteristicsBuilder();

            builder
                .WithHp(DiceManager.RollUndSumFromString(template.Hp)).WithMaxHp(DiceManager.RollUndSumFromString(template.MaxHp))
                .WithMp(DiceManager.RollUndSumFromString(template.Mp)).WithMaxMp(DiceManager.RollUndSumFromString(template.MaxMp))
                .Characteristic
                    .IntelligenceWithValue(DiceManager.RollUndSumFromString(template.Intelligence)).Modifactor.CalculateIntelligence()
                .Characteristic
                    .LuckyWithValue(DiceManager.RollUndSumFromString(template.Lucky)).Modifactor.CalculateLucky()
                .Characteristic
                    .CharismaWithValue(DiceManager.RollUndSumFromString(template.Charisma)).Modifactor.CalculateCharisma()
                .Characteristic
                    .StrengthWithValue(DiceManager.RollUndSumFromString(template.Strength)).Modifactor.CalculateStrength()
                .Characteristic
                    .DexterityWithValue(DiceManager.RollUndSumFromString(template.Dexterity)).Modifactor.CalculateDexterity();

            return builder.Build();
        }

        public static ActorCharacteristics FromDices(string diceForHp, string diceForMp)
        {
            var builder = new ActorCharacteristicsBuilder();

            var hpDice = DiceManager.RollUndSumFromString(diceForHp);
            var mpDice = DiceManager.RollUndSumFromString(diceForMp);

            builder
                .WithHp(hpDice).WithMaxHp(hpDice)
                .WithMp(mpDice).WithMaxMp(mpDice)
                .Characteristic
                    .IntelligenceWithValue(DiceManager.RollUndSumFromString("4d6")).Modifactor.CalculateIntelligence()
                .Characteristic
                    .LuckyWithValue(DiceManager.RollUndSumFromString("4d6")).Modifactor.CalculateLucky()
                .Characteristic
                    .CharismaWithValue(DiceManager.RollUndSumFromString("4d6")).Modifactor.CalculateCharisma()
                .Characteristic
                    .StrengthWithValue(DiceManager.RollUndSumFromString("4d6")).Modifactor.CalculateStrength()
                .Characteristic
                    .DexterityWithValue(DiceManager.RollUndSumFromString("4d6")).Modifactor.CalculateDexterity();

            return builder.Build();
        }
    }
}
