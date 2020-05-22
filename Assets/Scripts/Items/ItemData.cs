using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName ="new item data", menuName = "Items Data", order = 51)]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField, Multiline] private string decription;
        [SerializeField] private Sprite icon;
        public string Name { get => _name; }
        public string Description { get => decription; }
        public Sprite Icon { get => icon; }
    }
}
