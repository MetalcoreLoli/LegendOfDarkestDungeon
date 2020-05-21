﻿using Assets.Scripts.Dices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items.Potions
{
    public class HealingPotion : Item, IPotion 
    {
        public override void Drop()
        {
            throw new NotImplementedException();
        }
        public override void Take()
        {
            base.Take();
        }

        public void Use()
        {
            int value = DiceManager.RollDice("1d4");
            GameObject.Find("Player").GetComponent<Player>().UpdateHealth(value);
        }
    }
}
