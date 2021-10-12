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

        [SerializeField] private GameMenuSettings gameMenu;
        [SerializeField] public LevelUpMenu lvlMenu;
        [SerializeField] public OptionMenu optionMenu;
        [SerializeField] public CreatingMenu crtMenu;
        [SerializeField] public BookOfSpellMenu bookOfSpellsMenu;

        public Vector3[] SpellsPositions { get; private set; }

        public BarController HpController { get => hpContoroller; }
        public BarController MpController { get => mpContoroller; }

        private void Awake()
        {
            hpContoroller = _hpBar.GetComponent<BarController>();
            mpContoroller = _mpBar.GetComponent<BarController>();
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
            hpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Hp);
            mpContoroller.SetValue(GameManager.Instance.playerCharacteristics.Mp);
        }
    }
}