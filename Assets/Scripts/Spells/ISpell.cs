using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    public interface ISpell
    {
        SpellInfo Info { get; set; }
        void Cast();
    }
}
