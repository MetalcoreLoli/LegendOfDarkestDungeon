using System;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    [Serializable]
    public struct SpellInfo
    {
        public string Name       ;
        public int Cost          ;
        public GameObject Prefab;

        public SpellInfo(string name, int cost, GameObject prefab)
        {
            Name    = name;
            Cost    = cost;
            Prefab  = prefab;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Mana: Cost: {Cost};";
        }
    }
}
