using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    public class Inventory : MonoBehaviour
    {

        public List<Item> Items;

        private void Awake()
        {
            Items = new List<Item>();
        }
    }
}
