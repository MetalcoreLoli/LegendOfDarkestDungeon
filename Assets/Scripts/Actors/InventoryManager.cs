using Assets.Scripts.Items;
using Assets.Scripts.Items.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Actors
{
    public class InventoryManager : MonoBehaviour
    {

        public static InventoryManager instance = null;
        public bool IsOpen { get; set; }

        public Dictionary<Item, int> Items;
        [SerializeField] private int width;
        [SerializeField] private int height;
        private int cellWidth = 64;
        private int cellHeight = 64;
        public Vector3[] CellsPositions { get; private set; }
        public Vector3 SelectedCellPosition { get; private set; }


        private int selectedCellNumber = 0;
        private void Awake()
        {

            //if (instance == null)
            //    instance = this;
            //else if (instance != this)
            //    Destroy(gameObject);
            CellsPositions = new Vector3[width * height];
            Items = new Dictionary<Item, int>();

            AddItem(GameManager._instance.itemManager.GetItem("ManaPotion").GetComponent<ManaPotion>(), 5);
            //AddItem(GameManager._instance.itemManager.ManaPotion.GetComponent<ManaPotion>(), 5);
            AddItem(GameManager._instance.itemManager.GetItem("HealingPotion").GetComponent<HealingPotion>(), 5);

        }
        private void Start()
        {
            //style.font = Resources.Load<Font>("Sprites/GUI/SDS_6x6");
            //GameManager._instance.shortcutMenu.AddToShortcutMenu(GetItem("HealingPotion").gameObject, 4);
            //GameManager._instance.shortcutMenu.AddToShortcutMenu(GetItem("ManaPotion").gameObject, 3);
        }

        public void AddItem(Item item, int count = 1)
        {
            if (Items.ContainsKey(item))
                Items[item] += count;
            else
                Items.Add(item, count);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                IsOpen = !IsOpen;

            if (IsOpen)
            {
                if (Input.GetKeyDown(KeyCode.L))
                    MoveRight();

                if (Input.GetKeyDown(KeyCode.H))
                    MoveLeft();

                if (Input.GetKeyDown(KeyCode.J))
                    MoveUp();

                if (Input.GetKeyDown(KeyCode.K))
                    MoveDown();
                
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                     if (Items.Keys.Count() - 1 >= selectedCellNumber)
                    {
                        var item = Items.Keys.ToArray()[selectedCellNumber];
                        PlaceItemIntoShortcut(item, 3);
                    }

                }

                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    Debug.Log("Placing at 5");
                    if (Items.Keys.Count() - 1 >= selectedCellNumber)
                    {
                        var item = Items.Keys.ToArray()[selectedCellNumber];
                        PlaceItemIntoShortcut(item, 4);
                    }


                }

                SelectedCellPosition = CellsPositions[selectedCellNumber];
                Item[] keies = Items.Keys.Where(k => Items[k] == 0).ToArray();
                for (int i = 0; i < keies.Count(); i++)
                    Items.Remove(keies[i]);
            }
        }


        private void PlaceItemIntoShortcut(Item item, int number)
        {
            Debug.Log(item.name + " was placed");
            var shortcutMenu  = GameManager._instance.shortcutMenu;
            if (shortcutMenu.CanPlaceAt(number))
                shortcutMenu.AddToShortcutMenu(item.gameObject, number);
        }

        internal void RemoveOne(string name)
        {
            var item = GetItem(name);
            Items[item]--;
        }

        public void UseItem(string name)
        {
            if (CanUse(name))
            {
                Debug.Log($"use {name} ({Items[GetItem(name)]})");
                if (GetItem(name) is IUseable item)
                    item.Use();
                RemoveOne(name);
            }
        }
        public bool CanUse(string name)
        {
            if (Items[GetItem(name)] - 1 >= 0)
                return true;

            return false;
        }
        public Item GetItem(string name) => Items.Keys.FirstOrDefault(k => k.name == name || k.name == name + "(Clone)");
        private void OnGUI()
        {
            if (IsOpen)
            {
                GUIStyle style = new GUIStyle();


                var guiCell = Resources.Load<Texture2D>("Sprites/GUI/GUIEmptyInventoryCell");
                var font = Resources.Load<Font>("Sprites/GUI/SDS_8x8");

                int t_width = guiCell.width * 4;
                int t_height = guiCell.height * 4;

                var player = GameManager._instance.Player;
                GUI.skin.font = font;
                GUI.skin.font.material.SetColor("white", Color.white);
                GUI.skin.box.normal.textColor = Color.white;
                GUI.skin.box.wordWrap = true;
                GUI.skin.box.alignment = TextAnchor.MiddleCenter;


                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Vector3 position = new Vector3(x * t_width + t_width, y * t_height + t_height);
                        Vector3 size = new Vector3(t_width, t_height);

                        CellsPositions[x + width * y] = position;

                        GUI.DrawTexture(new Rect(position, size), guiCell, ScaleMode.StretchToFill);
                    }
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    var t = Items.Keys.ToArray()[i].gameObject.GetComponent<SpriteRenderer>();
                    GUI.DrawTexture(new Rect(i * t_width + t_width, t_height, t_width, t_width), t.sprite.FromSprite(), ScaleMode.StretchToFill);
                }

                string modToStr(int mod) => (mod > 0) ? $"+{mod}" : mod.ToString();

                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height, t_width * 3, t_height),
                    ($"Int: {player.Characteristics.Intelligence}({modToStr(player.Characteristics.IntelligenceMod)})"));

                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 2 + 1, t_width * 3, t_height),
                    ($"Str: {player.Characteristics.Strength}({modToStr(player.Characteristics.StrengthMod)})"));

                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 3 + 1, t_width * 3, t_height),
                    ($"Dex: {player.Characteristics.Dexterity}({modToStr(player.Characteristics.DexterityMod)})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 4 + 1, t_width * 3, t_height),
                    ($"Chr: {player.Characteristics.Charisma}({modToStr(player.Characteristics.CharismaMod)})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 5 + 1, t_width * 3, t_height),
                    ($"Lck: {player.Characteristics.Lucky}({modToStr(player.Characteristics.LuckyMod)})"));

                GUI.skin.box.alignment = TextAnchor.MiddleLeft;

                GUI.Box(
                    new Rect(t_width, t_height * 6, t_width * 9, t_height * 3),
                    "Use item: LCtrl;\n\n" +
                    "Add to shotcut menu: Alt + [number of free slot];\n\n" +
                    "Drop: G;\n\n" +
                    "Move: h(left), j(up), k(down), l(right);\n\n");

                SelecteCell(SelectedCellPosition);
                if (Items.Keys.Count - 1 >= selectedCellNumber)
                {
                    DrawItemDescription(Items.Keys.ToArray()[selectedCellNumber]);
                }
            }
        }

        void DrawItemDescription(Item item)
        {
            var position = new Vector3(cellWidth * 10, cellHeight);
            var size = new Vector3(cellWidth * 6, cellHeight * 4);
            GUI.Box(
                new Rect(position, size),
                $"Name: {item.Name}\n\n" +
                $"Count:{Items[item]}\n\n" +
                $"Description:\n\n" + item.Description);
        }
        void SelecteCell(Vector3 selectedCellPosition)
        {
            Vector3 position = selectedCellPosition;
            Vector3 size = new Vector3(64, 64);
            if (position != null)
                GUI.DrawTexture(
                    new Rect(position, size),
                    Resources.Load<Texture2D>("Sprites/GUI/GUISelectedCell"));
        }

        void MoveRight()
        {
            if (selectedCellNumber + 1 < width * height)
                selectedCellNumber++;
        }

        void MoveLeft()
        {
            if (selectedCellNumber - 1 >= 0)
                selectedCellNumber--;
        }

        void MoveUp()
        {
            int _selectedCellNumber = (int)(selectedCellNumber - width);
            if (_selectedCellNumber >= 0)
            {
                Debug.Log(_selectedCellNumber);
                selectedCellNumber = _selectedCellNumber;
            }
        }

        void MoveDown()
        {
            int _selectedCellNumber = (int)(selectedCellNumber + width);
            if (_selectedCellNumber < width * height)
                selectedCellNumber = _selectedCellNumber;
        }
    }
}
