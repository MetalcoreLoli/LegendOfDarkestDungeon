using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items.Potions
{
    public class Potion : Item, IPotion 
    {
        public override void Drop()
        {
            throw new NotImplementedException();
        }
        public override void Take()
        {
            throw new NotImplementedException();
        }

        public void Use()
        {
            throw new NotImplementedException();
        }
    }
}
