using Assets.Scripts.Dices;
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
            var light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
            int value = DiceManager.RollDice("1d4");

            GameManager._instance.Player.UpdateHealth(value);
            
            light.intensity += value;
        }
    }
}
