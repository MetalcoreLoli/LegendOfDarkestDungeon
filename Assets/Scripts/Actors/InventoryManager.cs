using Assets.Scripts.Items;
using Assets.Scripts.Items.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            AddItem(GameManager._instance.itemManager.ManaPotion.GetComponent<ManaPotion>(), 5);
            AddItem(GameManager._instance.itemManager.HealingPotion.GetComponent<HealingPotion>(), 5);

        }
        private void Start()
        {
            GameManager._instance.shortcutMenu.AddToShortcutMenu(GetItem("HealingPotion").gameObject, 4);
            GameManager._instance.shortcutMenu.AddToShortcutMenu(GetItem("ManaPotion").gameObject, 3);
        }

        public void AddItem(Item item, int count = 1)
        {
            Debug.Log(item.Name + " was add to inventory");
            Items.Add(item, count);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                IsOpen = !IsOpen;

            if (IsOpen)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                    MoveNext();

                if (Input.GetKeyDown(KeyCode.LeftShift))
                    MoveBack();

                SelectedCellPosition = CellsPositions[selectedCellNumber];
                Item[] keies = Items.Keys.Where(k => Items[k] == 0).ToArray();
                for (int i = 0; i < keies.Count(); i++)
                    Items.Remove(keies[i]);
            }
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
        public Item GetItem(string name) => Items.Keys.FirstOrDefault(k => k.Name == name);
        private void OnGUI()
        {
            if (IsOpen)
            {
                var guiCell = Resources.Load<Texture2D>("Sprites/GUI/GUIEmptyInventoryCell");
                var font = Resources.Load<Font>("Sprites/GUI/SDS_6x6");
                int t_width = guiCell.width * 4;
                int t_height = guiCell.height * 4;

                var player = GameManager._instance.Player;
                GUI.skin.font = font;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Vector3 position    = new Vector3(x * t_width + t_width, y * t_height + t_height);
                        Vector3 size        = new Vector3(t_width, t_height);
                        
                        CellsPositions[x + width * y] = position;

                        GUI.DrawTexture(new Rect(position, size), guiCell, ScaleMode.StretchToFill);
                    }
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    var t = Items.Keys.ToArray()[i].gameObject.GetComponent<SpriteRenderer>();
                    GUI.DrawTexture(new Rect(i * t_width + t_width, t_height, t_width, t_width), t.sprite.FromSprite(), ScaleMode.StretchToFill);
                }


                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height, t_width * 2, t_height),
                    ($"Int: {player.Characteristics.Intelligence}({player.Characteristics.IntelligenceMod})"));
                
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 2 + 1, t_width * 2, t_height),
                    ($"Str: {player.Characteristics.Strength}({player.Characteristics.StrengthMod})"));

                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 3 + 1, t_width * 2, t_height),
                    ($"Dex: {player.Characteristics.Dexterity}({player.Characteristics.DexterityMod})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 4 + 1, t_width * 2, t_height),
                    ($"Chr: {player.Characteristics.Charisma}({player.Characteristics.CharismaMod})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 5 + 1, t_width * 2, t_height),
                    ($"Lck: {player.Characteristics.Lucky}({player.Characteristics.LuckyMod})"));

                GUI.Box(
                    new Rect(t_width, t_height * 6, t_width * 8, t_height * 2),
                    "Use item: LCtrl;\n\nAdd to shotcut menu: Alt + [number of free slot];\n\nDrop: G");

                SelecteCell(SelectedCellPosition);
            }
        }

        void SelecteCell(Vector3 selectedCellPosition)
        {
            Vector3 position    = selectedCellPosition;
            Vector3 size        = new Vector3(64, 64);
            if (position != null)
                GUI.DrawTexture(
                    new Rect(position, size),
                    Resources.Load<Texture2D>("Sprites/GUI/GUISelectedCell"));
        }

        void MoveNext()
        {
            if (selectedCellNumber + 1 < width * height)
                selectedCellNumber++;
        }

        void MoveBack()
        {
            if (selectedCellNumber - 1 >= 0)
                selectedCellNumber--;
        }
    }
}
