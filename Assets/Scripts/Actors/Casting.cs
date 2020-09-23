using Assets.Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public Transform FirePoint;
    public Transform FirePointUp;

    public Transform Caster;

    public GameObject[] SpellPrefabs;

    public void CastSpellWithIndex(int index)
    {
        var spell           = SpellPrefabs[index].GetComponent<Spell>();
        spell.FirePoint     = FirePoint;
        spell.FirePointUp   = FirePointUp;
        
        spell.Cast(Caster);
    }

    public void CastSpellWithName(string name)
    {
        var spell           = SpellPrefabs.First(s => s.name == name).GetComponent<Spell>();
        spell.FirePoint     = FirePoint;
        spell.FirePointUp   = FirePointUp;
        spell.Cast(Caster);
    }
}
