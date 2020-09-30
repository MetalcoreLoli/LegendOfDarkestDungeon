using Assets.Scripts.Actors.Stats;
using Assets.Scripts.Stats;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    [Serializable]
    public class Actor : MonoBehaviour
    {
        [SerializeField] private ActorCharacteristics characteristics;

        [SerializeField] private ActorCharacteristicsTemplate template;

        [SerializeField] private TextMeshProTemplate textTemplate;

        public ActorCharacteristics Characteristics { get => characteristics; set => characteristics = value; }

        public event EventHandler<int> OnHealthUpdate;

        public event EventHandler<int> OnManaUpdate;

        private void Awake()
        {
            characteristics =
                template == null
                ? ActorCharacteristics.FromDices("6d6", "1d12")
                : ActorCharacteristics.FromTemplate(template);
        }

        public void UpdateHealth(int value)
        {
            Characteristics.Hp += CorrentValue(Characteristics.Hp += value, min: 0, max: Characteristics.MaxHp);
            var color = value > 0 ? Color.green : Color.red;
            TextPopUp.CreateWithColor(transform.position, "+" + value, textTemplate.Text.transform, color);
            OnHealthUpdate?.Invoke(this, Characteristics.Hp);
        }

        public void UpdateMana(int value)
        {
            Characteristics.Mp = CorrentValue(Characteristics.Mp += value, min: 0, max: Characteristics.MaxMp);
            TextPopUp.CreateWithColor(transform.position, "+" + value, textTemplate.Text.transform, Color.blue);
            OnManaUpdate?.Invoke(this, Characteristics.Mp);
        }

        /// <summary>
        /// Корректирует значение (value), если оно больше максимимума (max),
        /// то value присваивается max, иначе если
        /// value меньше либо равно минимуму (min),
        /// то ему присваивается min
        /// </summary>
        /// <param name="value">знaчeние для корректировки</param>
        /// <param name="min">минимум</param>
        /// <param name="max">максимум</param>
        private int CorrentValue(int value, int min, int max)
        {
            if (value > max) return max;
            else if (value <= min) return min;
            else return value;
        }
    }
}