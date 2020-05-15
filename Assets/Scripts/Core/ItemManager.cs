using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Items;

namespace Assets.Scripts.Core
{
    public class ItemManager : MonoBehaviour
    {

        public GameObject[] Items;

        public void DropAt(Vector3 position, string name)
        { 
            GameManager._instance.boardManager.SpawnObject(position, GetItem(name).gameObject);    
        }

        public Item GetItem(string name)
        {
            return Items.FirstOrDefault(item => item.name == name).GetComponent<Item>();
        }
    }
}
