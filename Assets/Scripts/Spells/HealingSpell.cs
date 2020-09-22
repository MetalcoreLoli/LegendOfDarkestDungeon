using Assets.Scripts.Dices;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    public class HealingSpell : Spell
    {
        public override void Cast()
        {
            var player = GameObject.Find("Player").GetComponent<Player>();

            if (player.PlayerCastSpell(2))
            {
                int heal = DiceManager.RollDice("1d4");
                //TextPopUp.CreateAt(transform.position, heal, player.DamageDealer.Text.transform);
                player.UpdateHealth(heal);
            }
        }
    }
}
