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

        private GameMenuSettings gameMenu;
        private LevelUpMenu lvlMenu;
        private OptionMenu optionMenu;
        private CreatingMenu crtMenu;
        private BookOfSpellMenu bookOfSpellsMenu;
        
        [SerializeField] private Transform _gameMenuLink;
        [SerializeField] private Transform _lvlMenuLink;
        [SerializeField] private Transform _optionMenuLink;
        [SerializeField] private Transform _crtMenuLink;
        [SerializeField] private Transform _bookOfSpellsMenuLink;

        public Vector3[] SpellsPositions { get; private set; }

        public BarController HpController { get => hpContoroller; }
        public BarController MpController { get => mpContoroller; }

        private void Awake()
        {
            hpContoroller = _hpBar.GetComponent<BarController>();
            mpContoroller = _mpBar.GetComponent<BarController>();

            gameMenu = _gameMenuLink.GetComponent<GameMenuSettings>();
            lvlMenu = _lvlMenuLink.GetComponent<LevelUpMenu>();
            optionMenu = _optionMenuLink.GetComponent<OptionMenu>();
            crtMenu = _crtMenuLink.GetComponent<CreatingMenu>();
        }

        private void Start()
        {
            hpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxHp);
            mpContoroller.SetMax(GameManager.Instance.playerCharacteristics.MaxMp);
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
            hpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Hp);
            mpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Mp);
        }
    }
}