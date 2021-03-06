﻿using UnityEngine;

namespace Assets.Scripts.Spells
{
    public abstract class Spell : MonoBehaviour
    {
        public SpellInfo Info;

        public Transform FirePoint;
        public Transform FirePointUp;
        public SpriteRenderer Icon;
        public Rigidbody2D rb2D;

        public abstract void Cast(Transform caster);
    }
}