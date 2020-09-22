using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TextPopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMeshPro;

        [SerializeField] private float livingTime;
        [SerializeField] private float textSpeed;
        public static void CreateAt(Vector3 postion, int damage, Transform prefab)
        {
            Transform popUpTransform = Instantiate(prefab, postion,  Quaternion.identity);
            TextPopUp damagePopUp = popUpTransform.GetComponent<TextPopUp>();
            damagePopUp.SetUp(damage);
           // return damagePopUp;
        }
        public static void CreateWithColor(Vector3 postion, string text, Transform prefab, Color color)
        {
            Transform popUpTransform = Instantiate(prefab, postion, Quaternion.identity);
            TextPopUp damagePopUp = popUpTransform.GetComponent<TextPopUp>();
            damagePopUp.SetUp(text, color);
            //return damagePopUp;
        }

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }

        public void SetUp(int damageAmount) {
            textMeshPro.SetText(damageAmount.ToString());
        }
        public void SetUp(string text, Color color)
        {
            textMeshPro.color = color;
            textMeshPro.SetText(text);
        }

        private void Update()
        {
            transform.position += new Vector3(0, Mathf.Sin(textSpeed * Time.deltaTime));

            livingTime -= Time.deltaTime;

            if (livingTime < 0)
                Destroy(gameObject);
        }
    }
}
