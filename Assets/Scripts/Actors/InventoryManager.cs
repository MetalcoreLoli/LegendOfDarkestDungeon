using Assets.Scripts.Items;
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

        private void Awake()
        {
            //if (instance == null)
            //    instance = this;
            //else if (instance != this)
            //    Destroy(gameObject);

            Items = new Dictionary<Item, int>()
            {
                [GameManager._instance.itemManager.HealingPotion.GetComponent<Item>()] = 5
            };

        }
        private void Start()
        {
            GameManager._instance.shortcutMenu.AddToShortcutMenu(GetItem("HealingPotion").gameObject, 4);    
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                IsOpen = !IsOpen;
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
        public Item GetItem(string name) => Items.Keys.First(k => k.Name == "HealingPotion");
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
                        GUI.DrawTexture(new Rect(x * t_width + t_width, y * t_height + t_height, t_width, t_width), guiCell, ScaleMode.StretchToFill);
                    }
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    var t = Items.Keys.ToArray()[i].gameObject.GetComponent<SpriteRenderer>();
                    GUI.DrawTexture(new Rect(i * t_width + t_width, t_height, t_width, t_width), t.sprite.texture, ScaleMode.StretchToFill);
                }


                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height, t_width * 2, t_height),
                    new GUIContent($"Int: {player.Characteristics.Intelligence}({player.Characteristics.IntelligenceMod})"));
                
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 2 + 1, t_width * 2, t_height),
                    new GUIContent($"Str: {player.Characteristics.Strength}({player.Characteristics.StrengthMod})"));

                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 3 + 1, t_width * 2, t_height),
                    new GUIContent($"Dex: {player.Characteristics.Dexterity}({player.Characteristics.DexterityMod})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 4 + 1, t_width * 2, t_height),
                    new GUIContent($"Chr: {player.Characteristics.Charisma}({player.Characteristics.CharismaMod})"));
                GUI.Box(
                    new Rect(t_width * 6 + t_width, t_height * 5 + 1, t_width * 2, t_height),
                    new GUIContent($"Lck: {player.Characteristics.Lucky}({player.Characteristics.LuckyMod})"));
                


            }
        }

    }
}
