using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    [CreateAssetMenu(fileName = "new spell data", menuName = "Spell Data", order = 54)]

    public class SpellInfo : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int cost;
        [SerializeField, Multiline] private string description;
        [SerializeField] private GameObject prefab;

        public String Name { get => _name; set => _name = value; }
        public int Cost { get => cost; set => cost = value; }
        public Sprite Icon { get => _icon; set => _icon = value; }
        public string Description { get => description; set => description = value; }
        public GameObject Prefab { get => prefab; set => prefab = value; }
    }
}
