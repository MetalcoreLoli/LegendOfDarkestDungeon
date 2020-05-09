using Assets.Scripts.Dices;
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
                player.Characteristics.Hp += DiceManager.RollDice("1d4");
                var light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
                light.intensity = player.Characteristics.Hp;
            }
        }
    }
}
