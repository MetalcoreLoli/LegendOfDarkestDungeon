using Assets.Scripts.UI.Menu;
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
        [SerializeField] public LevelUpMenu         lvlMenu;
        [SerializeField] public OptionMenu          optionMenu;
        [SerializeField] public CreatingMenu        crtMenu;
        [SerializeField] public BookOfSpellMenu     bookOfSpellsMenu;
        [SerializeField] private Text hpText;
        [SerializeField] private Text mpText;

        public Vector3[] SpellsPositions { get; private set; }

        public BarController HpController { get => hpContoroller; }
        public BarController MpController { get => mpContoroller; }
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
            hpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxHp);
            mpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxMp);
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

            hpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Hp);
            mpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Mp);

            hpText.text = $"{GameManager.Instance.playerCharacteristics.Hp} / {GameManager.Instance.playerCharacteristics.MaxHp}";
            mpText.text = $"{GameManager.Instance.playerCharacteristics.Mp} / {GameManager.Instance.playerCharacteristics.MaxMp}";
        }

        public void SelectShortcutBarCell(int numberOfCell)
        {
            //if (SelectedSpell != null)
            selectedSpell.transform.position = SpellsPositions[numberOfCell];
        }
    }
}
