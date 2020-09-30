using Assets.Scripts.Dices;
using UnityEngine;

namespace Assets.Scripts.Items.Potions
{
    public class ManaPotion : Item, IUseable
    {
        public void Use()
        {
            Debug.Log(nameof(ManaPotion));
            GameManager.Instance.PlayerActor.UpdateMana(DiceManager.RollDice("1d4"));
        }
    }
}