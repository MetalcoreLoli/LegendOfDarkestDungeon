using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class BarController : MonoBehaviour
    {
        public Slider Slider;

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }

        public void SetMin(int value)
        {
            Slider.minValue = value;
        }

        public void SetMax(int value)
        {
            Slider.maxValue = value;
            Slider.value = value;
        }

        public void SetValue(int value)
        { 
            Slider.value = value;
        }
    }
}
