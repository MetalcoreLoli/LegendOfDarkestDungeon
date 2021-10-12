using Assets.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        private BarController hpContoroller;
        private BarController mpContoroller;
        
        [SerializeField]private Transform _hpBar;
        [SerializeField]private Transform _mpBar;

        public GameObject selectedSpell;

        [SerializeField] private GameMenuSettings gameMenu;
        [SerializeField] public LevelUpMenu lvlMenu;
        [SerializeField] public OptionMenu optionMenu;
        [SerializeField] public CreatingMenu crtMenu;
        [SerializeField] public BookOfSpellMenu bookOfSpellsMenu;
        [SerializeField] private Text hpText;
        [SerializeField] private Text mpText;

        public Vector3[] SpellsPositions { get; private set; }

        public BarController HpController { get => hpContoroller; }
        public BarController MpController { get => mpContoroller; }

        private void Awake()
        {
            hpContoroller = _hpBar.GetComponent<BarController>();
            mpContoroller = _mpBar.GetComponent<BarController>();
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
            hpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxHp);
            mpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxMp);
            gameMenu.Close();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
        }

        public void SelectShortcutBarCell(int numberOfCell)
        {
            //if (SelectedSpell != null)
            selectedSpell.transform.position = SpellsPositions[numberOfCell];
        }
    }
}