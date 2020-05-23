using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [CreateAssetMenu(fileName = "DamagePopUp", menuName = "new text", order = 52)]
    public class DamageDealer : ScriptableObject
    {
        [SerializeField] private int damage;
        [SerializeField] private GameObject text;
        public int Damage { get => damage; set => damage = value; }
        public GameObject Text { get => text; set => text = value; }
    }

  
}
