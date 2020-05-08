using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public struct ItemInfo
    {
        public GameObject Prefab { get; set; }
        public string Name { get; set; }

        public ItemInfo(GameObject Prefab, string Name)
        {
            this.Prefab = Prefab;
            this.Name   = Name;
        }
    }
    public abstract class Item : MonoBehaviour, ITakable, IDropable
    {
        public ItemInfo Info;
        public GameObject Prefab;
        public string Name = "Item";

        protected virtual void Awake()
        {
            Info = new ItemInfo(Prefab, Name);
        }

        public virtual void Drop()
        {
        }

        public virtual void Take()
        {
        }
    }
}
