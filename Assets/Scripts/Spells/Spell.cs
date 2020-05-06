using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    public abstract class Spell : MonoBehaviour
    {
        public SpellInfo Info;

        public Transform FirePoint;
        public Transform FirePointUp;

        public Rigidbody2D rb2D;
        public abstract void Cast();
    }
}
