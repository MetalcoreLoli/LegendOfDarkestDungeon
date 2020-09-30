using Assets.Scripts.Actors;
using Assets.Scripts.Actors.Stats;
using Assets.Scripts.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class CreatingMenu : Menu
    {
        public Text IntText;
        public Text StrText;
        public Text DexText;
        public Text ChrText;
        public Text LckText;
        public Text ManaPointsText;
        public InputField NameText;

        [SerializeField] private ActorCharacteristicsTemplate template;

        private void Update()
        {
            var player = GameManager.Instance.Player.GetComponent<Actor>();
            Func<int, string> mod = num => (num > 0) ? $"+{num}" : num.ToString();
            IntText.text = $"Int: {player.Characteristics.Intelligence} ({mod(player.Characteristics.IntelligenceMod)})";
            StrText.text = $"Str: {player.Characteristics.Strength} ({mod(player.Characteristics.StrengthMod)})";
            DexText.text = $"Dex: {player.Characteristics.Dexterity} ({mod(player.Characteristics.DexterityMod)})";
            ChrText.text = $"Chr: {player.Characteristics.Charisma} ({mod(player.Characteristics.CharismaMod)})";
            LckText.text = $"Lck: {player.Characteristics.Lucky} ({mod(player.Characteristics.LuckyMod)})";
            ManaPointsText.text = $"Start ManaPoints : {player.Characteristics.MaxMp}";
        }

        public override void Open()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
        }

        public override void Close()
        {
            IsOpen = !IsOpen;
            gameObject.SetActive(IsOpen);
        }

        public void Done()
        {
            if (!string.IsNullOrEmpty(NameText.textComponent.text))
                Close();
            else
            {
                GameManager.MessageBox.Show("Error", "Enter your character name !!!");
            }
        }

        public void Roll()
        {
            GameManager.Instance.UpdatePlayersCharacteristics(ActorCharacteristics.FromTemplate(template));
        }
    }
}