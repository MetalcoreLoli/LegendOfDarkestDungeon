using Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class LevelUpMenu : Menu
    {
        [SerializeField] private Text intText;
        [SerializeField] private Text strText;
        [SerializeField] private Text dexText;
        [SerializeField] private Text chrText;
        [SerializeField] private Text lckText;

        [SerializeField] private Text points;
        [SerializeField] private Player player;
        [SerializeField] private ActorCharacteristics playerChaCharacteristicsBuffer;
        [SerializeField] private Int32 _points;
        [SerializeField] private Int32 _maxPoints = 2;

        public Int32 Points { get => _points; set => _points = value; }

        public Int32 MaxPoints { get => _maxPoints; set => _maxPoints = value; }

        private void Start()
        {
            UpdateChar(player.Characteristics);
        }

        private void Update()
        {
            if (IsOpen)
                GameManager._instance.enabled = false;
            else
                GameManager._instance.enabled = true;

            string func_mod(int mod) => mod < 0 ? "-" + mod : "+" + mod;
            intText.text = $"Int: {playerChaCharacteristicsBuffer.Intelligence} ({(func_mod(playerChaCharacteristicsBuffer.IntelligenceMod))})";
            strText.text = $"Str: {playerChaCharacteristicsBuffer.Strength} ({(func_mod(playerChaCharacteristicsBuffer.StrengthMod))})";
            dexText.text = $"Dex: {playerChaCharacteristicsBuffer.Dexterity} ({(func_mod(playerChaCharacteristicsBuffer.DexterityMod))})";
            chrText.text = $"Chr: {playerChaCharacteristicsBuffer.Charisma} ({(func_mod(playerChaCharacteristicsBuffer.CharismaMod))})";
            lckText.text = $"Lck: {playerChaCharacteristicsBuffer.Lucky} ({(func_mod(playerChaCharacteristicsBuffer.LuckyMod))})";

            points.text = "" + Points;
        }

        private void UpdateChar(ActorCharacteristics actorCharacteristics)
        {
            foreach (PropertyInfo property in typeof(ActorCharacteristics).GetProperties())
                property.SetValue(playerChaCharacteristicsBuffer, property.GetValue(actorCharacteristics));
        }

        public override void Close()
        {
            IsOpen = false;
            gameObject.SetActive(IsOpen);
        }

        public override void Open()
        {
            IsOpen = true;
            gameObject.SetActive(IsOpen);
        }

        public void OnDoneClick()
        {
            playerChaCharacteristicsBuffer.Hp = playerChaCharacteristicsBuffer.MaxHp;
            playerChaCharacteristicsBuffer.Mp = playerChaCharacteristicsBuffer.MaxMp;
            GameManager._instance.UpdatePlayersCharacteristics(playerChaCharacteristicsBuffer);
            MaxPoints = Points;
            Close();
        }

        public void OnAddClick(string statsName)
        {
            if (Points > 0)
            {
                Points--;
                UpdateCharacteristicValue(statsName, propertyValue => propertyValue + 1);
            }
        }

        public void OnSubClick(string statsName)
        {
            if (MaxPoints > Points)
            {
                Points++;
                UpdateCharacteristicValue(statsName, propertyValue => propertyValue - 1);
            }
        }

        private void UpdateCharacteristicValue(string statsName, Func<int, int> func)
        {
            Type characteristicType = playerChaCharacteristicsBuffer.GetType();
            PropertyInfo property = characteristicType.GetProperty(statsName);

            int propValue = (int)property.GetValue(playerChaCharacteristicsBuffer);
            property.SetValue(playerChaCharacteristicsBuffer, func(propValue));
        }
    }
}
