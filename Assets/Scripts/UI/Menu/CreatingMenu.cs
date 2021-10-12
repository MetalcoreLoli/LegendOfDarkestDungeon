using Assets.Scripts.Actors;
using Assets.Scripts.Actors.Stats;
using Assets.Scripts.Stats;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private ActorCharacteristics _player = new ActorCharacteristics();
        private void Update()
        {
            //GameManager.Instance.Player.GetComponent<Actor>();
            Func<int, string> mod = num => (num > 0) ? $"+{num}" : num.ToString();
            IntText.text = $"Int: {_player.Intelligence} ({mod(_player.IntelligenceMod)})";
            StrText.text = $"Str: {_player.Strength} ({mod(_player.StrengthMod)})";
            DexText.text = $"Dex: {_player.Dexterity} ({mod(_player.DexterityMod)})";
            ChrText.text = $"Chr: {_player.Charisma} ({mod(_player.CharismaMod)})";
            LckText.text = $"Lck: {_player.Lucky} ({mod(_player.LuckyMod)})";
            ManaPointsText.text = $"Start ManaPoints : {_player.MaxMp}";
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
                SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        public void Roll()
        {
            _player = ActorCharacteristics.FromTemplate(template); 
        }
    }
}