using Assets.Scripts.Actors.Stats;
using Assets.Scripts.Builders;
using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    [Serializable]
    public class Actor : MonoBehaviour
    {
        [SerializeField] private ActorCharacteristics characteristics;
        public ActorCharacteristics Characteristics { get => characteristics; set => characteristics = value; }

        [SerializeField] private ActorCharacteristicsTemplate template;

        private void Awake()
        {
            characteristics = 
                template == null 
                ? FromDices("6d6", "1d12") 
                : FromTemplate(template);
        }

        public static ActorCharacteristics FromTemplate(ActorCharacteristicsTemplate template)
        {
            var builder = new ActorCharacteristicsBuilder();

            builder
                .WithHp(DiceManager.RollUndSumFromString(template.Hp)).WithMaxHp(DiceManager.RollUndSumFromString(template.MaxHp))
                .WithMp(DiceManager.RollUndSumFromString(template.Mp)).WithMaxMp(DiceManager.RollUndSumFromString(template.MaxMp))
                .Characteristic
                    .IntelligenceWithValue (DiceManager.RollUndSumFromString(template.Intelligence)).Modifactor.CalculateIntelligence()
                .Characteristic
                    .LuckyWithValue (DiceManager.RollUndSumFromString(template.Lucky))              .Modifactor.CalculateLucky()
                .Characteristic
                    .CharismaWithValue (DiceManager.RollUndSumFromString(template.Charisma))        .Modifactor.CalculateCharisma()
                .Characteristic
                    .StrengthWithValue (DiceManager.RollUndSumFromString(template.Strength))        .Modifactor.CalculateStrength()
                .Characteristic
                    .DexterityWithValue (DiceManager.RollUndSumFromString(template.Dexterity))      .Modifactor.CalculateDexterity();

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
