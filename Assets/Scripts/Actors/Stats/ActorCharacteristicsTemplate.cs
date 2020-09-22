using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actors.Stats
{
    [CreateAssetMenu(fileName = "ActorCharacteristicsTemplate", menuName = "New Characteristics Template", order = 54)]

    public class ActorCharacteristicsTemplate : ScriptableObject
    {
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string hp;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string maxHp;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string mp;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string maxMp;
                                 
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string intelligence;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string charisma;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string dexterity;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string strength;
        [SerializeField, Tooltip("Dice in format {count of dices}d{count of edges}")] private string lucky;
        public string Hp => hp;
        public string MaxHp => maxHp;
        public string Mp => mp;
        public string MaxMp => maxMp;
               
        public string Lucky => lucky;
        public string Charisma => charisma;
        public string Dexterity => dexterity;
        public string Strength => strength;
        public string Intelligence => intelligence;
    }
}
