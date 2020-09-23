using Assets.Scripts.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class BookOfSpellMenu : Menu
    {
        [SerializeField] private Sprite inventoryTile;
        [SerializeField] private Sprite inventorySelectedTile;
        [SerializeField] private int tileWidth = 64;
        [SerializeField] private int tileHeight = 64;
        [SerializeField] private int countOfTiles = 3;
        [SerializeField] private List<SpellInfo> spellInfos;
        [SerializeField] private List<Vector3> tilesCoords;

        private void Awake()
        {
            spellInfos  = new List<SpellInfo>();
            tilesCoords = new List<Vector3>();

            inventoryTile           = Resources.Load<Sprite>("Sprites/GUI/GUIEmptyInventoryCell");
            inventorySelectedTile   = Resources.Load<Sprite>("Sprites/GUI/GUISelectedCell");
        }

        public override void Close()
        {
            IsOpen = false;
            GameManager.Instance.enabled           = true;
            GameManager.Instance.Player.enabled    = true;
        }

        public override void Open()
        {
            IsOpen = true;
            GameManager.Instance.enabled           = false;
            GameManager.Instance.Player.enabled    = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && !IsOpen)
                Open();

            if (Input.GetKeyDown(KeyCode.B) && IsOpen)
                Close();
        }

        private void OnGUI()
        {
            if (IsOpen)
            {
                for (int x = 0; x < countOfTiles; x++)
                {
                    for (int y = 0; y < countOfTiles; y++)
                    {
                        Vector3 pos = new Vector3(x + tileWidth, y + tileHeight);
                        Vector3 size = new Vector3(tileWidth, tileHeight);

                        var tex = inventoryTile.FromSpriteWithFilterMode(FilterMode.Point);

                        GUI.DrawTexture(
                            new Rect(pos, size),
                            tex,
                            ScaleMode.StretchToFill);
                    }
                }
            }
        }
    }
}
