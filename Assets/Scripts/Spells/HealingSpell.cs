using Assets.Scripts.Actors;
using Assets.Scripts.Dices;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    public class HealingSpell : Spell
    {
        public override void Cast(Transform caster)
        {
            var actor = caster.gameObject.GetComponent<Actor>();
            int heal = DiceManager.RollDice("1d4");
            actor.UpdateMana(-Info.Cost);
            actor.UpdateHealth(heal);
        }
    }
}