using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class BarController : MonoBehaviour
    {
        public Slider Slider;

        [SerializeField] private string _textFormat = "{0} / {1}";
        [SerializeField] private Text _text;
       
        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }

        private void FixedUpdate()
        {
            _text.text = String.Format(_textFormat, Slider.minValue, Slider.maxValue);
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