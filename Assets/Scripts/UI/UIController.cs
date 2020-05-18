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

        [SerializeField] private GameMenuSettings   gameMenu;
        [SerializeField] public OptionMenu         optionMenu;
        [SerializeField] public CreatingMenu        crtMenu;
        [SerializeField] private Text hpText;
        [SerializeField] private Text mpText;

        public Vector3[] SpellsPositions { get; private set; }
        private void Awake()
        {
            selectedSpell = GameObject.Find("SelectedSpell");
            SpellsPositions = new Vector3[5];
            var spellTransform = selectedSpell.transform;
            for (int i = 0; i < SpellsPositions.Length; i++)
            {
                SpellsPositions[i] 
                    = new Vector3(spellTransform.position.x + (16 * spellTransform.localScale.x) * i, spellTransform.position.y);
            }
        }

        private void Start()
        {
            hpContoroller = GameObject.FindGameObjectWithTag("HpBar").GetComponent<BarController>();
            mpContoroller = GameObject.FindGameObjectWithTag("MpBar").GetComponent<BarController>();
            hpContoroller.SetMax(GameManager._instance.playerCharacteristics.MaxHp);
            mpContoroller.SetMax(GameManager._instance.playerCharacteristics.MaxMp);
            gameMenu.Close();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) )
            {
                if (!gameMenu.IsOpen && !crtMenu.IsOpen)
                    gameMenu.Open();
            }
            
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
            selectedSpell.transform.position = SpellsPositions[numberOfCell];
        }
    }
}
