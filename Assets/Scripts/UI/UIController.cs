using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        BarController hpContoroller;
        BarController mpContoroller;
        
        public GameObject selectedSpell;

        [SerializeField]
        private Text hpText;
        [SerializeField]
        private Text mpText;

        private Vector3[] spellsPositions;
        private void Awake()
        {
            selectedSpell = GameObject.Find("SelectedSpell");
            spellsPositions = new Vector3[5];
            var spellTransform = selectedSpell.transform;
            for (int i = 0; i < spellsPositions.Length; i++)
            {
                spellsPositions[i] 
                    = new Vector3(spellTransform.position.x + (16 * spellTransform.localScale.x) * i, spellTransform.position.y);
            }
        }

        private void Start()
        {
            hpContoroller = GameObject.FindGameObjectWithTag("HpBar").GetComponent<BarController>();
            mpContoroller = GameObject.FindGameObjectWithTag("MpBar").GetComponent<BarController>();
            hpContoroller.SetMax(GameManager._instance.playerCharacteristics.MaxHp);
            mpContoroller.SetMax(GameManager._instance.playerCharacteristics.MaxMp);
        }

        private void FixedUpdate()
        {
            selectedSpell = GameObject.Find("SelectedSpell");

            hpContoroller.SetValue(GameManager._instance.playerCharacteristics.Hp);
            mpContoroller.SetValue(GameManager._instance.playerCharacteristics.Mp);

            hpText.text = $"{GameManager._instance.playerCharacteristics.Hp} / {GameManager._instance.playerCharacteristics.MaxHp}";
            mpText.text = $"{GameManager._instance.playerCharacteristics.Mp} / {GameManager._instance.playerCharacteristics.MaxMp}";
        }

        public void SelectShortcutBarCell(int numberOfCell)
        {
            //if (SelectedSpell != null)
            selectedSpell.transform.position = spellsPositions[numberOfCell];
        }
    }
}
