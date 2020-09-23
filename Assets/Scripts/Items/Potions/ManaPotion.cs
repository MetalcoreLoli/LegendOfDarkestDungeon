using Assets.Scripts.Dices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items.Potions
{

    public class ManaPotion : Item, IUseable
    {
        public void Use()
        {
            Debug.Log(nameof(ManaPotion));
            GameManager.Instance.Player.UpdateMana(DiceManager.RollDice("1d4"));
        }
    }
}
