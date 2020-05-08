using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class ShortcutMenu : MonoBehaviour
    {
        public GameObject[] Objects;
        public GameObject[] Cells;
        private UIController uIController;
        private void Awake()
        {
           
            for (int i = 0; i < Cells.Length; i++)
            {
                Debug.Log(Cells[i].name);
            }

            var playerCasting = GameObject.Find("Player").GetComponent<Casting>();
            //Cells[0].GetComponent<Image>().sprite = playerCasting.SpellPrefabs[0].GetComponent<SpriteRenderer>().sprite;
            //Cells[1].GetComponent<Image>().sprite = playerCasting.SpellPrefabs[1].GetComponent<SpriteRenderer>().sprite;
        }
        public void Init()
        {
            Cells = new GameObject[5];

            // uIController = GameObject.Find("UIController").GetComponent<UIController>();
            uIController = GameObject.Find("HUDCanvas").GetComponent<UIController>();

            Cells[0] = GameObject.Find("CellOne");
            Cells[1] = GameObject.Find("CellTwo");
            Cells[2] = GameObject.Find("CellTree");
            Cells[3] = GameObject.Find("CellFour");
            Cells[4] = GameObject.Find("CellFive");
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                uIController.SelectShortcutBarCell(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                uIController.SelectShortcutBarCell(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                uIController.SelectShortcutBarCell(2);

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                uIController.SelectShortcutBarCell(3);

            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                uIController.SelectShortcutBarCell(4);
            }
        }

        public void AddToShortcutMenu(GameObject @object, int numberOfCell)
        {
            Cells[numberOfCell].GetComponent<Image>().sprite = @object.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
