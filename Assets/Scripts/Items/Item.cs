using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Dices;

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
        
        public string Name          = "Item";

        [Multiline] public string Description   = "";
        
        protected virtual void Awake()
        {
            Info = new ItemInfo(Prefab, Name);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && IsPlayerStay())
                Take();
        }
        
        public virtual void Drop()
        {
        }

        protected virtual bool IsPlayerStay() 
        {
            var player = GameObject.Find("Player");
            return transform.position == player.transform.position;
        }
        public virtual void Take()
        {
            GameManager._instance.inventoryManager.AddItem(this, DiceManager.RollDice("1d4"));
            gameObject.SetActive(false);
        }
    }
}
