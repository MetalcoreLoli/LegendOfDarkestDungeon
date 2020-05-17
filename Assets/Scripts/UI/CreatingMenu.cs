using Assets.Scripts.Dices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CreatingMenu : MonoBehaviour
    {
        public bool IsOpen { get; set; }

        public Text IntText;
        public Text StrText;
        public Text DexText;
        public Text ChrText;
        public Text LckText;
        public Text ManaPointsText;
        public InputField NameText;


        private void Update()
        {
            var player = GameManager._instance.Player;
            Func<int, string> mod = num => (num > 0) ? $"+{num}" : num.ToString();
            IntText.text = $"Int: {player.Characteristics.Intelligence} ({mod(player.Characteristics.IntelligenceMod)})";
            StrText.text = $"Str: {player.Characteristics.Strength} ({mod(player.Characteristics.StrengthMod)})";
            DexText.text = $"Dex: {player.Characteristics.Dexterity} ({mod(player.Characteristics.DexterityMod)})";
            ChrText.text = $"Chr: {player.Characteristics.Charisma} ({mod(player.Characteristics.CharismaMod)})";
            LckText.text = $"Lck: {player.Characteristics.Lucky} ({mod(player.Characteristics.LuckyMod)})";
            ManaPointsText.text = $"Start ManaPoints : {player.Characteristics.MaxMp}";
        }

        public void Open()
        { 
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
        }

        public void Close()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
        }
        public void Done()
        {
            if (!string.IsNullOrEmpty(NameText.textComponent.text))
                Close();
        }

        public void Roll()
        {
            var stats = new Stats.ActorCharacteristics(25, DiceManager.RollUndSumFromString("4d6") * 6);
            GameManager._instance.UpdatePlayersCharacteristics(stats);
           
        }
    }
}
